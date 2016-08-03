#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.API.Forge;
using static Welt.Core.FastMath;

namespace Welt.Core.Forge
{
    public class Chunk : IChunk
    {
        public const int Width = 16, Depth = 16, Height = 256;

        public Chunk(IWorld world, uint x, uint z)
        {
            HeightMap = new byte[Width, Depth];
            Floors = new IFloor[16];
            for (byte i = 0; i < Height/16; i++)
                Floors[i] = new Floor(i);
            X = x;
            Z = z;
            
        }

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
        public byte[,] HeightMap { get; }
        public IFloor[] Floors { get; }
        public bool IsModified { get; set; }
        public bool IsGenerated { get; set; }

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

        public Block GetBlock(int x, int y, int z)
        {
            if (!WithinBounds(0, Height, y)) return new Block();

            if (WithinBounds(0, Width, x) && WithinBounds(0, Depth, z))
                return GetFloor(y).Blocks.GetBlock((uint) x, (uint) y, (uint) z);

            if (x < 0 && z >= 0)
            {
                return W.GetBlock(Width + x, y, z);
            }
            if (x >= 0 && z < 0)
            {
                return E.GetBlock(x, y, z + Depth);
            }
            if (x < 0 && z < 0)
            {
                return S.GetBlock(Width + x, y, z + Depth);
            }
            if (x >= Width && z >= Depth)
            {
                return N.GetBlock(x - Width, y, z - Depth);
            }

            return new Block();
        }

        public void SetBlock(int x, int y, int z, Block value)
        {
            if (!WithinBounds(0, Height, y)) return;
            if (WithinBounds(0, Width, x) && WithinBounds(0, Depth, z))
                GetFloor(y).Blocks.SetBlock((uint) x, (uint) y, (uint) z, value);

            if (x < 0 && z >= 0)
            {
                W.SetBlock(Width + x, y, z, value);
            }
            else if (x >= 0 && z < 0)
            {
                E.SetBlock(x, y, z + Depth, value);
            }
            else if (x < 0 && z < 0)
            {
                S.SetBlock(Width + x, y, z + Depth, value);
            }
            else if (x >= Width && z >= Depth)
            {
                N.SetBlock(x - Width, y, z - Depth, value);
            }
        }

        protected IFloor GetFloor(int y)
        {
            return y <= 0 ? Floors[0] : Floors[y/16];
        }

        public event EventHandler BlockAdded;
        public event EventHandler BlockRemoved;
    }
}