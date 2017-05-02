using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Forge.Renderers;
using Welt.Cameras;
using Welt.Forge;
using Welt.Core.Forge;
using Welt.API;
using System.Collections.Concurrent;
using Welt.Graphics;
using Welt.Events.Forge;
using Welt.Core;
using Welt.API.Forge;
using Welt.Extensions;
using System.IO;
using Welt.Lighting;
using System.Threading.Tasks;
using System.Linq;
using System.Diagnostics;

namespace Welt.Components
{
    public class ChunkComponent : IVisualComponent
    {
        public GraphicsDevice Graphics { get; }

        public WeltGame Game { get; }

        public PlayerRenderer PlayerRenderer;
        public ChunkRenderer Renderer;
        public int ChunksRendered;
        public ReadOnlyWorld World { get; set; }
        public LightEngine LightingEngine; // perhaps combine WorldLighting with LightEngine?
        public WorldLighting Lighting;

        protected Effect BlockEffect;
        protected Effect PointLight;
        protected Effect DirectionalLight;
        protected Texture2D TextureAtlas;
        private HashSet<Vector3I> m_ActiveMeshes;
        private List<ChunkMesh> m_Meshes;
        private ConcurrentBag<Mesh<VertexPositionNormalTextureEffect>> m_IncomingChunks;

        #region Deferred Rendering
        
        private QuadRenderer m_QuadRenderer;

        private RenderTarget2D m_ColorRt;
        private RenderTarget2D m_NormalRt;
        private RenderTarget2D m_DepthRt;
        private RenderTarget2D m_LightRt;

        private Effect m_ClearGBufferEffect;
        private Effect m_CombineFinalEffect;

        private Model m_LightSphereModel;

        private Vector2 m_HalfPixel;

        #endregion

        private float m_RippleTime;

        public ChunkComponent(WeltGame game, GraphicsDevice graphics, PlayerRenderer player, ReadOnlyWorld world)
        {
            Game = game;
            Graphics = graphics;

            PlayerRenderer = player;
            World = world;
            Renderer = new ChunkRenderer(world, game, game.Client.BlockRepository);
            Lighting = new WorldLighting(game.Client.BlockRepository);
            m_ActiveMeshes = new HashSet<Vector3I>();
            m_IncomingChunks = new ConcurrentBag<Mesh<VertexPositionNormalTextureEffect>>();
            m_Meshes = new List<ChunkMesh>();
            LightingEngine = new LightEngine(game, player.Player, world);

            PlayerRenderer.Player.BlockChanged += HandleBlockChanged;
            PlayerRenderer.Player.ChunkModified += HandleChunkModified;
            PlayerRenderer.Player.ChunkLoaded += HandleChunkLoaded;
            PlayerRenderer.Player.ChunkUnloaded += HandleChunkUnloaded;

            Renderer.MeshCompleted += HandleMeshCompleted;
        }

        public void Dispose()
        {
            PlayerRenderer = null;
            Renderer.Dispose();
            LightingEngine.Stop();
            World = null;
            Lighting = null;
            BlockEffect.Dispose();
            TextureAtlas.Dispose();
            m_ActiveMeshes.Clear();
            m_Meshes.Clear();
            m_IncomingChunks = null;
        }

        public void Draw(GameTime gameTime)
        {
            //SetGBuffer();
            //ClearGBuffer();
            DrawChunks();
            //ResolveGBuffer();
            //DrawLights(gameTime);
        }

        public void Initialize()
        {
            var width = Graphics.PresentationParameters.BackBufferWidth;
            var height = Graphics.PresentationParameters.BackBufferHeight;
            var depth = Graphics.PresentationParameters.DepthStencilFormat;
            m_HalfPixel = new Vector2(0.5f / width, 0.5f / height);
            m_QuadRenderer = new QuadRenderer(Game, Graphics);
            m_ColorRt = new RenderTarget2D(Graphics, width, height, false, SurfaceFormat.Color, DepthFormat.Depth24);
            m_NormalRt = new RenderTarget2D(Graphics, width, height, false, SurfaceFormat.Color, DepthFormat.None);
            m_DepthRt = new RenderTarget2D(Graphics, width, height, false, SurfaceFormat.Single, DepthFormat.None);
            m_LightRt = new RenderTarget2D(Graphics, width, height, false, SurfaceFormat.Color, DepthFormat.None);

            PlayerRenderer.Initialize();
            LightingEngine.Initialize();
            Renderer.Start();

        }

