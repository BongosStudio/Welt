#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Core.Forge;

#endregion

namespace Welt.Forge.Renderers
{
    public class SkyRenderer : IRenderer
    {
        public SkyRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            m_GraphicsDevice = graphicsDevice;
            m_Camera = camera;
            m_World = world;
            m_SunEffect = new BasicEffect(graphicsDevice);
            m_SunSprite = new SpriteBatch(graphicsDevice);
        }

        public void Initialize()
        {
            
        }

        public void LoadContent(ContentManager content)
        {
            #region SkyDome and Clouds

            // SkyDome
            SkyDome = content.Load<Model>("Models\\dome");
            SkyDome.Meshes[0].MeshParts[0].Effect = content.Load<Effect>("Effects\\SkyDome");
            CloudMap = WeltGame.Instance.GraphicsManager.CloudTexture;
            StarMap = WeltGame.Instance.GraphicsManager.StarTexture;

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                m_GraphicsDevice.Viewport.AspectRatio, 0.3f, 1000.0f);

            // GPU Generated Clouds
            if (CloudsEnabled)
            {
                PerlinNoiseEffect = content.Load<Effect>("Effects\\PerlinNoise");
                var pp = m_GraphicsDevice.PresentationParameters;
                //the mipmap does not work on some pc ( i5 laptops at least), with mipmap false it s fine 
                CloudsRenderTarget = new RenderTarget2D(m_GraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, false,
                    SurfaceFormat.Color, DepthFormat.None);
                CloudStaticMap = CreateStaticMap(32);
                FullScreenVertices = SetUpFullscreenVertices();
            }

