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

namespace Welt.Forge.Renderers
{
    internal class WorldRenderer : IRenderer
    {
        private const byte BUILD_RANGE = 4;
        private const byte LIGHT_RANGE = BUILD_RANGE + 1;
        private const byte GENERATE_RANGE_LOW = LIGHT_RANGE + 1;
        private const byte GENERATE_RANGE_HIGH = GENERATE_RANGE_LOW;
        private readonly FirstPersonCamera m_Camera;
        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly World m_World;
        private LightingChunkProcessor m_Lighting;
        protected Effect SolidBlockEffect;
        protected Texture2D TextureAtlas;
        private float m_Tod;
        private bool m_IsRunning;
        private bool m_IsInit;
        private VertexBuildChunkProcessor m_Vertex;
        private Stack<Vector3I> m_ToBeRebuilt;

        public List<Vector3I> ChunksToDraw { get; set; }

        public WorldRenderer(GraphicsDevice graphicsDevice, FirstPersonCamera camera, World world)
        {
            m_GraphicsDevice = graphicsDevice;
            m_Camera = camera;
            m_World = world;
            m_World.Renderer = this;
            m_IsRunning = true;
            m_IsInit = false;
            m_Vertex = new VertexBuildChunkProcessor(m_GraphicsDevice);
            m_Lighting = new LightingChunkProcessor();
            m_ToBeRebuilt = new Stack<Vector3I>();
        }

        public void Initialize()
        {
            if (m_IsInit) return;

            m_World.Chunks.Initialize();
            while (m_ToBeRebuilt.Count > 0)
            {
                var start = DateTime.Now;
                var r = m_ToBeRebuilt.Pop();
                //Debug.WriteLine($"Building {r}");
                _RebuildChunk(m_World.Chunks.GetChunk(r), false);
                Debug.WriteLine($"Took {(DateTime.Now - start).TotalMilliseconds}ms to generate {r}");
            }
            Debug.WriteLine("Completed building chunks");
            m_IsInit = true;
            m_World.BlockChanged += (sender, args) =>
            {
                //RebuildChunk(args.Chunk, false);
                //_RebuildChunk(args.Chunk, false);
                //if (args.Position.X == 0)
                //    _RebuildChunk(args.Chunk.W, false);
                //if (args.Position.X == 15)
                //    _RebuildChunk(args.Chunk.E, false);
                //if (args.Position.Z == 0)
                //    _RebuildChunk(args.Chunk.N, false);
                //if (args.Position.Z == 15)
                //    _RebuildChunk(args.Chunk.S, false);
                
            };
            m_World.Chunks.BeginPolling();
            //WeltGame.Instance.TaskManager.ExecuteInBackground(() =>
            //{
            //    while (true)
            //    {
            //        if (m_ToBeRebuilt.Count == 0) continue;
            //        var rebuildChunk = m_World.Chunks.GetChunk(m_ToBeRebuilt.Pop(), false);
            //        switch (rebuildChunk.State)
            //        {
            //            case ChunkState.AwaitingGenerate:
            //                DoGenerate(rebuildChunk);
            //                goto case ChunkState.AwaitingLighting;
            //            case ChunkState.AwaitingLighting:
            //            case ChunkState.AwaitingRelighting:
            //                DoLighting(rebuildChunk);
            //                goto case ChunkState.AwaitingBuild;
            //            case ChunkState.AwaitingBuild:
            //            case ChunkState.AwaitingRebuild:
            //                DoBuild(rebuildChunk);
            //                rebuildChunk.State = ChunkState.Ready;
            //                break;
            //        }
            //    }
            //}, true);
        }

        public void LoadContent(ContentManager content)
        {
            TextureAtlas = content.Load<Texture2D>("Textures\\terrain");
            SolidBlockEffect = content.Load<Effect>("Effects\\SolidBlockEffect");
        }

        public void Update(GameTime gameTime)
        {
            if (!m_IsRunning) return;
            //m_World.Chunks.Update();
            for (var i = 0; i < 5; i++)
            {
                if (m_ToBeRebuilt.Count == 0) break;
                var chunk = m_World.Chunks.GetChunk(m_ToBeRebuilt.Pop());
                _RebuildChunk(chunk, false);
            }
        }

        public void Draw(GameTime gameTime)
        {
            if (!m_IsRunning) return;
            DrawChunks(gameTime);
        }

        public void Stop()
        {
            m_IsRunning = false;
        }

        public event EventHandler LoadStepCompleted;

