#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;

#endregion

namespace Welt.Forge.Renderers
{
    public class SkyDomeRenderer : IRenderer
    {
        public SkyDomeRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            _mGraphicsDevice = graphicsDevice;
            _mCamera = camera;
            _mWorld = world;
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
            CloudMap = content.Load<Texture2D>("Textures\\cloudMap");
            StarMap = content.Load<Texture2D>("Textures\\newStars2");

            ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                _mGraphicsDevice.Viewport.AspectRatio, 0.3f, 1000.0f);

            // GPU Generated Clouds
            if (CloudsEnabled)
            {
                PerlinNoiseEffect = content.Load<Effect>("Effects\\PerlinNoise");
                var pp = _mGraphicsDevice.PresentationParameters;
                //the mipmap does not work on some pc ( i5 laptops at least), with mipmap false it s fine 
                CloudsRenderTarget = new RenderTarget2D(_mGraphicsDevice, pp.BackBufferWidth, pp.BackBufferHeight, true,
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

        #region Update

        public void Update(GameTime gameTime)
        {
            if (CloudsEnabled)
            {
                // Generate the clouds
                var time = (float) gameTime.TotalGameTime.TotalMilliseconds/100.0f;
                //CloudOvercast += 0.001f;
                GeneratePerlinNoise(time);
            }
            //update of chunks is handled in chunkReGenBuildTask for this class
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            _mGraphicsDevice.RasterizerState = !_mWorld.Wireframed ? _mWorld.NormalRaster : _mWorld.WireframedRaster;

            var currentViewMatrix = _mCamera.View;

            _mTod = _mWorld.Tod;

            var modelTransforms = new Matrix[SkyDome.Bones.Count];
            SkyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            RotationStars += 0.0001f;
            RotationClouds = 0;

            // Stars
            var wStarMatrix = Matrix.CreateRotationY(RotationStars)*Matrix.CreateTranslation(0, -0.1f, 0)*
                              Matrix.CreateScale(110)*Matrix.CreateTranslation(_mCamera.Position);
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
                    currentEffect.Parameters["timeOfDay"].SetValue(_mTod);
                }
                mesh.Draw();
            }

            // Clouds
            var wMatrix = Matrix.CreateRotationY(RotationClouds)*Matrix.CreateTranslation(0, -0.1f, 0)*
                          Matrix.CreateScale(100)*Matrix.CreateTranslation(_mCamera.Position);
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
                    currentEffect.Parameters["timeOfDay"].SetValue(_mTod);
                }
                mesh.Draw();
            }
        }

        #endregion

        #region Fields

        private readonly GraphicsDevice _mGraphicsDevice;
        private readonly FirstPersonCamera _mCamera;
        private readonly World _mWorld;

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

            var noiseImage = new Texture2D(_mGraphicsDevice, resolution, resolution, true, SurfaceFormat.Color);
            noiseImage.SetData(noisyColors);
            return noiseImage;
        }

        public virtual VertexPositionTexture[] SetUpFullscreenVertices()
        {
            var vertices = new VertexPositionTexture[4];

            vertices[0] = new VertexPositionTexture(new Vector3(-1, 1, 0f), new Vector2(0, 1));
            vertices[1] = new VertexPositionTexture(new Vector3(1, 1, 0f), new Vector2(1, 1));
            vertices[2] = new VertexPositionTexture(new Vector3(-1, -1, 0f), new Vector2(0, 0));
            vertices[3] = new VertexPositionTexture(new Vector3(1, -1, 0f), new Vector2(1, 0));

            return vertices;
        }

        public virtual void GeneratePerlinNoise(float time)
        {
            _mGraphicsDevice.SetRenderTarget(CloudsRenderTarget);
            //_graphicsDevice.Clear(Color.White);

            PerlinNoiseEffect.CurrentTechnique = PerlinNoiseEffect.Techniques["PerlinNoise"];
            PerlinNoiseEffect.Parameters["xTexture"].SetValue(CloudStaticMap);
            PerlinNoiseEffect.Parameters["xOvercast"].SetValue(CloudOvercast);
            PerlinNoiseEffect.Parameters["xTime"].SetValue(time/1000.0f);

            foreach (var pass in PerlinNoiseEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _mGraphicsDevice.DrawUserPrimitives(PrimitiveType.TriangleStrip, FullScreenVertices, 0, 2);
            }

            _mGraphicsDevice.SetRenderTarget(null);
            CloudMap = CloudsRenderTarget;

        }

        #endregion
    }
}