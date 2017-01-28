#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using System;
using System.Collections.Concurrent;
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
        public World World { get; }
        public IChunkPersistence Persistence { get; }
        public IEnumerable<Chunk> Chunks => m_LoadedChunks.Values;
        
        private ConcurrentDictionary<Vector3I, Chunk> m_LoadedChunks;
        private object m_ChunkLock = new object();

        public ChunkManager(IChunkPersistence persistence, World world)
        {
            World = world;
            Persistence = persistence;
            m_LoadedChunks = new ConcurrentDictionary<Vector3I, Chunk>();
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
                    m_LoadedChunks.TryAdd(index, chunk);
            }
        }

        public void RemoveChunk(Chunk chunk)
        {
            var index = chunk.Index % World.Size;
            if (!m_LoadedChunks.ContainsKey(index)) return;
            m_LoadedChunks.TryRemove(index, out chunk);
            chunk.Clear();
        }

        private bool TryLoad(Vector3I index, out Chunk chunk)
        {
            chunk = null;
            return false;
        }
    }
}
