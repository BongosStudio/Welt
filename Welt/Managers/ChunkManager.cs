#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Welt.Forge;
using Welt.Models;
using Welt.Persistence;
using Welt.Types;

#endregion

namespace Welt.Managers
{
    public class ChunkManager
    {
        private const int RENDER_DISTANCE = 2;
        public World World { get; }
        public IChunkPersistence Persistence { get; }
        private Queue<Vector3I> m_ToBeRemoved;
        private Queue<int> m_ToBeBuilt;
        private Dictionary<Vector3I, Chunk> m_LoadedChunks;
        private Vector3I m_FocalChunk;
        private object m_ChunkLock = new object();
        public List<Chunk> Chunks { get; private set; }

        public ChunkManager(IChunkPersistence persistence, World world)
        {
            World = world;
            Persistence = persistence;
            m_LoadedChunks = new Dictionary<Vector3I, Chunk>();
            Chunks = new List<Chunk>(RENDER_DISTANCE*RENDER_DISTANCE);
            m_ToBeRemoved = new Queue<Vector3I>();
            m_ToBeBuilt = new Queue<int>();
        }

        public void Initialize()
        {
            DoGenerate(World.Origin);
        }

        public Chunk GetChunk(uint x, uint y, uint z, bool generate = true)
        {
            return GetChunk(new Vector3I(x, y, z), generate);
        }

        public Chunk GetChunk(Vector3I index, bool generate = true)
        {
            index %= World.Size;
            if (m_LoadedChunks.ContainsKey(index)) return m_LoadedChunks[index];
            if (TryLoad(index, out var chunk)) return chunk;
            if (!generate) return null;
            chunk = new Chunk(World, index);
            World.Generator.Generate(World, chunk);
            return chunk;
        }

        public void SetChunk(Vector3I index, Chunk chunk)
        {
            index %= World.Size;
            // tbh idk when this'll be used but fuck it, might as well have it.
            lock (m_ChunkLock)
            {
                if (m_LoadedChunks.ContainsKey(index))
                    m_LoadedChunks[index] = chunk;
                else
                    m_LoadedChunks.Add(index, chunk);
            }
        }

        private bool TryLoad(Vector3I index, out Chunk chunk)
        {
            chunk = null;
            return false;
        }
        
        public void BeginPolling()
        {
            WeltGame.Instance.TaskManager.ExecuteInBackground(() =>
            {
                while (true)
                {
                    
                    // first we get the center chunk of the render area. 
                    Vector3I center;
                    if (Player.Current == null)
                        center = new Vector3I((int)World.Origin.X, 0, (int)World.Origin.Y);
                    else
                        center = World.ChunkAt(Player.Current.Position).Index;
                    if (m_FocalChunk != null && center == m_FocalChunk) continue;
                    Cardinal? moveDirection = null;

                    if (m_FocalChunk + Vector3I.OneX == center) moveDirection = Cardinal.E;
                    if (m_FocalChunk - Vector3I.OneX == center) moveDirection = Cardinal.W;
                    if (m_FocalChunk + Vector3I.OneZ == center) moveDirection = Cardinal.N;
                    if (m_FocalChunk - Vector3I.OneZ == center) moveDirection = Cardinal.S;

                    m_FocalChunk = center;
                    var centerChunk = GetChunk(center);
                    if (!Chunks.Contains(centerChunk)) Chunks.Add(centerChunk);

                    if (!moveDirection.HasValue) DoFullSearch();
                    else DoSearch(moveDirection.Value);
                }
            }, true);
        }

