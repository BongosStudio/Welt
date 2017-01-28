#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Welt.Cameras;
using Welt.Models;
using Welt.Processors;
using Welt.Types;
using System.Collections.Generic;
using Welt.Extensions;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.IO;

namespace Welt.Forge.Renderers
{
    internal class WorldRenderer : IRenderer
    {
        private static int m_RenderDistance = WeltGame.Instance.GameSettings.RenderDistance;
        private static int m_CacheDistance = WeltGame.Instance.GameSettings.CacheDistance;

        protected Effect BlockEffect;
        protected Texture2D TextureAtlas;

        private FirstPersonCamera m_Camera;
        private GraphicsDevice m_GraphicsDevice;
        private World m_World;
        private VertexBuildChunkProcessor m_Vertex;
        private LightingChunkProcessor m_Lighting;
        private RenderTarget2D m_RenderTarget;

        private BoundingBox m_ViewRangeBoundingBox;
        private BoundingBox m_CacheRangeBoundingBox;
        private float m_Tod;
        private bool m_IsRunning;
        private TaskFactory m_ChunkTaskFactory;
        private EventWaitHandle m_ChunkTaskHandle;
        private BlockingCollection<Vector3I> m_ChunksToLoad;
        private bool m_CacheThreadRunning;
        private Thread m_CacheThread;

        public WorldRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            m_GraphicsDevice = graphicsDevice;
            m_Camera = camera;
            m_World = world;
            m_World.Renderer = this;
            m_IsRunning = true;
            m_Vertex = new VertexBuildChunkProcessor(m_GraphicsDevice);
            m_Lighting = new LightingChunkProcessor();
            m_ChunkTaskFactory = new TaskFactory(TaskCreationOptions.RunContinuationsAsynchronously, TaskContinuationOptions.PreferFairness);
            m_ChunksToLoad = new BlockingCollection<Vector3I>();
            m_ChunkTaskHandle = new EventWaitHandle(true, EventResetMode.ManualReset);
            m_RenderTarget = new RenderTarget2D(graphicsDevice, WeltGame.Width, WeltGame.Height);
        }

        public void Initialize()
        {
            Player.Current.EnteredNewChunk += UpdateChunkCache;
            for (var x = 0; x < m_RenderDistance/2; ++x)
            {
                for (var z = 0; z < m_RenderDistance/2; ++z)
                {
                    var origin = new Vector3I((int)m_World.Origin.X, 0, (int)m_World.Origin.Y);
                    ProcessChunkInViewRange(m_World.ChunkManager.GetChunk(origin + new Vector3I(x, 0, z)));
                    ProcessChunkInViewRange(m_World.ChunkManager.GetChunk(origin + new Vector3I(-x, 0, z)));
                    ProcessChunkInViewRange(m_World.ChunkManager.GetChunk(origin + new Vector3I(x, 0, -z)));
                    ProcessChunkInViewRange(m_World.ChunkManager.GetChunk(origin + new Vector3I(-x, 0, -z)));
                    ProcessChunkInViewRange(m_World.ChunkManager.GetChunk(origin + new Vector3I(x, 0, z)));
                }
            }
        }