        public void LoadContent(ContentManager content)
        {
            BlockEffect = content.Load<Effect>("Effects\\BlockEffect");
            PointLight = content.Load<Effect>("Effects\\PointLight");
            DirectionalLight = content.Load<Effect>("Effects\\DirectionalLight");
            m_ClearGBufferEffect = content.Load<Effect>("Effects\\ClearGBuffer");
            m_CombineFinalEffect = content.Load<Effect>("Effects\\CombineFinal");
            m_LightSphereModel = content.Load<Model>("Models\\sphere");
            TextureAtlas = Game.GraphicsManager.BlockTexture;
        }

        public void Update(GameTime gameTime)
        {
            while (m_IncomingChunks.TryTake(out var result))
            {
                var mesh = result as ChunkMesh;
                if (m_ActiveMeshes.Contains(mesh.Chunk.GetIndex()))
                {
                    var index = m_Meshes.FindIndex(c => c.Chunk.GetIndex() == mesh.Chunk.GetIndex());
                    m_Meshes[index] = mesh;
                }
                else
                {
                    m_ActiveMeshes.Add(mesh.Chunk.GetIndex());
                    m_Meshes.Add(mesh);
                }
            }
            m_RippleTime += 0.1f;
            if (m_RippleTime == 1.0f) m_RippleTime = 0;
        }

        #region Private methods
        
        private void DrawChunks()
        {
            var tod = World.World.TimeOfDay;

            BlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            BlockEffect.Parameters["View"].SetValue(PlayerRenderer.CameraController.Camera.View);
            BlockEffect.Parameters["Projection"].SetValue(PlayerRenderer.CameraController.Camera.Projection);
            BlockEffect.Parameters["CameraPosition"].SetValue(PlayerRenderer.CameraController.Camera.Position);

            BlockEffect.Parameters["FogNear"].SetValue(World.World.FogNear);
            BlockEffect.Parameters["FogFar"].SetValue(World.World.FogFar);
            BlockEffect.Parameters["BlockTexture"].SetValue(TextureAtlas);

            BlockEffect.Parameters["HorizonColor"].SetValue(World.World.HorizonColor);
            BlockEffect.Parameters["NightColor"].SetValue(World.World.NightColor);

            BlockEffect.Parameters["MorningTint"].SetValue(World.World.MorningTint);
            BlockEffect.Parameters["EveningTint"].SetValue(World.World.EveningTint);

            BlockEffect.Parameters["DeepWaterColor"].SetValue(World.World.DeepWaterColor);
            
            BlockEffect.Parameters["SunColor"].SetValue(World.World.SunColor);
            BlockEffect.Parameters["TimeOfDay"].SetValue((float)tod);
            BlockEffect.Parameters["RippleTime"].SetValue(m_RippleTime);
            BlockEffect.Parameters["Random"].SetValue((float)FastMath.NextRandomDouble());
            BlockEffect.Parameters["IsUnderWater"].SetValue(World.GetBlock(PlayerRenderer.Player.Position).Id == BlockType.WATER);

            var viewFrustum = new BoundingFrustum(PlayerRenderer.CameraController.Camera.View * PlayerRenderer.CameraController.Camera.Projection);
            Graphics.BlendState = BlendState.AlphaBlend;
            Graphics.DepthStencilState = DepthStencilState.Default;
            //Graphics.RasterizerState = RasterizerState.CullClockwise;
            var rendered = 0;
            if (m_Meshes.Count == 0) return;
            for (var i = 0; i < m_Meshes.Count; i++)
            {
                var chunk = m_Meshes[i];
                if (!viewFrustum.Intersects(chunk.BoundingBox)) continue;
                if (!chunk.IsReady) continue;
                chunk.Draw(BlockEffect, 0);
                rendered++;
            }
            for (var i = 0; i < m_Meshes.Count; i++)
            {
                var chunk = m_Meshes[i];
                if (!viewFrustum.Intersects(chunk.BoundingBox)) continue;
                if (!chunk.IsReady) continue;
                chunk.Draw(BlockEffect, 1);
            }
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
        }
        
