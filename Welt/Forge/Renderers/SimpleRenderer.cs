#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.IO;
using Welt.Models;
using Welt.Processors;
using Welt.Types;

namespace Welt.Forge.Renderers
{
    internal class SimpleRenderer : IRenderer
    {
        private const byte BUILD_RANGE = 4;
        private const byte LIGHT_RANGE = BUILD_RANGE + 1;
        private const byte GENERATE_RANGE_LOW = LIGHT_RANGE + 1;
        private const byte GENERATE_RANGE_HIGH = GENERATE_RANGE_LOW;
        private readonly FirstPersonCamera m_camera;
        private readonly GraphicsDevice m_graphicsDevice;
        private readonly World m_world;
        private LightingChunkProcessor m_lightingChunkProcessor;
        protected Effect SolidBlockEffect;
        protected Texture2D TextureAtlas;
        private float m_tod;
        private bool m_isRunning;
        private VertexBuildChunkProcessor m_vertexBuildChunkProcessor;
        protected Effect WaterBlockEffect;
        protected Effect GrassBlockEffect;

        public SimpleRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            m_graphicsDevice = graphicsDevice;
            m_camera = camera;
            m_world = world;
            m_isRunning = true;
        }

        public void Initialize()
        {
            m_vertexBuildChunkProcessor = new VertexBuildChunkProcessor(m_graphicsDevice);
            m_lightingChunkProcessor = new LightingChunkProcessor();

            Debug.WriteLine("Generate initial chunks");
            m_world.VisitChunks(DoGenerate, GENERATE_RANGE_HIGH);
            Debug.WriteLine("Light initial chunks");
            m_world.VisitChunks(DoLighting, LIGHT_RANGE);
            Debug.WriteLine("Build initial chunks");
            m_world.VisitChunks(DoBuild, BUILD_RANGE);
        }

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Textures\\terrain");
            SolidBlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
            WaterBlockEffect = content.Load<Effect>("Effects\\WaterBlockEffect");
            GrassBlockEffect = content.Load<Effect>("Effects\\GrassBlockEffect");
        }

        public void Update(GameTime gameTime)
        {
            if (!m_isRunning) return;
        }

        public void Draw(GameTime gameTime)
        {
            if (!m_isRunning) return;
            DrawSolid(gameTime);
            DrawGrass(gameTime);
            DrawWater(gameTime);
        }

        public void Stop()
        {
            m_isRunning = false;
        }

        public void RebuildChunk(Chunk rebuildChunk)
        {
            switch (rebuildChunk.State)
            {
                case ChunkState.AwaitingRelighting:
                    DoLighting(rebuildChunk);
                    DoBuild(rebuildChunk);
                    rebuildChunk.State = ChunkState.Ready;
                    break;
                case ChunkState.AwaitingRebuild:
                    DoBuild(rebuildChunk);
                    rebuildChunk.State = ChunkState.Ready;
                    break;
            }
        }

        #region DrawSolid

        private void DrawSolid(GameTime gameTime)
        {
            m_tod = m_world.Tod;

            SolidBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            SolidBlockEffect.Parameters["View"].SetValue(m_camera.View);
            SolidBlockEffect.Parameters["Projection"].SetValue(m_camera.Projection);
            SolidBlockEffect.Parameters["CameraPosition"].SetValue(m_camera.Position);
            SolidBlockEffect.Parameters["FogNear"].SetValue(m_world.Fognear);
            SolidBlockEffect.Parameters["FogFar"].SetValue(m_world.Fogfar);
            SolidBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);

            SolidBlockEffect.Parameters["HorizonColor"].SetValue(m_world.Horizoncolor);
            SolidBlockEffect.Parameters["NightColor"].SetValue(m_world.Nightcolor);

            SolidBlockEffect.Parameters["MorningTint"].SetValue(m_world.Morningtint);
            SolidBlockEffect.Parameters["EveningTint"].SetValue(m_world.Eveningtint);

            SolidBlockEffect.Parameters["SunColor"].SetValue(m_world.Suncolor);
            SolidBlockEffect.Parameters["timeOfDay"].SetValue(m_tod);

            var viewFrustum = new BoundingFrustum(m_camera.View*m_camera.Projection);

            m_graphicsDevice.BlendState = BlendState.Opaque;
            m_graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in SolidBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in m_world.Chunks.Values.Where(chunk => chunk != null))
                {
                    if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);

                    if (!chunk.BoundingBox.Intersects(viewFrustum) || chunk.IndexBuffer == null) continue;
                    if (chunk.IndexBuffer.IndexCount <= 0) continue;
                    m_graphicsDevice.SetVertexBuffer(chunk.VertexBuffer);
                    m_graphicsDevice.Indices = chunk.IndexBuffer;
                    //_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                    //    chunk.VertexBuffer.VertexCount, 0, chunk.IndexBuffer.IndexCount/3);
                    m_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.VertexCount);
                }
            }
        }

        #endregion

        private Chunk DoLighting(Vector3I chunkIndex)
        {
            var chunk = m_world.Chunks[chunkIndex.X, chunkIndex.Z];
            return DoLighting(chunk);
        }

        private Chunk DoLighting(Chunk chunk)
        {
            m_lightingChunkProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private Chunk DoBuild(Vector3I chunkIndex)
        {
            var chunk = m_world.Chunks[chunkIndex.X, chunkIndex.Z];
            return DoBuild(chunk);
        }

        private Chunk DoBuild(Chunk chunk)
        {
            m_vertexBuildChunkProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private Chunk DoGenerate(Vector3I chunkIndex)
        {
            var chunk = new Chunk(m_world, chunkIndex);
            return DoGenerate(chunk);
        }

        private Chunk DoGenerate(Chunk chunk)
        {
            m_world.Chunks[chunk.Index.X, chunk.Index.Z] = chunk;
            m_world.Generator.Generate(chunk);
            return chunk;
        }

        #region DrawGrass

        private float m_waveTime;

        private void DrawGrass(GameTime time)
        {
            m_waveTime += 0.05f;
            GrassBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            GrassBlockEffect.Parameters["View"].SetValue(m_camera.View);
            GrassBlockEffect.Parameters["Projection"].SetValue(m_camera.Projection);
            GrassBlockEffect.Parameters["CameraPosition"].SetValue(m_camera.Position);
            GrassBlockEffect.Parameters["Texture0"].SetValue(TextureAtlas);
            GrassBlockEffect.Parameters["WaveTime"].SetValue(m_waveTime);
            var viewFrustum = new BoundingFrustum(m_camera.View*m_camera.Projection);

            m_graphicsDevice.BlendState = BlendState.Opaque;
            m_graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in GrassBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in from chunk in m_world.Chunks.Values
                                      where chunk != null
                                      where chunk.BoundingBox.Intersects(viewFrustum)
                                      //where chunk.GrassVertexBuffer != null
                                      //where chunk.GrassIndexBuffer.IndexCount > 0
                                      select chunk)
                {
                    //_graphicsDevice.SetVertexBuffer(chunk.GrassVertexBuffer);
                    //_graphicsDevice.Indices = chunk.GrassIndexBuffer;
                    //_graphicsDevice.DrawIndexPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.GrassVertexCount);
                }
            }
        }

        #endregion

        #region DrawWater

        private float m_rippleTime;

        private void DrawWater(GameTime gameTime)
        {
            m_rippleTime += 0.05f;

            m_tod = m_world.Tod;
            
            WaterBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            WaterBlockEffect.Parameters["View"].SetValue(m_camera.View);
            WaterBlockEffect.Parameters["Projection"].SetValue(m_camera.Projection);
            WaterBlockEffect.Parameters["CameraPosition"].SetValue(m_camera.Position);
            WaterBlockEffect.Parameters["FogNear"].SetValue(m_world.Fognear);
            WaterBlockEffect.Parameters["FogFar"].SetValue(m_world.Fogfar);
            WaterBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);
            WaterBlockEffect.Parameters["SunColor"].SetValue(m_world.Suncolor);

            WaterBlockEffect.Parameters["HorizonColor"].SetValue(m_world.Horizoncolor);
            WaterBlockEffect.Parameters["NightColor"].SetValue(m_world.Nightcolor);

            WaterBlockEffect.Parameters["MorningTint"].SetValue(m_world.Morningtint);
            WaterBlockEffect.Parameters["EveningTint"].SetValue(m_world.Eveningtint);

            WaterBlockEffect.Parameters["timeOfDay"].SetValue(m_tod);
            WaterBlockEffect.Parameters["RippleTime"].SetValue(m_rippleTime);

            var viewFrustum = new BoundingFrustum(m_camera.View*m_camera.Projection);

            m_graphicsDevice.BlendState = BlendState.NonPremultiplied;
            m_graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in WaterBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in from chunk in m_world.Chunks.Values
                    where chunk != null
                    where chunk.BoundingBox.Intersects(viewFrustum) && chunk.WaterVertexBuffer != null
                    where chunk.WaterIndexBuffer.IndexCount > 0
                    select chunk)
                {
                    m_graphicsDevice.SetVertexBuffer(chunk.WaterVertexBuffer);
                    m_graphicsDevice.Indices = chunk.WaterIndexBuffer;
                    m_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                        0, chunk.WaterVertexCount);
                }
            }
        }

        #endregion
    }
}