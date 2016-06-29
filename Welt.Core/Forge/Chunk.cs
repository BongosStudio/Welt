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
        public const int WIDTH = 16, DEPTH = 16, HEIGHT = 256;

        public Chunk(IWorld world, uint x, uint z)
        {
            HeightMap = new byte[WIDTH, DEPTH];
            Floors = new IFloor[16];
            for (byte i = 0; i < HEIGHT/16; i++)
                Floors[i] = new Floor(i);
            X = x;
            Z = z;
            world.SetChunk(x, z, this);
            N = world.GetChunk(x + 1, z);
            E = world.GetChunk(x, z + 1);
            S = world.GetChunk(x - 1, z);
            W = world.GetChunk(x, z - 1);
            IsModified = true;
        }

        public uint X { get; }
        public uint Z { get; }
        public IChunk N { get; }
        public IChunk E { get; }
        public IChunk S { get; }
        public IChunk W { get; }
        public IChunk NE { get; }
        public IChunk NW { get; }
        public IChunk SE { get; }
        public IChunk SW { get; }
        public byte[,] HeightMap { get; }
        public IFloor[] Floors { get; }
        public bool IsModified { get; set; }

        public Block GetBlock(int x, int y, int z)
        {
            if (!WithinBounds(0, HEIGHT, y)) return new Block();

            if (WithinBounds(0, WIDTH, x) && WithinBounds(0, DEPTH, z))
                return GetFloor(y).Blocks.GetBlock((uint) x, (uint) y, (uint) z);

            if (x < 0 && z >= 0)
            {
                return W.GetBlock(WIDTH + x, y, z);
            }
            if (x >= 0 && z < 0)
            {
                return E.GetBlock(x, y, z + DEPTH);
            }
            if (x < 0 && z < 0)
            {
                return S.GetBlock(WIDTH + x, y, z + DEPTH);
            }
            if (x >= WIDTH && z >= DEPTH)
            {
                return N.GetBlock(x - WIDTH, y, z - DEPTH);
            }

            return new Block();
        }

        public void SetBlock(int x, int y, int z, Block value)
        {
            if (!WithinBounds(0, HEIGHT, y)) return;
            if (WithinBounds(0, WIDTH, x) && WithinBounds(0, DEPTH, z))
                GetFloor(y).Blocks.SetBlock((uint) x, (uint) y, (uint) z, value);

            if (x < 0 && z >= 0)
            {
                W.SetBlock(WIDTH + x, y, z, value);
            }
            else if (x >= 0 && z < 0)
            {
                E.SetBlock(x, y, z + DEPTH, value);
            }
            else if (x < 0 && z < 0)
            {
                S.SetBlock(WIDTH + x, y, z + DEPTH, value);
            }
            else if (x >= WIDTH && z >= DEPTH)
            {
                N.SetBlock(x - WIDTH, y, z - DEPTH, value);
            }
        }
        
        private IFloor GetFloor(int y)
        {
            return y <= 0 ? Floors[0] : Floors[y/16];
        }

        public event EventHandler BlockAdded;
        public event EventHandler BlockRemoved;
    }
}