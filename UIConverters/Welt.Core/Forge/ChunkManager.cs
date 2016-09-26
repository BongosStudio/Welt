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
        private readonly Chunk[] _chunks;
        private readonly World _world;

        public ChunkManager(World world)
        {
            _chunks = new Chunk[world.Size*world.Size];
            _world = world;
        }

        public Chunk GetChunk(uint x, uint z, bool genIfNotFound = true)
        {
            if (x >= _world.Size || z >= _world.Size) return null;
            if (_chunks.Length <= x || _chunks.Length <= z) throw new IndexOutOfRangeException();

            if (_chunks[x*_world.Size + z]?.X == x && _chunks[x*_world.Size + z]?.Z == z)
                return _chunks[x*_world.Size + z];

            var chunk = new Chunk(_world, x, z);
            if (genIfNotFound) _world.Generator.GenerateChunk(_world, chunk);
            _chunks[x*_world.Size + z] = chunk;
            return chunk;
        }

        public Chunk[] GetChunks()
        {
            return _chunks;
        }

        public void SetChunk(uint x, uint z, Chunk chunk)
        {
            if (x >= _world.Size || z >= _world.Size) return;
            if (_chunks.Length <= x || _chunks.Length <= z) throw new IndexOutOfRangeException();
            _chunks[x* _world.Size + z] = chunk;
            // TODO: hook into persistence to check if chunk is dirty
        }

        public void RemoveChunk(uint x, uint z)
        {
            if (_chunks.Length <= z || _chunks.Length <= z) throw new IndexOutOfRangeException();
            _chunks[x*_world.Size + z] = null;
        }
    }
}