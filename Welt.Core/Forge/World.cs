#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Runtime.CompilerServices;
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
        public IWorldSystem System { get; }
        public int SystemIndex { get; }

        protected readonly ChunkManager Manager;

        public World(string name, IForgeGenerator gen)
        {
            Name = name;
            var random = new FastMath.LongRandom();
            Seed = random.Next(int.MaxValue)*name.GetHashCode();
            Size = 32;
            Generator = gen;
            Manager = new ChunkManager(this);

            System = null;
            SystemIndex = 0;
        }

        public World(string name, long seed, IForgeGenerator gen)
        {
            Name = name;
            Seed = seed;
            Size = 32;
            Generator = gen;
            Manager = new ChunkManager(this);

            System = null;
            SystemIndex = 0;
        }

        /// <summary>
        ///     Creates the chunk and adds it to the chunk manager.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public Chunk CreateChunk(uint x, uint z)
        {
            var chunk = new Chunk(this, x, z);
            Generator.GenerateChunk(this, chunk);
            _SetChunk(x, z, chunk, ChunkChangedEventArgs.ChunkChangedAction.Created);
            return chunk;
        }

        /// <summary>
        ///     Creates the chunk without adding it, keeping it solely on the stack.
        /// </summary>
        /// <returns></returns>
        public Chunk CreateChunkInMemory(uint x, uint z)
        {
            var chunk = new Chunk(this, x, z);
            Generator.GenerateChunk(this, chunk);
            ChunkChanged?.Invoke(this, new ChunkChangedEventArgs(x, z, ChunkChangedEventArgs.ChunkChangedAction.Created));
            return chunk;
        }

        public Chunk CreateChunkWithoutGeneration(uint x, uint z)
        {
            var chunk = new Chunk(this, x, z);
            return chunk;
        }

        public virtual IChunk GetChunk(uint x, uint z)
        {
            var chunk = Manager.GetChunk(x, z) ?? CreateChunk(x, z);
            return chunk;
        }

        public virtual void SetChunk(uint x, uint z, IChunk value)
        {
            _SetChunk(x, z, value, ChunkChangedEventArgs.ChunkChangedAction.Adjusted);
        }

        private void _SetChunk(uint x, uint z, IChunk value, ChunkChangedEventArgs.ChunkChangedAction action)
        {
            Manager.SetChunk(x, z, (Chunk) value);
            ChunkChanged?.Invoke(this, new ChunkChangedEventArgs(x, z, action));
        }

        public virtual void RemoveChunk(uint x, uint z)
        {
            Manager.RemoveChunk(x, z);
            ChunkChanged?.Invoke(this, new ChunkChangedEventArgs(x, z, ChunkChangedEventArgs.ChunkChangedAction.Destroyed));
        }

        public virtual Block GetBlock(uint x, uint y, uint z)
        {
            if (x >= Size*Chunk.Width || z >= Size*Chunk.Depth || y >= Chunk.Height)
                throw new ArgumentOutOfRangeException();
            var chunk = GetChunk(x/Chunk.Width, z/Chunk.Depth);
            return chunk.GetBlock((int) (x%Chunk.Width), (int) y, (int) (z%Chunk.Depth));
        }

        public virtual void SetBlock(uint x, uint y, uint z, Block value)
        {
            if (x >= Size*Chunk.Width || z >= Size*Chunk.Depth || y >= Chunk.Height)
                throw new ArgumentOutOfRangeException();
            var chunk = GetChunk(x/Chunk.Width, z/Chunk.Depth);
            chunk.SetBlock((int) (x%Chunk.Width), (int) y, (int) (z%Chunk.Depth), value);
            BlockChanged?.Invoke(this, new BlockChangedEventArgs(x, y, z));
        }

        public event EventHandler<BlockChangedEventArgs> BlockChanged;
        public event EventHandler<ChunkChangedEventArgs> ChunkChanged;
    }
}