        private void _RebuildChunk(Chunk rebuildChunk, bool fullRebuild = true)
        {
            switch (rebuildChunk.State)
            {
                case ChunkState.AwaitingGenerate:
                    DoGenerate(rebuildChunk);
                    goto case ChunkState.AwaitingLighting;
                case ChunkState.AwaitingLighting:
                case ChunkState.AwaitingRelighting:
                    DoLighting(rebuildChunk);
                    if (fullRebuild)
                    {
                        DoLighting(rebuildChunk.N);
                        DoLighting(rebuildChunk.S);
                        DoLighting(rebuildChunk.W);
                        DoLighting(rebuildChunk.E);
                        DoLighting(rebuildChunk.Nw);
                        DoLighting(rebuildChunk.Ne);
                        DoLighting(rebuildChunk.Sw);
                        DoLighting(rebuildChunk.Se);
                    }
                    goto case ChunkState.AwaitingBuild;
                case ChunkState.AwaitingBuild:
                case ChunkState.AwaitingRebuild:
                    DoBuild(rebuildChunk);
                    if (fullRebuild)
                    {
                        DoBuild(rebuildChunk.N);
                        DoBuild(rebuildChunk.S);
                        DoBuild(rebuildChunk.W);
                        DoBuild(rebuildChunk.E);
                        DoBuild(rebuildChunk.Nw);
                        DoBuild(rebuildChunk.Ne);
                        DoBuild(rebuildChunk.Sw);
                        DoBuild(rebuildChunk.Se);
                    }
                    rebuildChunk.State = ChunkState.Ready;
                    break;
            }
        }
        public void RebuildChunk(Chunk rebuildChunk, bool fullRebuild = true)
        {
            if (m_ToBeRebuilt.Contains(rebuildChunk.Index)) return;
            m_ToBeRebuilt.Push(rebuildChunk.Index);
        }

        public void DoLightFor(Chunk chunk)
        {
            m_Lighting.ProcessChunk(chunk);
        }

        #region DrawSolid

        private float m_RippleTime;
        private TimeSpan m_PreviousFullRebuild = TimeSpan.Zero;
        private void DrawChunks(GameTime gameTime)
        {
            m_Tod = m_World.Tod;
            m_RippleTime += 0.1f;
            if (m_RippleTime == 1.0f) m_RippleTime = 0;
            m_PreviousFullRebuild += gameTime.ElapsedGameTime;

            SolidBlockEffect.Parameters["World"].SetValue(Matrix.Identity);
            SolidBlockEffect.Parameters["View"].SetValue(m_Camera.View);
            SolidBlockEffect.Parameters["Projection"].SetValue(m_Camera.Projection);
            SolidBlockEffect.Parameters["CameraPosition"].SetValue(m_Camera.Position);
            SolidBlockEffect.Parameters["FogNear"].SetValue(m_World.Fognear);
            SolidBlockEffect.Parameters["FogFar"].SetValue(m_World.Fogfar);
            SolidBlockEffect.Parameters["Texture1"].SetValue(TextureAtlas);

            SolidBlockEffect.Parameters["HorizonColor"].SetValue(m_World.Horizoncolor);
            SolidBlockEffect.Parameters["NightColor"].SetValue(m_World.Nightcolor);

            SolidBlockEffect.Parameters["MorningTint"].SetValue(m_World.Morningtint);
            SolidBlockEffect.Parameters["EveningTint"].SetValue(m_World.Eveningtint);

            SolidBlockEffect.Parameters["DeepWaterColor"].SetValue(m_World.DeepWaterColor);
            SolidBlockEffect.Parameters["HeldItemLight"].SetValue(Block.GetLightLevel(Player.Current.HeldItem).ToVector3());
            SolidBlockEffect.Parameters["HasFlicker"].SetValue(Player.Current.HeldItem.Block.Id == BlockType.TORCH);
            SolidBlockEffect.Parameters["SunColor"].SetValue(m_World.Suncolor);
            SolidBlockEffect.Parameters["TimeOfDay"].SetValue(m_Tod);
            SolidBlockEffect.Parameters["RippleTime"].SetValue(m_RippleTime);
            SolidBlockEffect.Parameters["Random"].SetValue((float)FastMath.NextRandomDouble());
            SolidBlockEffect.Parameters["IsUnderWater"].SetValue(m_World.GetBlock(Player.Current.Position).Id == BlockType.WATER);

            var viewFrustum = new BoundingFrustum(m_Camera.View*m_Camera.Projection);
            m_GraphicsDevice.BlendState = BlendState.AlphaBlend;
            m_GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            foreach (var pass in SolidBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();

                var chunks = m_World.Chunks.Chunks.ToArray();
                for (var i = 0; i < chunks.Length; ++i)
                {
                    var chunk = chunks[i];
                    if (chunk == null) continue;
                    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                    //if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);
                    m_GraphicsDevice.SetVertexBuffer(chunk.PrimaryVertexBuffer);
                    m_GraphicsDevice.Indices = chunk.PrimaryIndexBuffer;
                    if (chunk.PrimaryVertexBuffer != null && chunk.PrimaryVertexCount > 0 && chunk.PrimaryIndexBuffer != null)
                        m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.PrimaryVertexCount);
                }

                for (var i = 0; i < chunks.Length; ++i)
                {
                    var chunk = chunks[i];
                    if (chunk == null) continue;
                    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                    //RebuildChunk(chunk, false);
                    m_GraphicsDevice.SetVertexBuffer(chunk.SecondaryVertexBuffer);
                    m_GraphicsDevice.Indices = chunk.SecondaryIndexBuffer;
                    if (chunk.SecondaryVertexBuffer != null && chunk.SecondaryVertexCount > 0 && chunk.SecondaryIndexBuffer != null)
                        m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.SecondaryVertexCount);
                }