        private void DrawLights(GameTime gameTime)
        {
            Game.SetRenderTarget(m_LightRt, Color.Transparent);
            Graphics.BlendState = BlendState.AlphaBlend;
            Graphics.DepthStencilState = DepthStencilState.None;
            foreach (var light in LightingEngine.Lights)
            {
                if (light.Type == LightType.Point)
                    DrawPointLight(light);
                else
                    DrawDirectionalLight(light);
            }
            Graphics.BlendState = BlendState.Opaque;
            Graphics.DepthStencilState = DepthStencilState.None;
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;

            Game.ClearRenderTarget();

            m_CombineFinalEffect.Parameters["ColorMap"].SetValue(m_ColorRt);
            m_CombineFinalEffect.Parameters["LightMap"].SetValue(m_LightRt);
            m_CombineFinalEffect.Parameters["HalfPixel"].SetValue(m_HalfPixel);

            m_CombineFinalEffect.Techniques[0].Passes[0].Apply();
            m_QuadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        private void DrawPointLight(Light light)
        {
            PointLight.Parameters["ColorMap"].SetValue(m_ColorRt);
            PointLight.Parameters["NormalMap"].SetValue(m_NormalRt);
            PointLight.Parameters["Depthmap"].SetValue(m_DepthRt);
            var sphereWorldMatrix = Matrix.CreateScale(light.Intensity * 10) * Matrix.CreateTranslation(light.Position);
            PointLight.Parameters["World"].SetValue(sphereWorldMatrix);
            PointLight.Parameters["View"].SetValue(PlayerRenderer.CameraController.Camera.View);
            PointLight.Parameters["Projection"].SetValue(PlayerRenderer.CameraController.Camera.Projection);
            PointLight.Parameters["LightPosition"].SetValue(light.Position);
            PointLight.Parameters["LightColor"].SetValue(light.Color);
            PointLight.Parameters["LightRadius"].SetValue(light.Intensity * 10);
            PointLight.Parameters["LightIntensity"].SetValue(light.Intensity);

            PointLight.Parameters["CameraPosition"].SetValue(PlayerRenderer.CameraController.Camera.Position);
            PointLight.Parameters["InvertViewProjection"].SetValue(Matrix.Invert(PlayerRenderer.CameraController.Camera.View * PlayerRenderer.CameraController.Camera.Projection));
            PointLight.Parameters["HalfPixel"].SetValue(m_HalfPixel);

            var cameraToCenter = Vector3.Distance(PlayerRenderer.CameraController.Camera.Position, light.Position);
            if (cameraToCenter > light.Intensity * 10)
            {
                Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            }
            else
            {
                Graphics.RasterizerState = RasterizerState.CullClockwise;
            }

            Graphics.DepthStencilState = DepthStencilState.None;
            PointLight.Techniques[0].Passes[0].Apply();
            foreach (var mesh in m_LightSphereModel.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    Graphics.Indices = part.IndexBuffer;
                    Graphics.SetVertexBuffer(part.VertexBuffer);
                    Graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, part.StartIndex, part.PrimitiveCount);
                }
            }
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
            Graphics.DepthStencilState = DepthStencilState.Default;
        }

        private void DrawDirectionalLight(Light light)
        {

        }

        private void HandleChunkModified(object sender, ChunkEventArgs args)
        {
            Renderer.Enqueue(args.Chunk, true);
        }

        private void HandleChunkLoaded(object sender, ChunkEventArgs args)
        {
            if (!Renderer.Enqueue(args.Chunk)) return;
            foreach (var adj in m_AdjacentCoords)
            {
                var chunk = World.GetChunk(adj + args.Chunk.GetIndex());
                if (chunk == null) continue;
                Renderer.Enqueue(chunk);
            }
        }

        private void HandleChunkUnloaded(object sender, ChunkEventArgs args)
        {
            m_ActiveMeshes.Remove(args.Chunk.GetIndex());
            m_Meshes.RemoveAll(c => c.Chunk.GetIndex() == args.Chunk.GetIndex());
        }

        private void HandleBlockChanged(object sender, BlockChangedEventArgs args)
        {
            Lighting.ProcessChunk(args.Chunk.Chunk);
        }

        private void HandleMeshCompleted(object sender, RendererEventArgs<ReadOnlyChunk, VertexPositionNormalTextureEffect> args)
        {
            m_IncomingChunks.Add(args.Result);
        }

        private static Vector3I[] m_AdjacentCoords =
        {
            Vector3I.Forward, Vector3I.Backward,
            Vector3I.Right, Vector3I.Left
        };

        private static Vector3[] m_CubeFaceVectors =
        {
            Vector3.Right, Vector3.Left,
            Vector3.Up, Vector3.Down,
            Vector3.Forward, Vector3.Backward
        };

        private void SetGBuffer()
        {
            Graphics.SetRenderTargets(m_ColorRt, m_NormalRt, m_DepthRt);
        }

        private void ResolveGBuffer()
        {
            Graphics.SetRenderTarget(Game.RenderTarget);
        }

        private void ClearGBuffer()
        {
            m_ClearGBufferEffect.Techniques[0].Passes[0].Apply();
            m_QuadRenderer.Render(Vector2.One * -1, Vector2.One);
        }

        #endregion  
    }
}