        private void UpdateChunkCache(object sender, EventArgs e)
        {
            Debug.WriteLine("recaching");
            m_ChunkTaskHandle.Set();
        }

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = WeltGame.Instance.GraphicsManager.BlockTexture;
            BlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
        }

        public void Update(GameTime gameTime)
        {
            var chunk = Player.Current.Chunk;
            UpdateBoundingBoxes();
            Process();
            if (m_CacheThreadRunning) return;
            m_CacheThreadRunning = true;
            m_CacheThread = new Thread(CacheThread);
            m_CacheThread.Start();
        }

        public void Draw(GameTime gameTime)
        {
            DrawChunks(gameTime);
        }

        public void Stop()
        {
            m_IsRunning = false;
            m_CacheThreadRunning = false;
            m_ChunkTaskHandle.Close();
            m_CacheThread.Abort();
        }

        #region DrawSolid

        private float m_RippleTime;
        private TimeSpan m_PreviousFullRebuild = TimeSpan.Zero;

        public event EventHandler LoadStepCompleted;

        private void DrawChunks(GameTime gameTime)
        {
            m_Tod = m_World.Tod;
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
            //BlockEffect.Parameters["ReflectionTexture"].SetValue(m_GraphicsDevice.Textures[0]);

            BlockEffect.Parameters["HorizonColor"].SetValue(m_World.HorizonColor);
            BlockEffect.Parameters["NightColor"].SetValue(m_World.NightColor);

            BlockEffect.Parameters["MorningTint"].SetValue(m_World.Morningtint);
            BlockEffect.Parameters["EveningTint"].SetValue(m_World.Eveningtint);

            BlockEffect.Parameters["DeepWaterColor"].SetValue(m_World.DeepWaterColor);
            BlockEffect.Parameters["HeldItemLight"].SetValue(Block.GetLightLevel(Player.Current.HeldItem).ToVector3());
            BlockEffect.Parameters["HasFlicker"].SetValue(Player.Current.HeldItem.Block.Id == BlockType.TORCH);
            BlockEffect.Parameters["SunColor"].SetValue(m_World.SunColor);
            BlockEffect.Parameters["TimeOfDay"].SetValue(m_Tod);
            BlockEffect.Parameters["SkyLightPosition"].SetValue(m_World.SunPos);
            BlockEffect.Parameters["RippleTime"].SetValue(m_RippleTime);
            BlockEffect.Parameters["Random"].SetValue((float)FastMath.NextRandomDouble());
            BlockEffect.Parameters["IsUnderWater"].SetValue(m_World.GetBlock(Player.Current.Position).Id == BlockType.WATER);

            var viewFrustum = new BoundingFrustum(m_Camera.View*m_Camera.Projection);
            m_GraphicsDevice.BlendState = BlendState.AlphaBlend;
            m_GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in BlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                var chunks = m_World.ChunkManager.Chunks.ToArray();
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

                for (var i = 0; i < chunks.Length; ++i)
                {
                    var chunk = chunks[i];
                    if (chunk == null) continue;
                    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                    if (chunk.SecondaryVertexCount == 0 || chunk.SecondaryVertexBuffer == null || chunk.SecondaryIndexBuffer == null)
                        continue;
                    m_GraphicsDevice.SetVertexBuffer(chunk.SecondaryVertexBuffer);
                    m_GraphicsDevice.Indices = chunk.SecondaryIndexBuffer;
                    m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.SecondaryVertexCount);
                }
            }
            //m_GraphicsDevice.SetRenderTarget(null);
        }

        #endregion

        private void UpdateBoundingBoxes()
        {
            m_ViewRangeBoundingBox = new BoundingBox(
                new Vector3(
                    Player.Current.Chunk.Position.X - (m_RenderDistance * 16), 0,
                    Player.Current.Chunk.Position.Z - (m_RenderDistance * 16)),
                new Vector3(
                    Player.Current.Chunk.Position.X + ((m_RenderDistance + 1) * 16), Chunk.Size.Y,
                    Player.Current.Chunk.Position.Z + ((m_RenderDistance + 1) * 16)));
            m_CacheRangeBoundingBox = new BoundingBox(
                new Vector3(
                    Player.Current.Chunk.Position.X - (m_CacheDistance * 16), 0,
                    Player.Current.Chunk.Position.Z - (m_CacheDistance * 16)),
                new Vector3(
                    Player.Current.Chunk.Position.X + ((m_CacheDistance + 1) * 16), Chunk.Size.Y,
                    Player.Current.Chunk.Position.Z + ((m_CacheDistance + 1) * 16)));
        }

        private void CacheThread()
        {
            Vector3I? lastCheckedIndex = null;
            while (m_CacheThreadRunning)
            {
                m_ChunkTaskHandle.WaitOne();
                // TODO: add surrounding chunks to cache
                for (var x = (int)m_CacheRangeBoundingBox.Min.X; x <= m_CacheRangeBoundingBox.Max.X; x += 16)
                {
                    for (var z = (int)m_CacheRangeBoundingBox.Min.Z; z <= m_CacheRangeBoundingBox.Max.Z; z += 16)
                    {
                        var position = new Vector3I(x, 0, z);
                        var chunk = m_World.ChunkAt(position);
                        if (IsChunkInViewRange(chunk)) continue;
                        ProcessChunkInCacheRange(chunk);
                        //m_ChunkTaskFactory.StartNew(() =>
                        //{
                        //    ProcessChunkInCacheRange(chunk);
                        //}, TaskCreationOptions.RunContinuationsAsynchronously);
                    }
                }
                GC.Collect();
                m_ChunkTaskHandle.Reset();
            }
        }

        private void RecacheChunks()
        {
            
            for (var x = (int)m_CacheRangeBoundingBox.Min.X; x <= m_CacheRangeBoundingBox.Max.X; x += 16)
            {
                for (var z = (int)m_CacheRangeBoundingBox.Min.Z; z <= m_CacheRangeBoundingBox.Max.Z; z += 16)
                {
                    var position = new Vector3I(x, 0, z);
                    var chunk = m_World.ChunkAt(position);
                    if (IsChunkInViewRange(chunk)) continue;
                    ProcessChunkInCacheRange(chunk);
                }
            }
            GC.Collect();
        }

        private void Process()
        {
            Parallel.ForEach(m_World.ChunkManager.Chunks.ToArray().Where(c => c != null && c.State != ChunkState.Ready),
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = Environment.ProcessorCount
                },
                (chunk) =>
                {
                    if (IsChunkInViewRange(chunk))
                    {
                        ProcessChunkInViewRange(chunk);
                    }
                    else if (IsChunkInCacheRange(chunk))
                    {
                        m_ChunkTaskFactory.StartNew(() =>
                        {
                            ProcessChunkInCacheRange(chunk);
                            //if (IsChunkInCacheRange(chunk.N))
                            //    ProcessChunkInCacheRange(chunk.N);
                            //if (IsChunkInCacheRange(chunk.S))
                            //    ProcessChunkInCacheRange(chunk.S);
                            //if (IsChunkInCacheRange(chunk.E))
                            //    ProcessChunkInCacheRange(chunk.E);
                            //if (IsChunkInCacheRange(chunk.W))
                            //    ProcessChunkInCacheRange(chunk.W);
                        });
                    }
                    else
                    {
                        m_World.ChunkManager.RemoveChunk(chunk);
                    }

                });
        }

        private bool IsChunkInViewRange(Chunk chunk)
        {
            return m_ViewRangeBoundingBox.Contains(chunk.BoundingBox) == ContainmentType.Contains;
        }

        private bool IsChunkInCacheRange(Chunk chunk)
        {
            if (chunk == null) return false;
            return m_CacheRangeBoundingBox.Contains(chunk.BoundingBox) == ContainmentType.Contains;
        }

        private void ProcessChunkInViewRange(Chunk chunk)
        {
            if (chunk == null) return;
            switch (chunk.State)
            {
                case ChunkState.AwaitingGenerate:
                    m_World.Generator.Generate(m_World, chunk);
                    m_World.ChunkManager.SetChunk(chunk.Index, chunk);
                    break;
                    //goto case ChunkState.AwaitingRelighting;
                case ChunkState.AwaitingLighting:
                case ChunkState.AwaitingRelighting:
                    m_Lighting.ProcessChunk(chunk);
                    m_World.ChunkManager.SetChunk(chunk.Index, chunk);
                    break;
                    //goto case ChunkState.AwaitingRebuild;
                case ChunkState.AwaitingBuild:
                case ChunkState.AwaitingRebuild:
                    m_Vertex.ProcessChunk(chunk);
                    m_World.ChunkManager.SetChunk(chunk.Index, chunk);
                    break;
                default:
                    break;
            }
        }

        private void ProcessChunkInCacheRange(Chunk chunk)
        {
            switch (chunk.State)
            {
                case ChunkState.AwaitingGenerate:
                    m_World.Generator.Generate(m_World, chunk);
                    m_World.ChunkManager.SetChunk(chunk.Index, chunk);
                    break;
                case ChunkState.AwaitingLighting:
                    m_Lighting.ProcessChunk(chunk);
                    m_World.ChunkManager.SetChunk(chunk.Index, chunk);
                    break;
            }
        }
    }
}