        public void Update()
        {
            // first we get the center chunk of the render area. 
            Vector3I center;
            if (Player.Current == null)
                center = new Vector3I((int)World.Origin.X, 0, (int)World.Origin.Y);
            else
                center = World.ChunkAt(Player.Current.Position).Index;
            if (m_FocalChunk != null && center == m_FocalChunk) return;
            m_FocalChunk = center;
            var centerChunk = GetChunk(center);
            if (!Chunks.Contains(centerChunk)) Chunks.Add(centerChunk);
            var a = 0;
            const int additionCap = 15;
            for (var i = 0; i < Chunks.Count; i++)
            {
                var chunk = Chunks[i];
                if (chunk.Index.DistanceTo(center) > RENDER_DISTANCE + 1) // too far
                {
                    chunk.Clear();
                    Chunks.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (chunk.Index.DistanceTo(center) >= RENDER_DISTANCE) // that sweetspot at the edge
                    continue;
                var cn = chunk.N;
                var cs = chunk.S;
                var cw = chunk.W;
                var ce = chunk.E;
                if (cn == null) cn = GetChunk(chunk.Index + Vector3I.OneZ);
                if (cs == null) cs = GetChunk(chunk.Index - Vector3I.OneZ);
                if (ce == null) ce = GetChunk(chunk.Index + Vector3I.OneX);
                if (cw == null) cw = GetChunk(chunk.Index - Vector3I.OneX);
                if (!Chunks.Contains(cn))
                {
                    Chunks.Add(cn);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                    a++;
                }
                if (!Chunks.Contains(cs))
                {
                    Chunks.Add(cs);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                    a++;
                }
                if (!Chunks.Contains(ce))
                {
                    Chunks.Add(ce);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                    a++;
                }
                if (!Chunks.Contains(cw))
                {
                    Chunks.Add(cw);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                    a++;
                }
                World.Renderer.RebuildChunk(chunk, false);
                //if (a > additionCap) break;
            }
        }

        private void DoGenerate(Vector2 center)
        {
            for (var x = center.X - RENDER_DISTANCE*2; x < center.X + RENDER_DISTANCE*2; ++x)
            {
                for (var y = center.Y - RENDER_DISTANCE*2; y < center.Y + RENDER_DISTANCE*2; ++y)
                {
                    var chunk = GetChunk(new Vector3I((uint)x, 0, (uint)y));
                    World.Renderer.RebuildChunk(chunk);
                    Chunks.Add(chunk);
                }
            }
            Debug.WriteLine("Generated spawn region.");
        }

        private void DoSearch(Cardinal cardinal)
        {
            // this is optimal because it'll only check at most 15 chunks at a time
            
            for (var i = 0; i < Chunks.Count; i++)
            {
                var chunk = Chunks[i];
                if (chunk.Index.DistanceTo(m_FocalChunk) > RENDER_DISTANCE + 5) // too far
                {
                    chunk.Clear();
                    Chunks.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (chunk.Index.DistanceTo(m_FocalChunk) >= RENDER_DISTANCE) // that sweetspot at the edge
                    continue;
                Chunk toAdd = null;
                switch (cardinal)
                {
                    case Cardinal.N:
                        toAdd = chunk.N;
                        if (toAdd == null)
                            toAdd = GetChunk(chunk.Index + Vector3I.OneZ);
                        break;
                    case Cardinal.S:
                        toAdd = chunk.S;
                        if (toAdd == null)
                            toAdd = GetChunk(chunk.Index - Vector3I.OneZ);
                        break;
                    case Cardinal.E:
                        toAdd = chunk.E;
                        if (toAdd == null)
                            toAdd = GetChunk(chunk.Index + Vector3I.OneX);
                        break;
                    case Cardinal.W:
                        toAdd = chunk.W;
                        if (toAdd == null)
                            toAdd = GetChunk(chunk.Index - Vector3I.OneX);
                        break;
                }
                if (!Chunks.Contains(toAdd)) Chunks.Add(toAdd);
            }
        }

        private void DoFullSearch()
        {
            for (var i = 0; i < Chunks.Count; i++)
            {
                var chunk = Chunks[i];
                if (chunk.Index.DistanceTo(m_FocalChunk) > RENDER_DISTANCE + 1) // too far
                {
                    chunk.Clear();
                    Chunks.RemoveAt(i);
                    i--;
                    continue;
                }
                else if (chunk.Index.DistanceTo(m_FocalChunk) >= RENDER_DISTANCE) // that sweetspot at the edge
                    continue;
                var cn = chunk.N;
                var cs = chunk.S;
                var cw = chunk.W;
                var ce = chunk.E;
                if (cn == null) cn = GetChunk(chunk.Index + Vector3I.OneZ);
                if (cs == null) cs = GetChunk(chunk.Index - Vector3I.OneZ);
                if (ce == null) ce = GetChunk(chunk.Index + Vector3I.OneX);
                if (cw == null) cw = GetChunk(chunk.Index - Vector3I.OneX);
                if (!Chunks.Contains(cn))
                {
                    Chunks.Add(cn);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                }
                if (!Chunks.Contains(cs))
                {
                    Chunks.Add(cs);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                }
                if (!Chunks.Contains(ce))
                {
                    Chunks.Add(ce);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                }
                if (!Chunks.Contains(cw))
                {
                    Chunks.Add(cw);
                    m_ToBeBuilt.Enqueue(Chunks.Count - 1);
                }
                World.Renderer.RebuildChunk(chunk, false);
                //if (a > additionCap) break;
            }
        }
    }
}