            #endregion
        }

        public void Stop()
        {
            //_running = false;
        }

        public event EventHandler LoadStepCompleted;

        #region Update

        public void Update(GameTime gameTime)
        {
            if (CloudsEnabled)
            {
                // Generate the clouds
                var time = (float) gameTime.TotalGameTime.TotalMilliseconds/100.0f;
                GeneratePerlinNoise(time);
            }
        }

        #endregion

        #region Draw

        private void DrawSun(GameTime gameTime)
        {
            m_SunEffect.World = Matrix.CreateScale(1, -1, 1) * Matrix.CreateTranslation(m_World.SunPos);
            m_SunEffect.View = FirstPersonCamera.Instance.View;
            m_SunEffect.Projection = FirstPersonCamera.Instance.Projection;

            m_SunSprite.Begin(0, null, null, DepthStencilState.DepthRead, RasterizerState.CullNone, m_SunEffect);
            m_SunSprite.Draw(WeltGame.Instance.GraphicsManager.SunTexture, Vector2.Zero, Color.White);
            m_SunSprite.End();
        }

        public void Draw(GameTime gameTime)
        {
            m_GraphicsDevice.RasterizerState = !m_World.Wireframed ? m_World.NormalRaster : m_World.WireframedRaster;

            var currentViewMatrix = m_Camera.View;

            _mTod = m_World.TimeOfDay;

            var modelTransforms = new Matrix[SkyDome.Bones.Count];
            SkyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            RotationStars += 0.0001f;
            RotationClouds = 0;

            // Stars
            var wStarMatrix = Matrix.CreateRotationY(RotationStars)*Matrix.CreateTranslation(0, -0.1f, 0)*
                              Matrix.CreateScale(110)*Matrix.CreateTranslation(m_Camera.Position);
            foreach (var mesh in SkyDome.Meshes)
            {
                foreach (var currentEffect in mesh.Effects)
                {
                    var worldMatrix = modelTransforms[mesh.ParentBone.Index]*wStarMatrix;

                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyStarDome"];

                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(currentViewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(ProjectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(StarMap);
                    currentEffect.Parameters["NightColor"].SetValue(NightColor);
                    currentEffect.Parameters["SunColor"].SetValue(OverheadSunColor);
                    currentEffect.Parameters["HorizonColor"].SetValue(HorizonColor);

                    currentEffect.Parameters["MorningTint"].SetValue(MorningTint);
                    currentEffect.Parameters["EveningTint"].SetValue(EveningTint);
                    currentEffect.Parameters["TimeOfDay"].SetValue(_mTod);
                }
                mesh.Draw();
            }

            // Clouds
            var wMatrix = Matrix.CreateRotationY(RotationClouds)*Matrix.CreateTranslation(0, -0.1f, 0)*
                          Matrix.CreateScale(100)*Matrix.CreateTranslation(m_Camera.Position);
            foreach (var mesh in SkyDome.Meshes)
            {
                foreach (var currentEffect in mesh.Effects)
                {
                    var worldMatrix = modelTransforms[mesh.ParentBone.Index]*wMatrix;

                    currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];

                    currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
                    currentEffect.Parameters["xView"].SetValue(currentViewMatrix);
                    currentEffect.Parameters["xProjection"].SetValue(ProjectionMatrix);
                    currentEffect.Parameters["xTexture"].SetValue(CloudMap);
                    currentEffect.Parameters["NightColor"].SetValue(NightColor);
                    currentEffect.Parameters["SunColor"].SetValue(OverheadSunColor);
                    currentEffect.Parameters["HorizonColor"].SetValue(HorizonColor);

                    currentEffect.Parameters["MorningTint"].SetValue(MorningTint);
                    currentEffect.Parameters["EveningTint"].SetValue(EveningTint);
                    currentEffect.Parameters["TimeOfDay"].SetValue(_mTod);
                }
                mesh.Draw();
            }
            DrawSun(gameTime);
        }

        #endregion

        #region Fields

        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly FirstPersonCamera m_Camera;
        private readonly World m_World;

        private readonly BasicEffect m_SunEffect;
        private readonly SpriteBatch m_SunSprite;

        #region Atmospheric settings

        //TODO accord with ThreadedWorldRenderer fog constants

        public const float Farplane = 220*4;
        public const int Fognear = 200*4;
        public const int Fogfar = 220*4;

        protected Vector3 Suncolor = Color.White.ToVector3();

        protected Vector4 OverheadSunColor = Color.DarkBlue.ToVector4();
        protected Vector4 NightColor = Color.DarkBlue.ToVector4();

        protected Vector4 FogColor = Color.White.ToVector4();
        protected Vector4 HorizonColor = Color.White.ToVector4();

        protected Vector4 EveningTint = Color.Red.ToVector4();
        protected Vector4 MorningTint = Color.Gold.ToVector4();

        public float CloudOvercast = 1f;

        public const bool CloudsEnabled = true;

        #region SkyDome and Clouds

        // SkyDome
        protected Model SkyDome;
        protected Matrix ProjectionMatrix;
        public Texture2D CloudMap;
        // TODO: replace StarMap with particle generator of stars
        protected Texture2D StarMap;
        protected float RotationClouds;
        protected float RotationStars;

        // GPU generated clouds
        protected Texture2D CloudStaticMap;
        protected RenderTarget2D CloudsRenderTarget;
        protected Effect PerlinNoiseEffect;
        protected VertexPositionTexture[] FullScreenVertices;

        private float _mTod;

        #endregion

        #endregion

        #endregion

        #region Generate Clouds

        public virtual Texture2D CreateStaticMap(int resolution)
        {
            var rand = new Random();
            var noisyColors = new Color[resolution*resolution];
            for (var x = 0; x < resolution; x++)
                for (var y = 0; y < resolution; y++)
                    noisyColors[x + y*resolution] = new Color(new Vector3(rand.Next(1000)/1000.0f, 0, 0));

            var noiseImage = new Texture2D(m_GraphicsDevice, resolution, resolution, true, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        public virtual VertexPositionTexture[] SetUpFullscreenVertices()
        {
            return new[]
            {

                new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1)),
                new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0))
            };
        }

        public virtual void GeneratePerlinNoise(float time)
        {
            m_GraphicsDevice.SetRenderTarget(CloudsRenderTarget);
            //_graphicsDevice.Clear(Color.White);

            PerlinNoiseEffect.CurrentTechnique = PerlinNoiseEffect.Techniques["PerlinNoise"];
            PerlinNoiseEffect.Parameters["xTexture"].SetValue(CloudStaticMap);
            PerlinNoiseEffect.Parameters["xOvercast"].SetValue(CloudOvercast);
            PerlinNoiseEffect.Parameters["xTime"].SetValue(time/1000.0f);

            foreach (var pass in PerlinNoiseEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                m_GraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, FullScreenVertices, 0, 2);
            }

            m_GraphicsDevice.SetRenderTarget(null);
            CloudMap = CloudsRenderTarget;

        }

        #endregion
    }
}