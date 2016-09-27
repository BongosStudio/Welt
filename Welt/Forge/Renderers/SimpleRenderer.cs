#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.API.Forge;
using Welt.Cameras;
using Welt.Core.Forge;
using Welt.Forge;
using Welt.Forge.Builders;
using Welt.Forge.Renderers;
using Welt.IO;
using Welt.Models;
using Welt.Processors;
using Welt.Types;

namespace Welt.Forge.Renderers
{
    internal class SimpleRenderer : IRenderer
    {
        private const byte BUILD_RANGE = 12;
        private const byte LIGHT_RANGE = BUILD_RANGE + 1;
        private const byte GENERATE_RANGE_LOW = LIGHT_RANGE + 1;
        private const byte GENERATE_RANGE_HIGH = GENERATE_RANGE_LOW;
        private readonly FirstPersonCamera _mCamera;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly WorldBuilder _world;
        private readonly VertexBuildChunkProcessor _mVertexProcessor;
        protected Effect SolidBlockEffect;
        protected Texture2D TextureAtlas;
        private float _mTod;
        private bool _mIsRunning;
        protected Effect WaterBlockEffect;
        protected Effect GrassBlockEffect;

        public SimpleRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, WorldBuilder world)
        {
            _graphicsDevice = graphicsDevice;
            _mCamera = camera;
            _world = world;
            _world.Renderer = this;
            _mIsRunning = true;
        }

        public void Initialize()
        {
            
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
            if (!_mIsRunning) return;
        }

        public void Draw(GameTime gameTime)
        {
            if (!_mIsRunning) return;
            DrawSolid(gameTime);
            DrawGrass(gameTime);
            DrawWater(gameTime);
        }

        public void Stop()
        {
            _mIsRunning = false;
        }

        public event EventHandler LoadStepCompleted;
        
        public void DoLightFor(ChunkBuilder chunk)
        {
            _world.ProcessChunk(chunk);
        }

        #region DrawSolid

        private void DrawSolid(GameTime gameTime)
        {
            _mTod = _world.TimeOfDay;

            SolidBlockEffect.Parameters["WorldObject"].SetValue(Matrix.Identity);
            SolidBlockEffect.Parameters["View"].SetValue(_mCamera.View);
            SolidBlockEffect.Parameters["Projection"].SetValue(_mCamera.Projection);
            SolidBlockEffect.Parameters["CameraPosition"].SetValue(_mCamera.Position);
            SolidBlockEffect.Parameters["FogNear"].SetValue(_world.Fognear);
            SolidBlockEffect.Parameters["FogFar"].SetValue(_world.Fogfar);
            SolidBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);

            SolidBlockEffect.Parameters["HorizonColor"].SetValue(_world.Horizoncolor);
            SolidBlockEffect.Parameters["NightColor"].SetValue(_world.Nightcolor);

            SolidBlockEffect.Parameters["MorningTint"].SetValue(_world.Morningtint);
            SolidBlockEffect.Parameters["EveningTint"].SetValue(_world.Eveningtint);