                //foreach (var chunk in chunks)
                //{
                //    if (chunk == null) continue;
                //    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                //    if (chunk.State != ChunkState.Ready) RebuildChunk(chunk);
                //    m_GraphicsDevice.SetVertexBuffer(chunk.PrimaryVertexBuffer);
                //    m_GraphicsDevice.Indices = chunk.PrimaryIndexBuffer;
                //    try
                //    {
                //        if (chunk.PrimaryVertexBuffer != null && chunk.PrimaryVertexCount > 0 && chunk.PrimaryIndexBuffer != null)
                //            m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.PrimaryVertexCount);
                //    }
                //    catch (ArgumentOutOfRangeException e)
                //    {
                //        Console.WriteLine($"Argument out of range - vertex count:{chunk.PrimaryVertexCount}");
                //    }
                //}
                
                //foreach (var chunk in chunks)
                //{
                //    if (chunk == null) continue;
                //    if (!chunk.BoundingBox.Intersects(viewFrustum)) continue;
                //    //RebuildChunk(chunk, false);
                //    m_GraphicsDevice.SetVertexBuffer(chunk.SecondaryVertexBuffer);
                //    m_GraphicsDevice.Indices = chunk.SecondaryIndexBuffer;
                //    try
                //    {
                //        if (chunk.SecondaryVertexBuffer != null && chunk.SecondaryVertexCount > 0 && chunk.SecondaryIndexBuffer != null)
                //            m_GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.SecondaryVertexCount);
                //    }
                //    catch (ArgumentOutOfRangeException e)
                //    {
                //        Console.WriteLine($"Argument out of range - vertex count:{chunk.SecondaryVertexCount}");
                //    }
                //}
            }
        }

        #endregion

        private Chunk DoChunkCreation(Vector3I index)
        {
            var chunk = m_World.Chunks.GetChunk(index);
            RebuildChunk(chunk);
            return chunk;
        }

        private Chunk DoLighting(Vector3I chunkIndex)
        {
            var chunk = m_World.Chunks.GetChunk(chunkIndex);
            return DoLighting(chunk);
        }

        private Chunk DoLighting(Chunk chunk)
        {
            m_Lighting.ProcessChunk(chunk);
            return chunk;
        }

        private Chunk DoBuild(Vector3I chunkIndex)
        {
            var chunk = m_World.Chunks.GetChunk(chunkIndex);
            if (chunk?.Dirty == false) return chunk;
            return DoBuild(chunk);
        }

        private Chunk DoBuild(Chunk chunk)
        {
            m_Vertex.ProcessChunk(chunk);
            if (chunk != null) chunk.Dirty = false;
            return chunk;
        }

        private Chunk DoGenerate(Vector3I chunkIndex)
        {
            var chunk = new Chunk(m_World, chunkIndex);
            return DoGenerate(chunk);
        }

        private Chunk DoGenerate(Chunk chunk)
        {
            m_World.Generator.Generate(m_World, chunk);
            m_World.Chunks.SetChunk(chunk.Index, chunk);     
            return chunk;
        }
        
    }
}