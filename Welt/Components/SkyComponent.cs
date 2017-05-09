using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Forge;

namespace Welt.Components
{
    public class SkyComponent : IVisualComponent
    {
        public WeltGame Game { get; set; }
        public GraphicsDevice Graphics { get; set; }
        private BasicEffect SkyPlaneEffect { get; set; }
        private BasicEffect CelestialPlaneEffect { get; set; }
        private VertexBuffer SkyPlane { get; set; }
        private VertexBuffer CelestialPlane { get; set; }
        private PlayerRenderer Player { get; set; }
        private Texture2D CloudMap { get; set; }
        // GPU generated clouds

        public float CloudOvercast = 1f;

        protected Model SkyDome;
        protected Texture2D CloudStaticMap;
        protected RenderTarget2D CloudsRenderTarget;
        protected Effect PerlinNoise;
        protected VertexPositionTexture[] FullScreenVertices;

        public SkyComponent(WeltGame game, PlayerRenderer player)
        {
            Game = game;
            Player = player;
            Graphics = Game.GraphicsDevice;
            CelestialPlaneEffect = new BasicEffect(Game.GraphicsDevice)
            {
                TextureEnabled = true
            };
            SkyPlaneEffect = new BasicEffect(Game.GraphicsDevice)
            {
                VertexColorEnabled = true,
                FogEnabled = true,
                FogStart = 0,
                FogEnd = 64 * 0.8f,
                LightingEnabled = false,
            };
            var plane = new[]
            {
                new VertexPositionColor(new Vector3(-64, 0, -64), Color.Black),
                new VertexPositionColor(new Vector3(64, 0, -64), Color.Black),
                new VertexPositionColor(new Vector3(-64, 0, 64), Color.Black),

                new VertexPositionColor(new Vector3(64, 0, -64), Color.Black),
                new VertexPositionColor(new Vector3(64, 0, 64), Color.Black),
                new VertexPositionColor(new Vector3(-64, 0, 64), Color.Black)
            };
            SkyPlane = new VertexBuffer(Game.GraphicsDevice, VertexPositionColor.VertexDeclaration,
                plane.Length, BufferUsage.WriteOnly);
            SkyPlane.SetData(plane);
            var celestialPlane = new[]
            {
                new VertexPositionTexture(new Vector3(-60, 0, -60), new Vector2(0, 0)),
                new VertexPositionTexture(new Vector3(60, 0, -60), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(-60, 0, 60), new Vector2(0, 1)),

                new VertexPositionTexture(new Vector3(60, 0, -60), new Vector2(1, 0)),
                new VertexPositionTexture(new Vector3(60, 0, 60), new Vector2(1, 1)),
                new VertexPositionTexture(new Vector3(-60, 0, 60), new Vector2(0, 1))
            };
            CelestialPlane = new VertexBuffer(Game.GraphicsDevice, VertexPositionTexture.VertexDeclaration,
                celestialPlane.Length, BufferUsage.WriteOnly);
            CelestialPlane.SetData(celestialPlane);
        }

        private float CelestialAngle
        {
            get
            {
                float x = (Game.Client.World.World.TimeOfDay % 24000f) / 24000f - 0.25f;
                if (x < 0) x = 0;
                if (x > 1) x = 1;
                return x + ((1 - ((float)Math.Cos(x * MathHelper.Pi) + 1) / 2) - x) / 3;
            }
        }

        public static Color HSL2RGB(float h, float sl, float l)
        {
            // Thanks http://www.java2s.com/Code/CSharp/2D-Graphics/HSLtoRGBconversion.htm
            float v, r, g, b;
            r = g = b = l;   // default to gray
            v = (l <= 0.5f) ? (l * (1.0f + sl)) : (l + sl - l * sl);
            if (v > 0)
            {
                int sextant;
                float m, sv, fract, vsf, mid1, mid2;
                m = l + l - v;
                sv = (v - m) / v;
                h *= 6.0f;
                sextant = (int)h;
                fract = h - sextant;
                vsf = v * sv * fract;
                mid1 = m + vsf;
                mid2 = v - vsf;
                switch (sextant)
                {
                    case 0:
                        r = v; g = mid1; b = m;
                        break;
                    case 1:
                        r = mid2; g = v; b = m;
                        break;
                    case 2:
                        r = m; g = v; b = mid1;
                        break;
                    case 3:
                        r = m; g = mid2; b = v;
                        break;
                    case 4:
                        r = mid1; g = m; b = v;
                        break;
                    case 5:
                        r = v; g = m; b = mid2;
                        break;
                }
            }
            return new Color(r, g, b);
        }