            SolidBlockEffect.Parameters["SunColor"].SetValue(_world.Suncolor);
            SolidBlockEffect.Parameters["timeOfDay"].SetValue(_mTod);

            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in SolidBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in _world.Where(c => !c.IsGenerated))
                {
                    if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);
                    if (!chunk.BoundingBox.Intersects(viewFrustum) || chunk.IndexBuffer == null) continue;
                    if (chunk.IndexBuffer.IndexCount <= 0) continue;
                    _graphicsDevice.SetVertexBuffer(chunk.VertexBuffer);
                    _graphicsDevice.Indices = chunk.IndexBuffer;
                    _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.VertexCount);
                }
            }
        }

        #endregion

        private ChunkBuilder DoLighting(Vector3I chunkIndex)
        {
            var chunk = (ChunkBuilder) _world.World.GetChunk(chunkIndex.X, chunkIndex.Z);
            
            return DoLighting(chunk);
        }

        private ChunkBuilder DoLighting(ChunkBuilder chunk)
        {
            _lightingChunkProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private ChunkBuilder DoBuild(Vector3I chunkIndex)
        {
            var chunk = (Chunk) _world.World.GetChunk(chunkIndex.X, chunkIndex.Z);
            return DoBuild(chunk);
        }

        private ChunkBuilder DoBuild(ChunkBuilder chunk)
        {
            _mVertexProcessor.ProcessChunk(chunk);
            return chunk;
        }

        private ChunkBuilder DoGenerate(Vector3I chunkIndex)
        {

        }

        private ChunkBuilder DoGenerate(uint x, uint z)
        {
            var chunk = new ChunkBuilder(_world, x, z);  
            _world.Generator.GenerateChunk(_world, chunk);
            _world.SetChunk(x, z, chunk);
            return chunk;
        }

        #region DrawGrass

        private float _mWaveTime;

        private void DrawGrass(GameTime time)
        {
            _mWaveTime += 0.05f;
            GrassBlockEffect.Parameters["WorldObject"].SetValue(Matrix.Identity);
            GrassBlockEffect.Parameters["View"].SetValue(_mCamera.View);
            GrassBlockEffect.Parameters["Projection"].SetValue(_mCamera.Projection);
            GrassBlockEffect.Parameters["CameraPosition"].SetValue(_mCamera.Position);
            GrassBlockEffect.Parameters["Texture0"].SetValue(TextureAtlas);
            GrassBlockEffect.Parameters["WaveTime"].SetValue(_mWaveTime);
            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

            _graphicsDevice.BlendState = BlendState.Opaque;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in GrassBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in from chunk in _world.Chunks
                                      where chunk != null
                                      where chunk.BoundingBox.Intersects(viewFrustum)
                                      //where ChunkObject.GrassVertexBuffer != null
                                      //where ChunkObject.GrassIndexBuffer.IndexCount > 0
                                      select chunk)
                {
                    //_graphicsDevice.SetVertexBuffer(ChunkObject.GrassVertexBuffer);
                    //_graphicsDevice.Indices = ChunkObject.GrassIndexBuffer;
                    //_graphicsDevice.DrawIndexPrimitives(PrimitiveType.TriangleList, 0, 0, ChunkObject.GrassVertexCount);
                }
            }
        }

        #endregion

        #region DrawWater

        private float _mRippleTime;

        private void DrawWater(GameTime gameTime)
        {
            _mRippleTime += 0.05f;

            _mTod = _world.Tod;
            
            WaterBlockEffect.Parameters["WorldObject"].SetValue(Matrix.Identity);
            WaterBlockEffect.Parameters["View"].SetValue(_mCamera.View);
            WaterBlockEffect.Parameters["Projection"].SetValue(_mCamera.Projection);
            WaterBlockEffect.Parameters["CameraPosition"].SetValue(_mCamera.Position);
            WaterBlockEffect.Parameters["FogNear"].SetValue(_world.Fognear);
            WaterBlockEffect.Parameters["FogFar"].SetValue(_world.Fogfar);
            WaterBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);
            WaterBlockEffect.Parameters["SunColor"].SetValue(_world.Suncolor);

            WaterBlockEffect.Parameters["HorizonColor"].SetValue(_world.Horizoncolor);
            WaterBlockEffect.Parameters["NightColor"].SetValue(_world.Nightcolor);

            WaterBlockEffect.Parameters["MorningTint"].SetValue(_world.Morningtint);
            WaterBlockEffect.Parameters["EveningTint"].SetValue(_world.Eveningtint);

            WaterBlockEffect.Parameters["timeOfDay"].SetValue(_mTod);
            WaterBlockEffect.Parameters["RippleTime"].SetValue(_mRippleTime);

            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

            _graphicsDevice.BlendState = BlendState.NonPremultiplied;
            _graphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in WaterBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                foreach (var chunk in from chunk in _world.Chunks
                    where chunk != null
                    where chunk.BoundingBox.Intersects(viewFrustum) && chunk.WaterVertexBuffer != null
                    where chunk.WaterIndexBuffer.IndexCount > 0
                    select chunk)
                {
                    _graphicsDevice.SetVertexBuffer(chunk.WaterVertexBuffer);
                    _graphicsDevice.Indices = chunk.WaterIndexBuffer;
                    _graphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0,
                        0, chunk.WaterVertexCount);
                }
            }
        }

        #endregion
    }
}