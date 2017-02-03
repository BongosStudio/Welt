#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.API;
using Welt.Processors;
using System.Collections.Generic;
using Welt.Extensions;
using System.Threading;
using System.Threading.Tasks;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Core;

namespace Welt.Forge.Renderers
{
    internal class WorldRenderer : IRenderer
    {
        protected Effect BlockEffect;
        protected Texture2D TextureAtlas;

        private FirstPersonCamera m_Camera;
        private GraphicsDevice m_GraphicsDevice;
        private VertexBuildChunkProcessor m_Vertex;

        private bool m_IsRunning;

        public WorldRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera)
        {
            m_GraphicsDevice = graphicsDevice;
            m_Camera = camera;
            m_IsRunning = true;
            m_Vertex = new VertexBuildChunkProcessor(m_GraphicsDevice);
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = WeltGame.Instance.GraphicsManager.BlockTexture;
            BlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
        }

        public void Update(GameTime gameTime)
        {

        }

        public void Draw(GameTime gameTime)
        {
            DrawChunks(gameTime);
        }

        public void Stop()
        {
            m_IsRunning = false;
        }

        #region DrawSolid

        private float m_RippleTime;
        private TimeSpan m_PreviousFullRebuild = TimeSpan.Zero;

        public event EventHandler LoadStepCompleted;

        private void DrawChunks(GameTime gameTime)
        {
            var tod = m_World.World.TimeOfDay;
            m_RippleTime += 0.1f;
            if (m_RippleTime == 1.0f) m_RippleTime = 0;
            
            BlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            BlockEffect.Parameters["View"].SetValue(m_Camera.View);
            BlockEffect.Parameters["Projection"].SetValue(m_Camera.Projection);
            BlockEffect.Parameters["ReflectionMatrix"].SetValue(m_Camera.ReflectionViewMatrix);
            BlockEffect.Parameters["CameraPosition"].SetValue(m_Camera.Position);
            BlockEffect.Parameters["FogNear"].SetValue(m_World.Fognear);
            BlockEffect.Parameters["FogFar"].SetValue(m_World.Fogfar);
            BlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);
            //BlockEffect.Parameters["ReflectionTexture"].SetValue(m_GraphicsDevice.GetRenderTargets()[0].RenderTarget);

            BlockEffect.Parameters["HorizonColor"].SetValue(m_World.HorizonColor);
            BlockEffect.Parameters["NightColor"].SetValue(m_World.NightColor);

            BlockEffect.Parameters["MorningTint"].SetValue(m_World.Morningtint);
            BlockEffect.Parameters["EveningTint"].SetValue(m_World.Eveningtint);

            BlockEffect.Parameters["DeepWaterColor"].SetValue(m_World.DeepWaterColor);
            BlockEffect.Parameters["HeldItemLight"].SetValue(BlockLogic.GetLightLevel(Player.Current.HeldItem).ToVector3());
            BlockEffect.Parameters["HasFlicker"].SetValue(Player.Current.HeldItem.Block.Id == BlockType.TORCH);
            BlockEffect.Parameters["SunColor"].SetValue(m_World.SunColor);
            BlockEffect.Parameters["TimeOfDay"].SetValue(tod);
            BlockEffect.Parameters["SkyLightPosition"].SetValue(m_World.SunPos);
            BlockEffect.Parameters["RippleTime"].SetValue(m_RippleTime);
            BlockEffect.Parameters["Random"].SetValue((float)FastMath.NextRandomDouble());
            BlockEffect.Parameters["IsUnderWater"].SetValue(m_World.GetBlock(Player.Current.Position).Id == BlockType.WATER);

            var viewFrustum = new BoundingFrustum(m_Camera.View*m_Camera.Projection);
            m_GraphicsDevice.BlendState = BlendState.AlphaBlend;
            m_GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            var chunks = m_World.GetChunks();
            foreach (var pass in BlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                
                for (var i = 0; i < chunks.Length; ++i)
                {
                    var chunk = chunks[i];
                    if (chunk == null) continue;
                    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                    if (chunk.PrimaryVertexCount == 0 || chunk.PrimaryVertexBuffer == null || chunk.PrimaryIndexBuffer == null)
                        continue;
                    m_GraphicsDevice.SetVertexBuffer(chunk.PrimaryVertexBuffer);
                    m_GraphicsDevice.Indices = chunk.PrimaryIndexBuffer;
                    m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.PrimaryVertexCount);
                }

                for (var i = 0; i < Chunks.Count; ++i)
                {
                    var chunk = Chunks[i];
                    if (chunk == null) continue;
                    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                    if (chunk.SecondaryVertexCount == 0 || chunk.SecondaryVertexBuffer == null || chunk.SecondaryIndexBuffer == null)
                        continue;
                    m_GraphicsDevice.SetVertexBuffer(chunk.SecondaryVertexBuffer);
                    m_GraphicsDevice.Indices = chunk.SecondaryIndexBuffer;
                    m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.SecondaryVertexCount);
                }
            }
        }

        #endregion

        private void Process()
        {
            
        }
    }
}