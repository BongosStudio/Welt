#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Models;
using Welt.Profiling;

#endregion

namespace Welt.Forge.Renderers
{
    public class DiagnosticWorldRenderer : IRenderer
    {
        public DiagnosticWorldRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            m_graphicsDevice = graphicsDevice;
            m_camera = camera;
            m_world = world;
        }

        public void Initialize()
        {
            m_effect = new BasicEffect(m_graphicsDevice);

            #region debugFont Rectangle

            m_debugRectTexture = new Texture2D(m_graphicsDevice, 1, 1);
            var texcol = new Color[1];
            m_debugRectTexture.GetData(texcol);
            texcol[0] = Color.Black;
            m_debugRectTexture.SetData(texcol);

            m_backgroundRectangle = new Rectangle(680, 0, 120, 144);

            m_chunksVector2 = new Vector2(680, 0);
            m_awaitingGenerateVector2 = new Vector2(680, 16);
            m_generatingVector2 = new Vector2(680, 32);
            m_awaitingLightingVector2 = new Vector2(680, 48);
            m_lightingVector2 = new Vector2(680, 64);
            m_awaitingBuildVector2 = new Vector2(680, 80);
            m_awaitingRebuildVector2 = new Vector2(680, 96);
            m_awaitingRelightingVector2 = new Vector2(680, 112);
            m_readyVector2 = new Vector2(680, 128);

            #endregion
        }

        public void LoadContent(ContentManager content)
        {
            m_debugSpriteBatch = new SpriteBatch(m_graphicsDevice);
            m_debugFont = content.Load<SpriteFont>("Fonts\\OSDdisplay");
        }

        public void Update(GameTime gameTime)
        {
        }

        #region Draw

        public void Draw(GameTime gameTime)
        {
            var viewFrustum = new BoundingFrustum(m_camera.View*m_camera.Projection);

            var totalChunksCounter = 0;
            var awaitingGenerateCounter = 0;
            var generatingCounter = 0;
            var awaitingLightingCounter = 0;
            var lightingCounter = 0;
            var awaitingBuildCounter = 0;
            var awaitingRebuildCounter = 0;
            var buildingCounter = 0;
            var awaitingRelightingCounter = 0;
            var readyCounter = 0;

            foreach (var chunk in m_world.Chunks.Values)
            {
                //if (chunk.BoundingBox.Intersects(viewFrustum))
                //{
                switch (chunk.State)
                {
                    case ChunkState.AwaitingGenerate:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Red);
                        awaitingGenerateCounter++;
                        break;
                    case ChunkState.Generating:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Pink);
                        generatingCounter++;
                        break;
                    case ChunkState.AwaitingLighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Orange);
                        awaitingLightingCounter++;
                        break;
                    case ChunkState.Lighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Yellow);
                        lightingCounter++;
                        break;
                    case ChunkState.AwaitingBuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Green);
                        awaitingBuildCounter++;
                        break;
                    case ChunkState.AwaitingRebuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Green);
                        awaitingRebuildCounter++;
                        break;
                    case ChunkState.Building:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.LightGreen);
                        buildingCounter++;
                        break;
                    case ChunkState.AwaitingRelighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Black);
                        awaitingRelightingCounter++;
                        break;
                    case ChunkState.Ready:
                        readyCounter++;
                        break;
                    default:
                        Debug.WriteLine("Unchecked State: {0}", chunk.State);
                        Utility.DrawBoundingBox(chunk.BoundingBox, m_graphicsDevice, m_effect, Matrix.Identity,
                            m_camera.View, m_camera.Projection, Color.Blue);
                        break;
                }
                //}
                totalChunksCounter++;
            }

            #region OSD debug texts

            m_debugSpriteBatch.Begin();
            if (m_debugRectangle)
            {
                m_debugSpriteBatch.Draw(m_debugRectTexture, m_backgroundRectangle, Color.Black);
            }
            m_debugSpriteBatch.DrawString(m_debugFont, "Chunks: " + totalChunksCounter, m_chunksVector2, Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "A.Generate: " + awaitingGenerateCounter, m_awaitingGenerateVector2,
                Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "Generating: " + generatingCounter, m_generatingVector2, Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "A.Lighting: " + awaitingLightingCounter, m_awaitingLightingVector2,
                Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "Lighting: " + lightingCounter, m_lightingVector2, Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "A.Build: " + awaitingBuildCounter, m_awaitingBuildVector2, Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "A.Rebuild: " + awaitingRebuildCounter, m_awaitingRebuildVector2,
                Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "A.Relighting: " + awaitingRelightingCounter,
                m_awaitingRelightingVector2, Color.White);
            m_debugSpriteBatch.DrawString(m_debugFont, "Ready: " + readyCounter, m_readyVector2, Color.White);
            m_debugSpriteBatch.End();

            #endregion
        }

        #endregion

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #region Fields

        private BasicEffect m_effect;
        private readonly GraphicsDevice m_graphicsDevice;
        private readonly FirstPersonCamera m_camera;
        private readonly World m_world;

        #region debugFont

        private SpriteBatch m_debugSpriteBatch;
        private SpriteFont m_debugFont;
        private Texture2D m_debugRectTexture;
        private readonly bool m_debugRectangle = true;
        private Rectangle m_backgroundRectangle;

        private Vector2 m_chunksVector2;
        private Vector2 m_awaitingGenerateVector2;
        private Vector2 m_generatingVector2;
        private Vector2 m_awaitingLightingVector2;
        private Vector2 m_lightingVector2;
        private Vector2 m_awaitingBuildVector2;
        private Vector2 m_awaitingRebuildVector2;
        private Vector2 m_awaitingRelightingVector2;
        private Vector2 m_readyVector2;

        #endregion

        #endregion
    }
}