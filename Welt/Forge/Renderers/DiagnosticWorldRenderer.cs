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
            _mGraphicsDevice = graphicsDevice;
            _mCamera = camera;
            _mWorld = world;
        }

        public void Initialize()
        {
            _mEffect = new BasicEffect(_mGraphicsDevice);

            #region debugFont Rectangle

            _mDebugRectTexture = new Texture2D(_mGraphicsDevice, 1, 1);
            var texcol = new Color[1];
            _mDebugRectTexture.GetData(texcol);
            texcol[0] = Color.Black;
            _mDebugRectTexture.SetData(texcol);

            _mBackgroundRectangle = new Rectangle(680, 0, 120, 144);

            _mChunksVector2 = new Vector2(680, 0);
            _mAwaitingGenerateVector2 = new Vector2(680, 16);
            _mGeneratingVector2 = new Vector2(680, 32);
            _mAwaitingLightingVector2 = new Vector2(680, 48);
            _mLightingVector2 = new Vector2(680, 64);
            _mAwaitingBuildVector2 = new Vector2(680, 80);
            _mAwaitingRebuildVector2 = new Vector2(680, 96);
            _mAwaitingRelightingVector2 = new Vector2(680, 112);
            _mReadyVector2 = new Vector2(680, 128);

            #endregion
        }

        public void LoadContent(ContentManager content)
        {
            _mDebugSpriteBatch = new SpriteBatch(_mGraphicsDevice);
            _mDebugFont = content.Load<SpriteFont>("Fonts\\OSDdisplay");
        }

        public void Update(GameTime gameTime)
        {
        }

        #region Draw

        public void Draw(GameTime gameTime)
        {
            var viewFrustum = new BoundingFrustum(_mCamera.View*_mCamera.Projection);

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

            foreach (var chunk in _mWorld.Chunks.Values)
            {
                //if (chunk.BoundingBox.Intersects(viewFrustum))
                //{
                switch (chunk.State)
                {
                    case ChunkState.AwaitingGenerate:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Red);
                        awaitingGenerateCounter++;
                        break;
                    case ChunkState.Generating:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Pink);
                        generatingCounter++;
                        break;
                    case ChunkState.AwaitingLighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Orange);
                        awaitingLightingCounter++;
                        break;
                    case ChunkState.Lighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Yellow);
                        lightingCounter++;
                        break;
                    case ChunkState.AwaitingBuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Green);
                        awaitingBuildCounter++;
                        break;
                    case ChunkState.AwaitingRebuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Green);
                        awaitingRebuildCounter++;
                        break;
                    case ChunkState.Building:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.LightGreen);
                        buildingCounter++;
                        break;
                    case ChunkState.AwaitingRelighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Black);
                        awaitingRelightingCounter++;
                        break;
                    case ChunkState.Ready:
                        readyCounter++;
                        break;
                    default:
                        Debug.WriteLine("Unchecked State: {0}", chunk.State);
                        Utility.DrawBoundingBox(chunk.BoundingBox, _mGraphicsDevice, _mEffect, Matrix.Identity,
                            _mCamera.View, _mCamera.Projection, Color.Blue);
                        break;
                }
                //}
                totalChunksCounter++;
            }

            #region OSD debug texts

            _mDebugSpriteBatch.Begin();
            if (_mDebugRectangle)
            {
                _mDebugSpriteBatch.Draw(_mDebugRectTexture, _mBackgroundRectangle, Color.Black);
            }
            _mDebugSpriteBatch.DrawString(_mDebugFont, "Chunks: " + totalChunksCounter, _mChunksVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "A.Generate: " + awaitingGenerateCounter, _mAwaitingGenerateVector2,
                Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "Generating: " + generatingCounter, _mGeneratingVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "A.Lighting: " + awaitingLightingCounter, _mAwaitingLightingVector2,
                Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "Lighting: " + lightingCounter, _mLightingVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "A.Build: " + awaitingBuildCounter, _mAwaitingBuildVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "A.Rebuild: " + awaitingRebuildCounter, _mAwaitingRebuildVector2,
                Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "A.Relighting: " + awaitingRelightingCounter,
                _mAwaitingRelightingVector2, Color.White);
            _mDebugSpriteBatch.DrawString(_mDebugFont, "Ready: " + readyCounter, _mReadyVector2, Color.White);
            _mDebugSpriteBatch.End();

            #endregion
        }

        #endregion

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #region Fields

        private BasicEffect _mEffect;
        private readonly GraphicsDevice _mGraphicsDevice;
        private readonly FirstPersonCamera _mCamera;
        private readonly World _mWorld;

        #region debugFont

        private SpriteBatch _mDebugSpriteBatch;
        private SpriteFont _mDebugFont;
        private Texture2D _mDebugRectTexture;
        private readonly bool _mDebugRectangle = true;
        private Rectangle _mBackgroundRectangle;

        private Vector2 _mChunksVector2;
        private Vector2 _mAwaitingGenerateVector2;
        private Vector2 _mGeneratingVector2;
        private Vector2 _mAwaitingLightingVector2;
        private Vector2 _mLightingVector2;
        private Vector2 _mAwaitingBuildVector2;
        private Vector2 _mAwaitingRebuildVector2;
        private Vector2 _mAwaitingRelightingVector2;
        private Vector2 _mReadyVector2;

        #endregion

        #endregion
    }
}