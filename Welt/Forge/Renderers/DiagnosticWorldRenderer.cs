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
            _graphicsDevice = graphicsDevice;
            _camera = camera;
            _world = world;
        }

        public void Initialize()
        {
            _effect = new BasicEffect(_graphicsDevice);

            #region debugFont Rectangle

            debugRectTexture = new Texture2D(_graphicsDevice, 1, 1);
            var texcol = new Color[1];
            debugRectTexture.GetData(texcol);
            texcol[0] = Color.Black;
            debugRectTexture.SetData(texcol);

            backgroundRectangle = new Rectangle(680, 0, 120, 144);

            chunksVector2 = new Vector2(680, 0);
            awaitingGenerateVector2 = new Vector2(680, 16);
            generatingVector2 = new Vector2(680, 32);
            awaitingLightingVector2 = new Vector2(680, 48);
            lightingVector2 = new Vector2(680, 64);
            awaitingBuildVector2 = new Vector2(680, 80);
            awaitingRebuildVector2 = new Vector2(680, 96);
            awaitingRelightingVector2 = new Vector2(680, 112);
            readyVector2 = new Vector2(680, 128);

            #endregion
        }

        public void LoadContent(ContentManager content)
        {
            debugSpriteBatch = new SpriteBatch(_graphicsDevice);
            debugFont = content.Load<SpriteFont>("Fonts\\OSDdisplay");
        }

        public void Update(GameTime gameTime)
        {
        }

        #region Draw

        public void Draw(GameTime gameTime)
        {
            var viewFrustum = new BoundingFrustum(_camera.View*_camera.Projection);

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

            foreach (var chunk in _world.Chunks.Values)
            {
                //if (chunk.BoundingBox.Intersects(viewFrustum))
                //{
                switch (chunk.State)
                {
                    case ChunkState.AwaitingGenerate:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Red);
                        awaitingGenerateCounter++;
                        break;
                    case ChunkState.Generating:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Pink);
                        generatingCounter++;
                        break;
                    case ChunkState.AwaitingLighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Orange);
                        awaitingLightingCounter++;
                        break;
                    case ChunkState.Lighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Yellow);
                        lightingCounter++;
                        break;
                    case ChunkState.AwaitingBuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Green);
                        awaitingBuildCounter++;
                        break;
                    case ChunkState.AwaitingRebuild:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Green);
                        awaitingRebuildCounter++;
                        break;
                    case ChunkState.Building:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.LightGreen);
                        buildingCounter++;
                        break;
                    case ChunkState.AwaitingRelighting:
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Black);
                        awaitingRelightingCounter++;
                        break;
                    case ChunkState.Ready:
                        readyCounter++;
                        break;
                    default:
                        Debug.WriteLine("Unchecked State: {0}", chunk.State);
                        Utility.DrawBoundingBox(chunk.BoundingBox, _graphicsDevice, _effect, Matrix.Identity,
                            _camera.View, _camera.Projection, Color.Blue);
                        break;
                }
                //}
                totalChunksCounter++;
            }

            #region OSD debug texts

            debugSpriteBatch.Begin();
            if (debugRectangle)
            {
                debugSpriteBatch.Draw(debugRectTexture, backgroundRectangle, Color.Black);
            }
            debugSpriteBatch.DrawString(debugFont, "Chunks: " + totalChunksCounter, chunksVector2, Color.White);
            debugSpriteBatch.DrawString(debugFont, "A.Generate: " + awaitingGenerateCounter, awaitingGenerateVector2,
                Color.White);
            debugSpriteBatch.DrawString(debugFont, "Generating: " + generatingCounter, generatingVector2, Color.White);
            debugSpriteBatch.DrawString(debugFont, "A.Lighting: " + awaitingLightingCounter, awaitingLightingVector2,
                Color.White);
            debugSpriteBatch.DrawString(debugFont, "Lighting: " + lightingCounter, lightingVector2, Color.White);
            debugSpriteBatch.DrawString(debugFont, "A.Build: " + awaitingBuildCounter, awaitingBuildVector2, Color.White);
            debugSpriteBatch.DrawString(debugFont, "A.Rebuild: " + awaitingRebuildCounter, awaitingRebuildVector2,
                Color.White);
            debugSpriteBatch.DrawString(debugFont, "A.Relighting: " + awaitingRelightingCounter,
                awaitingRelightingVector2, Color.White);
            debugSpriteBatch.DrawString(debugFont, "Ready: " + readyCounter, readyVector2, Color.White);
            debugSpriteBatch.End();

            #endregion
        }

        #endregion

        public void Stop()
        {
            throw new NotImplementedException();
        }

        #region Fields

        private BasicEffect _effect;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly FirstPersonCamera _camera;
        private readonly World _world;

        #region debugFont

        private SpriteBatch debugSpriteBatch;
        private SpriteFont debugFont;
        private Texture2D debugRectTexture;
        private readonly bool debugRectangle = true;
        private Rectangle backgroundRectangle;

        private Vector2 chunksVector2;
        private Vector2 awaitingGenerateVector2;
        private Vector2 generatingVector2;
        private Vector2 awaitingLightingVector2;
        private Vector2 lightingVector2;
        private Vector2 awaitingBuildVector2;
        private Vector2 awaitingRebuildVector2;
        private Vector2 awaitingRelightingVector2;
        private Vector2 readyVector2;

        #endregion

        #endregion
    }
}