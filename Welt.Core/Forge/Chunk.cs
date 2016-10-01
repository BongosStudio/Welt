#region Copyright

// COPYRIGHT 2016 JUSTIN COX (CONJI)

#endregion Copyright

using System;
using Welt.API.Forge;
using static Welt.Core.FastMath;

namespace Welt.Core.Forge
{
    public class Chunk : IChunk
    {
        public const int Width = 16, Depth = 16;

        public Chunk(IWorld world, uint x, uint z)
        {
            //Height = world.Height;
            Height = 256;
            HeightMap = new int[Width*Depth];
            X = x;
            Z = z;
            Blocks = new BlockPalette(this);
        }

        public int Height { get; }

        public uint X { get; set; }
        public uint Z { get; set; }
        public virtual IChunk N { get; protected set; }
        public virtual IChunk E { get; protected set; }
        public virtual IChunk S { get; protected set; }
        public virtual IChunk W { get; protected set; }
        public virtual IChunk Ne { get; protected set; }
        public virtual IChunk Nw { get; protected set; }
        public virtual IChunk Se { get; protected set; }
        public virtual IChunk Sw { get; protected set; }
        public int[] HeightMap { get; }
        public bool IsModified { get; set; }
        public bool IsGenerated { get; set; }

        protected BlockPalette Blocks;

        private int _lowestAirBlock;

        public virtual void Initialize(IWorld world)
        {
            world.SetChunk(X, Z, this);
            N = world.GetChunk(X + 1, Z);
            E = world.GetChunk(X, Z + 1);
            S = world.GetChunk(X - 1, Z);
            W = world.GetChunk(X, Z - 1);
            Ne = world.GetChunk(X + 1, Z + 1);
            Nw = world.GetChunk(X + 1, Z - 1);
            Se = world.GetChunk(X - 1, Z + 1);
            Sw = world.GetChunk(X - 1, Z - 1);
            IsModified = true;
        }

        public (ushort Id, byte Metadata) GetBlock(int x, int y, int z)
        {
            if (!WithinBounds(0, Height, y))
                return (0, 0);

            if (WithinBounds(0, Width, x) && WithinBounds(0, Depth, z))
                return Blocks.GetBlock((uint) x, (uint) y, (uint) z);

            //if (x < 0 && z >= 0)
            //{
            //    return W.GetBlock(Width + x, y, z);
            //}
            //if (x >= 0 && z < 0)
            //{
            //    return E.GetBlock(x, y, z + Depth);
            //}
            //if (x < 0 && z < 0)
            //{
            //    return S.GetBlock(Width + x, y, z + Depth);
            //}
            //if (x >= Width && z >= Depth)
            //{
            //    return N.GetBlock(x - Width, y, z - Depth);
            //}

            return (0, 0);
        }

        public void SetBlock(int x, int y, int z, ushort id, byte metadata)
        {
            //Blocks.SetBlock((uint) x, (uint) y, (uint) z, id, metadata);
            if (!WithinBounds(0, Height, y))
                return;
            if (WithinBounds(0, Width, x) && WithinBounds(0, Depth, z))
            {
                Blocks.SetBlock((uint) x, (uint) y, (uint) z, id, metadata);
                if (id == 0)
                {
                    if (_lowestAirBlock > y)
                        _lowestAirBlock = y;
                    BlockRemoved?.Invoke(this, new BlockChangedEventArgs(x, y, z));
                }
                else
                {
                    BlockAdded?.Invoke(this, new BlockChangedEventArgs(x, y, z));
                }
                SetHeight(x, z);
                return;
            }

            if (x < 0 && z >= 0)
            {
                W.SetBlock(Width + x, y, z, id, metadata);
                return;
            }
            if (x >= 0 && z < 0)
            {
                E.SetBlock(x, y, z + Depth, id, metadata);
                return;
            }
            if (x < 0 && z < 0)
            {
                S.SetBlock(Width + x, y, z + Depth, id, metadata);
                return;
            }
            if (x >= Width && z >= Depth)
            {
                N.SetBlock(x - Width, y, z - Depth, id, metadata);
                return;
            }
        }

        public int GetHeight(int x, int z)
        {
            return HeightMap[x*Width + z];
        }

        public int GetLowestNoneBlock()
        {
            return _lowestAirBlock;
        }

        private void SetHeight(int x, int z)
        {
            //HeightMap[x*Width + z] = value;
            for (var i = Height; i >= 0; --i)
            {
                if (GetBlock(x, i, z).Id == 0)
                    continue;
                HeightMap[x*Width + z] = i;
            }
        }

        public event EventHandler<BlockChangedEventArgs> BlockAdded;

        public event EventHandler<BlockChangedEventArgs> BlockRemoved;
    }
}