        private Color BaseColor
        {
            get
            {
                const float temp = 0.8f / 3;
                return HSL2RGB(0.6222222f - temp * 0.05f, 0.5f + temp * 0.1f, BrightnessModifier);
            }
        }

        public float BrightnessModifier
        {
            get
            {
                var mod = (float)Math.Cos(CelestialAngle * MathHelper.TwoPi) * 2 + 0.5f;
                if (mod < 0) mod = 0;
                if (mod > 1) mod = 1;
                return mod;
            }
        }

        public Color WorldSkyColor => BaseColor;

        public Color WorldFogColor
        {
            get
            {
                float y = (float)Math.Cos(CelestialAngle * MathHelper.TwoPi) * 2 + 0.5f;
                return new Color(0.7529412f * y * 0.94f + 0.06f,
                    0.8470588f * y * 0.94f + 0.06f, 1.0f * y * 0.91f + 0.09f);
            }
        }

        public Color AtmosphereColor
        {
            get
            {
                const float blendFactor = 0.29f; // TODO: Compute based on view distance
                Func<float, float, float> blend = (float source, float destination) =>
                    destination + (source - destination) * blendFactor;
                var fog = WorldFogColor.ToVector3();
                var sky = WorldSkyColor.ToVector3();
                var color = new Vector3(blend(sky.X, fog.X), blend(sky.Y, fog.Y), blend(sky.Z, fog.Z));
                // TODO: more stuff
                return new Color(color);
            }
        }

        public void Draw(GameTime gameTime)
        {
            Game.GraphicsDevice.Clear(AtmosphereColor);
            Game.GraphicsDevice.SetVertexBuffer(SkyPlane);

            var position = Player.Camera.Position;
            var yaw = Player.Player.Yaw;
            Player.Camera.Position = Vector3.Zero;
            Player.Player.Yaw = 0;
            Player.Camera.Update(gameTime);
            Player.Camera.ApplyTo(SkyPlaneEffect);
            Player.Player.Yaw = yaw;
            Player.Camera.ApplyTo(CelestialPlaneEffect);
            Player.Camera.Position = position;
            // Sky
            SkyPlaneEffect.FogColor = AtmosphereColor.ToVector3();
            SkyPlaneEffect.World = Matrix.CreateRotationX(MathHelper.Pi)
                * Matrix.CreateTranslation(0, 100, 0)
                * Matrix.CreateRotationX(MathHelper.TwoPi * CelestialAngle);
            SkyPlaneEffect.AmbientLightColor = WorldSkyColor.ToVector3();
            foreach (var pass in SkyPlaneEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                SkyPlaneEffect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }

            // Sun
            Game.GraphicsDevice.SetVertexBuffer(CelestialPlane);
            var backup = Game.GraphicsDevice.BlendState;
            Game.GraphicsDevice.BlendState = BlendState.Additive;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.DepthRead;
            CelestialPlaneEffect.Texture = Game.GraphicsManager.SunTexture;
            CelestialPlaneEffect.World = Matrix.CreateRotationX(MathHelper.Pi)
                * Matrix.CreateTranslation(0, 100, 0)
                * Matrix.CreateRotationX(MathHelper.TwoPi * CelestialAngle);
            foreach (var pass in CelestialPlaneEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                CelestialPlaneEffect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }
            // Moon
            CelestialPlaneEffect.Texture = Game.GraphicsManager.MoonTexture;
            CelestialPlaneEffect.World = Matrix.CreateTranslation(0, -100, 0)
                * Matrix.CreateRotationX(MathHelper.TwoPi * CelestialAngle);
            foreach (var pass in CelestialPlaneEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                CelestialPlaneEffect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            }

            var currentViewMatrix = Player.Camera.View;

            var tod = Player.Player.World.World.TimeOfDay;

            var modelTransforms = new Matrix[SkyDome.Bones.Count];
            SkyDome.CopyAbsoluteBoneTransformsTo(modelTransforms);

            // Void
            //Game.GraphicsDevice.SetVertexBuffer(SkyPlane);
            //SkyPlaneEffect.World = Matrix.CreateTranslation(0, -16, 0);
            //SkyPlaneEffect.AmbientLightColor = WorldSkyColor.ToVector3()
            //    * new Vector3(0.2f, 0.2f, 0.6f)
            //    + new Vector3(0.04f, 0.04f, 0.1f);
            //foreach (var pass in SkyPlaneEffect.CurrentTechnique.Passes)
            //{
            //    pass.Apply();
            //    SkyPlaneEffect.GraphicsDevice.DrawPrimitives(PrimitiveType.TriangleList, 0, 2);
            //}

            //var wMatrix = Matrix.CreateTranslation(0, -0.1f, 0) *
            //              Matrix.CreateScale(100) * Matrix.CreateTranslation(Player.Camera.Position);
            //foreach (var mesh in SkyDome.Meshes)
            //{
            //    foreach (var currentEffect in mesh.Effects)
            //    {
            //        var worldMatrix = modelTransforms[mesh.ParentBone.Index] * wMatrix;

            //        currentEffect.CurrentTechnique = currentEffect.Techniques["SkyDome"];

            //        currentEffect.Parameters["xWorld"].SetValue(worldMatrix);
            //        currentEffect.Parameters["xView"].SetValue(currentViewMatrix);
            //        currentEffect.Parameters["xProjection"].SetValue(Player.Camera.Projection);
            //        currentEffect.Parameters["xTexture"].SetValue(CloudMap);
            //        currentEffect.Parameters["NightColor"].SetValue(Color.Black.ToVector3());
            //        currentEffect.Parameters["SunColor"].SetValue(Color.Yellow.ToVector3());
            //        currentEffect.Parameters["HorizonColor"].SetValue(WorldSkyColor.ToVector3());

            //        currentEffect.Parameters["MorningTint"].SetValue(Color.White.ToVector3());
            //        currentEffect.Parameters["EveningTint"].SetValue(Color.Red.ToVector3());
            //        currentEffect.Parameters["TimeOfDay"].SetValue(tod);
            //    }
            //    mesh.Draw();
            //}

            Game.GraphicsDevice.BlendState = backup;
            Game.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            
        }

