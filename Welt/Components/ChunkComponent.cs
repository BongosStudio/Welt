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
using Welt.Particles;

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
        public WorldLighting Lighting;
        public ParticleSystem Particles;

        protected Effect BlockEffect;
        protected Texture2D TextureAtlas;
        private HashSet<Vector3I> m_ActiveMeshes;
        private List<ChunkMesh> m_Meshes;
        private ConcurrentBag<Mesh<VertexPositionNormalTextureEffect>> m_IncomingChunks;
        
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
            Particles = new ParticleSystem(game, game.Content, player.Camera, new ParticleSettings
            {
                TextureName = "Textures/rain",
                MaxParticles = 100,
                Duration = TimeSpan.FromSeconds(10),
                MinHorizontalVelocity = 0,
                MaxHorizontalVelocity = 10,
                MinEndSize = 10,
                MaxEndSize = 15,
                MinVerticalVelocity = 50,
                MaxVerticalVelocity = 50,
                MinStartSize = 10,
                MinColor = Color.LightBlue,
                MaxColor = Color.DarkBlue,
                EmitterVelocitySensitivity = 1,
                BlendState = BlendState.AlphaBlend
            });

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
            DrawChunks(gameTime);
            Particles.Draw(gameTime);
        }

        public void Initialize()
        {
            PlayerRenderer.Initialize();
            Renderer.Start();

        }

        public void LoadContent(ContentManager content)
        {
            BlockEffect = content.Load<Effect>("Effects\\BlockEffect");
            
            TextureAtlas = Game.GraphicsManager.BlockTexture;
            Particles.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            var processed = 0;
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
                processed++;
            }
            m_RippleTime += 0.1f;
            if (m_RippleTime == 1.0f) m_RippleTime = 0;
            Particles.SetCamera(PlayerRenderer.Camera.View, PlayerRenderer.Camera.Projection);
            for (var i = 0; i < 100; i++)
            {
                Particles.AddParticle(PlayerRenderer.Player.Position + new Vector3(0, 20, 0), new Vector3(0, 15, 0));
            }
            Particles.Update(gameTime);
        }

        /// <summary>
        ///     Flushes all pending meshes to the render collection.
        /// </summary>
        public void FlushIncoming()
        {
            var processed = 0;
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
                processed++;
            }
        }

        #region Private methods
        
        private void DrawChunks(GameTime gameTime)
        {
            Game.ResetRenderTarget();
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
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
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

            Graphics.BlendState = BlendState.NonPremultiplied;
            //BlockEffect.Parameters["ReflectionTexture"].SetValue(ReflectionTarget);
            for (var i = 0; i < m_Meshes.Count; i++)
            {
                var chunk = m_Meshes[i];
                if (!viewFrustum.Intersects(chunk.BoundingBox)) continue;
                if (!chunk.IsReady) continue;
                chunk.Draw(BlockEffect, 1);
            }
            Graphics.RasterizerState = RasterizerState.CullCounterClockwise;
        }
        
        private void HandleChunkModified(object sender, ChunkEventArgs args)
        {
            //var zeroedPosition = new Vector2(PlayerRenderer.Player.Position.X, PlayerRenderer.Player.Position.Z);
            //var zeroedChunk = new Vector2(args.Chunk.GetPosition().X, args.Chunk.GetPosition().Z);
            //if (Vector2.Distance(zeroedPosition, zeroedChunk) * Chunk.Width > 8)
            //    Renderer.Enqueue(args.Chunk, RenderPriority.Elevated);
            //else
                Renderer.Enqueue(args.Chunk, RenderPriority.Highest);
        }

        private void HandleChunkLoaded(object sender, ChunkEventArgs args)
        {
            if (!Renderer.Enqueue(args.Chunk)) return;
            foreach (var adj in m_AdjacentCoords)
            {
                var chunk = World.GetChunk(adj + args.Chunk.GetIndex());
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
        
        #endregion  
    }
}
