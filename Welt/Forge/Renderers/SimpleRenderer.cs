#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Models;
using Welt.Processors;
using Welt.Types;

namespace Welt.Forge.Renderers
{
    internal class SimpleRenderer : IRenderer
    {
        private const byte BUILD_RANGE = 2;
        private const byte LIGHT_RANGE = BUILD_RANGE + 1;
        private const byte GENERATE_RANGE_LOW = LIGHT_RANGE + 1;
        private const byte GENERATE_RANGE_HIGH = GENERATE_RANGE_LOW;
        private readonly FirstPersonCamera _camera;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly World _world;
        private LightingChunkProcessor _lightingChunkProcessor;
        protected Effect SolidBlockEffect;
        protected Texture2D TextureAtlas;
        private float _tod;
        private VertexBuildChunkProcessor _vertexBuildChunkProcessor;
        protected Effect WaterBlockEffect;

        public SimpleRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            _graphicsDevice = graphicsDevice;
            _camera = camera;
            _world = world;
        }

        public void Initialize()
        {
            _vertexBuildChunkProcessor = new VertexBuildChunkProcessor(_graphicsDevice);
            _lightingChunkProcessor = new LightingChunkProcessor();

            Debug.WriteLine("Generate initial chunks");
            _world.VisitChunks(DoGenerate, GENERATE_RANGE_HIGH);
            Debug.WriteLine("Light initial chunks");
            _world.VisitChunks(DoLighting, LIGHT_RANGE);
            Debug.WriteLine("Build initial chunks");
            _world.VisitChunks(DoBuild, BUILD_RANGE);
        }

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Textures\\terrain");
            SolidBlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
            WaterBlockEffect = content.Load<Effect>("Effects\\WaterBlockEffect");
        }

        public void Update(GameTime gameTime)
        {
        }

        public void Draw(GameTime gameTime)
        {
            DrawSolid(gameTime);
            DrawWater(gameTime);
        }

        public void Stop()
        {
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
            _tod = _world.Tod;

            SolidBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            SolidBlockEffect.Parameters["View"].SetValue(_camera.View);
            SolidBlockEffect.Parameters["Projection"].SetValue(_camera.Projection);
            SolidBlockEffect.Parameters["CameraPosition"].SetValue(_camera.Position);
            SolidBlockEffect.Parameters["FogNear"].SetValue(_world.Fognear);
            SolidBlockEffect.Parameters["FogFar"].SetValue(_world.Fogfar);
            SolidBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);

            SolidBlockEffect.Parameters["HorizonColor"].SetValue(_world.Horizoncolor);
            SolidBlockEffect.Parameters["NightColor"].SetValue(_world.Nightcolor);

            SolidBlockEffect.Parameters["MorningTint"].SetValue(_world.Morningtint);
            SolidBlockEffect.Parameters["EveningTint"].SetValue(_world.Eveningtint);

            SolidBlockEffect.Parameters["SunColor"].SetValue(_world.Suncolor);
            SolidBlockEffect.Parameters["timeOfDay"].SetValue(_tod);

            var viewFrustum = new BoundingFrustum(_camera.View*_camera.Projection);

            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in SolidBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in _world.Chunks.Values.Where(chunk => chunk != null))
                {
                    if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);

                    if (!chunk.BoundingBox.Intersects(viewFrustum) || chunk.IndexBuffer == null) continue;
                    if (chunk.IndexBuffer.IndexCount <= 0) continue;
                    _graphicsDevice.SetVertexBuffer(chunk.VertexBuffer);
                    _graphicsDevice.Indices = chunk.IndexBuffer;
                    //_graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0,
                    //    chunk.VertexBuffer.VertexCount, 0, chunk.IndexBuffer.IndexCount/3);
                    _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.VertexCount);
                }
            }
        }

        #endregion

        private Chunk DoLighting(Vector3I chunkIndex)
        {
            var chunk = _world.Chunks[chunkIndex.X, chunkIndex.Z];
            return DoLighting(chunk);
        }

        private Chunk DoLighting(Chunk chunk)
        {
            _lightingChunkProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private Chunk DoBuild(Vector3I chunkIndex)
        {
            var chunk = _world.Chunks[chunkIndex.X, chunkIndex.Z];
            return DoBuild(chunk);
        }

        private Chunk DoBuild(Chunk chunk)
        {
            _vertexBuildChunkProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private Chunk DoGenerate(Vector3I chunkIndex)
        {
            var chunk = new Chunk(_world, chunkIndex);
            return DoGenerate(chunk);
        }

        private Chunk DoGenerate(Chunk chunk)
        {
            _world.Chunks[chunk.Index.X, chunk.Index.Z] = chunk;
            _world.Generator.Generate(chunk);
            return chunk;
        }

        #region DrawWater

        private float _rippleTime;

        private void DrawWater(GameTime gameTime)
        {
            _rippleTime += 0.05f;

            _tod = _world.Tod;
            

            WaterBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            WaterBlockEffect.Parameters["View"].SetValue(_camera.View);
            WaterBlockEffect.Parameters["Projection"].SetValue(_camera.Projection);
            WaterBlockEffect.Parameters["CameraPosition"].SetValue(_camera.Position);
            WaterBlockEffect.Parameters["FogNear"].SetValue(_world.Fognear);
            WaterBlockEffect.Parameters["FogFar"].SetValue(_world.Fogfar);
            WaterBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);
            WaterBlockEffect.Parameters["SunColor"].SetValue(_world.Suncolor);

            WaterBlockEffect.Parameters["HorizonColor"].SetValue(_world.Horizoncolor);
            WaterBlockEffect.Parameters["NightColor"].SetValue(_world.Nightcolor);

            WaterBlockEffect.Parameters["MorningTint"].SetValue(_world.Morningtint);
            WaterBlockEffect.Parameters["EveningTint"].SetValue(_world.Eveningtint);

            WaterBlockEffect.Parameters["timeOfDay"].SetValue(_tod);
            WaterBlockEffect.Parameters["RippleTime"].SetValue(_rippleTime);

            var viewFrustum = new BoundingFrustum(_camera.View*_camera.Projection);

            _graphicsDevice.BlendState = BlendState.NonPremultiplied;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in WaterBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in from chunk in _world.Chunks.Values
                    where chunk != null
                    where chunk.BoundingBox.Intersects(viewFrustum) && chunk.waterVertexBuffer != null
                    where chunk.waterIndexBuffer.IndexCount > 0
                    select chunk)
                {
                    _graphicsDevice.SetVertexBuffer(chunk.waterVertexBuffer);
                    _graphicsDevice.Indices = chunk.waterIndexBuffer;
                    _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                        0, chunk.waterVertexCount);
                    //_graphicsDevice.BlendState = BlendState.Additive;
                }
            }
        }

        #endregion
    }
}