        public void Update(GameTime gameTime)
        {
            // Generate the clouds
            var time = (float)gameTime.TotalGameTime.TotalMilliseconds / 100.0f;
            //CloudOvercast += 0.001f;
            GeneratePerlinNoise(time);
        }

        public void LoadContent(ContentManager content)
        {
            SkyDome = content.Load<Model>("Models\\dome");
            SkyDome.Meshes[0].MeshParts[0].Effect = content.Load<Effect>("Effects\\SkyDome");

            CloudMap = Game.GraphicsManager.CloudTexture;
            PerlinNoise = content.Load<Effect>("Effects\\PerlinNoise");

            var pp = Graphics.PresentationParameters;

            //the mipmap does not work on some pc ( i5 laptops at least), with mipmap false it s fine 

            CloudsRenderTarget = new RenderTarget2D(Graphics, pp.BackBufferWidth, pp.BackBufferHeight, false,

                SurfaceFormat.Color, DepthFormat.None);

            CloudStaticMap = CreateStaticMap(32);

            FullScreenVertices = SetUpFullscreenVertices();
        }

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        public virtual Texture2D CreateStaticMap(int resolution)
        {
            var rand = new Random();
            var noisyColors = new Color[resolution * resolution];
            for (var x = 0; x < resolution; x++)
                for (var y = 0; y < resolution; y++)
                    noisyColors[x + y * resolution] = new Color(new Vector3(rand.Next(1000) / 1000.0f, 0, 0));

            var noiseImage = new Texture2D(Graphics, resolution, resolution, true, SurfaceFormat.Color);
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
            Graphics.SetRenderTarget(CloudsRenderTarget);
            //_graphicsDevice.Clear(Color.White);

            PerlinNoise.CurrentTechnique = PerlinNoise.Techniques["PerlinNoise"];
            PerlinNoise.Parameters["xTexture"].SetValue(CloudStaticMap);
            PerlinNoise.Parameters["xOvercast"].SetValue(CloudOvercast);
            PerlinNoise.Parameters["xTime"].SetValue(time / 1000.0f);

            foreach (var pass in PerlinNoise.CurrentTechnique.Passes)
            {
                pass.Apply();
                Graphics.DrawUserPrimitives(PrimitiveType.TriangleStrip, FullScreenVertices, 0, 2);
            }

            Graphics.SetRenderTarget(null);
            CloudMap = CloudsRenderTarget;

        }

    }
}