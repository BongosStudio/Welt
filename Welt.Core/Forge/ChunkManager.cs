#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.API.Persistence;

namespace Welt.Core.Forge
{
    public class ChunkManager
    {
        private readonly IChunkPersistence _persistence;
        private readonly Chunk[,] _chunks;
        private readonly World _world;

        public ChunkManager(World world)
        {
            _chunks = new Chunk[world.Size, world.Size];
            _world = world;
        }

        public Chunk GetChunk(uint x, uint z)
        {
            if (_chunks.Length <= x || _chunks.Length <= z) throw new IndexOutOfRangeException();
            return _chunks[x, z] ?? (_chunks[x, z] = _world.CreateChunkInMemory(x, z));
        }

        public void SetChunk(uint x, uint z, Chunk chunk)
        {
            if (_chunks.Length <= x || _chunks.Length <= z) throw new IndexOutOfRangeException();
            _chunks[x, z] = chunk;
            // TODO: hook into persistence to check if chunk is dirty
        }

        public void RemoveChunk(uint x, uint z)
        {
            if (_chunks.Length <= z || _chunks.Length <= z) throw new IndexOutOfRangeException();
            _chunks[x, z] = null;
        }
    }
}