#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.API.Forge;
using Welt.API.Forge.Generators;

namespace Welt.Core.Forge
{
    public class World : IWorld
    {
        public string Name { get; }
        public long Seed { get; }
        public int Size { get; }
        public IForgeGenerator Generator { get; }

        private readonly ChunkManager _manager;

        public World(string name, IForgeGenerator gen)
        {
            Name = name;
            var random = new FastMath.LongRandom();
            Seed = random.Next(int.MaxValue)*name.GetHashCode();
            Size = 32;
            Generator = gen;
            _manager = new ChunkManager(this);
        }

        public World(string name, long seed, IForgeGenerator gen)
        {
            Name = name;
            Seed = seed;
            Size = 32;
            Generator = gen;
            _manager = new ChunkManager(this);
        }

        /// <summary>
        ///     Creates the chunk and adds it to the chunk manager.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Chunk CreateChunk(uint x, uint z)
        {
            var chunk = Generator.GenerateChunk(this, x, z) as Chunk;
            SetChunk(x, z, chunk);
            return chunk;
        }

        /// <summary>
        ///     Creates the chunk without adding it, keeping it solely on the stack.
        /// </summary>
        /// <returns></returns>
        public Chunk CreateChunkInMemory(uint x, uint z)
        {
            return Generator.GenerateChunk(this, x, z) as Chunk;
        }

        public IChunk GetChunk(uint x, uint z)
        {
            return _manager.GetChunk(x, z);
        }

        public void SetChunk(uint x, uint z, IChunk value)
        {
            _manager.SetChunk(x, z, (Chunk) value);
        }

        public void RemoveChunk(uint x, uint z)
        {
            _manager.RemoveChunk(x, z);
        }

        public Block GetBlock(uint x, uint y, uint z)
        {
            if (x >= Size*Chunk.WIDTH || z >= Size*Chunk.DEPTH || y >= Chunk.HEIGHT)
                throw new ArgumentOutOfRangeException();
            var chunk = GetChunk(x/Chunk.WIDTH, z/Chunk.DEPTH);
            return chunk.GetBlock((int) (x%Chunk.WIDTH), (int) y, (int) (z%Chunk.DEPTH));
        }

        public void SetBlock(uint x, uint y, uint z, Block value)
        {
            if (x >= Size * Chunk.WIDTH || z >= Size * Chunk.DEPTH || y >= Chunk.HEIGHT)
                throw new ArgumentOutOfRangeException();
            var chunk = GetChunk(x / Chunk.WIDTH, z / Chunk.DEPTH);
            chunk.SetBlock((int) (x%Chunk.WIDTH), (int) y, (int) (z%Chunk.DEPTH), value);
        }
    }
}