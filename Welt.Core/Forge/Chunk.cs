#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;
using System.Linq;
using System.Drawing;

namespace Welt.Core.Forge
{
    public class Chunk : IChunk
    {

        public const int Width = 16, Depth = 16, Height = 128;
        public static int FlattenOffset = Size.Z * Size.Y;
        public static Vector3B Size = new Vector3B(16, 128, 16);
        public static Vector3B Max = new Vector3B(15, 127, 15);

        public Chunk(World world, Vector3I index)
        {
            World = world;
            Blocks = new BlockPalette(Width, Height, Depth);
            HeightMap = new byte[Width * Depth];

            index %= World.Size;
            //ensure world is set directly in here to have access to N S E W as soon as possible
            World.ChunkManager.SetChunk(index, this);
            
            //Array.Clear(Blocks, 0, Blocks.Length);
            Index = index;
            Position = new Vector3I(index.X * Size.X, index.Y * Size.Y, index.Z * Size.Z);
            BoundingBox = new BoundingBox(new Vector3(Position.X, Position.Y, Position.Z),
                new Vector3(Position.X + Size.X, Position.Y + Size.Y, Position.Z + Size.Z));
        }

        public byte[] GetData()
        {
            // for now, we'll return just the block palette? idk
            var data = Blocks.ToByteArray();

            return data;
        }

        public void Fill(byte[] data)
        {
            Blocks = BlockPalette.FromByteArray(Width, Height, Depth, data);
        }
        
        public byte[] HeightMap { get; }
        public bool IsModified { get; set; }
        public bool IsTerrainPopulated { get; set; }
        public bool IsLit { get; set; }
        public byte MaxHeight => HeightMap.Max();

        public World World { get; protected set; }

        public BoundingBox BoundingBox { get; protected set; }

        public Vector3I Index { get; protected set; }

        public Vector3I Position { get; protected set; }

        public Vector3B HighestSolidBlock = new Vector3B(0, 0, 0);
        //highestNoneBlock starts at 0 so it will be adjusted. if you start at highest it will never be adjusted ! 

        public Vector3B LowestNoneBlock = new Vector3B(0, Size.Y, 0);


        public BlockPalette Blocks;

        public static int GetIndex(int x, int y, int z)
        {
            return x * FlattenOffset + z * Size.Y + y;
        }

        #region SetBlock

        public void SetBlockId(byte x, byte y, byte z, ushort id)
        {
            SetBlock(x, y, z, new Block(id));
        }

        public void SetBlock(byte x, byte y, byte z, Block b)
        {
            if (b.Id == BlockType.WATER)
            {
                if (LowestNoneBlock.Y > y)
                {
                    LowestNoneBlock = new Vector3B(x, y, z);
                }
            }

            if (b.Id == BlockType.NONE)
            {
                if (LowestNoneBlock.Y > y)
                {
                    LowestNoneBlock = new Vector3B(x, y, z);
                }
            }
            else if (HighestSolidBlock.Y < y)
            {
                HighestSolidBlock = new Vector3B(x, y, z);
            }

            if (HeightMap[x * Size.X + z] < y)
                HeightMap[x * Size.X + z] = y;
            
            Blocks[x, y, z] = b;
        }

        #endregion

        public static bool OutOfBounds(byte x, byte y, byte z)
        {
            return x >= Size.X || y >= Size.Y || z >= Size.Z;
        }

        #region GetBlock

        public byte GetHeight(byte x, byte z)
        {
            return HeightMap[x * Size.X + z];
        }

        public void SetId(byte x, byte y, byte z, ushort id)
        {
            SetBlock(x, y, z, new Block(id));
        }

        public Block GetBlock(byte x, byte y, byte z)
        {
            return GetBlock(relx: x, rely: y, relz: z);
        }


        public Block GetBlock(uint relx, uint rely, uint relz)
        {
            return GetBlock((int)relx, (int)rely, (int)relz);
        }

        public Block GetBlock(int relx, int rely, int relz)
        {
            if (rely < 0 || rely > Max.Y)
            {
                //infinite Y : y bounds currently set as rock for never rendering those y bounds
                return new Block(BlockType.NONE);
            }

            //handle the normal simple case
            if (relx >= 0 && relz >= 0 && relx < Size.X && relz < Size.Z)
            {
                var b = Blocks[relx, rely, relz];
                return b;
            }

            //handle all special cases

            int x = relx, z = relz;
            Chunk nChunk = null;

            //TODO chunk relative GetBlock could even handle more tha just -1 but -2 -3 ... -15 

            if (relx < 0) x = Max.X - relx;
            if (relz < 0) z = Max.Z - relz;
            if (relx > 15) x = relx - Max.X;
            if (relz > 15) z = relz - Max.Z;


            if (x != relx && x == 0)
                if (z != relz && z == 0)
                    nChunk = Nw;
                else if (z != relz && z == 15)
                    nChunk = Sw;
                else
                    nChunk = W;
            else if (x != relx && x == 15)
                if (z != relz && z == 0)
                    nChunk = Ne;
                else if (z != relz && z == 15)
                    nChunk = Se;
                else
                    nChunk = E;
            else if (z != relz && z == 0)
                nChunk = N;
            else if (z != relz && z == 15)
                nChunk = S;

            if (nChunk == null)
            {
                //happens at current world bounds
                return new Block(BlockType.NONE);
            }
            var block = nChunk.Blocks[x, rely, z];
            return block;
        }

        #endregion

        private Chunk _mN, _mS, _mE, _mW, _mNe, _mNw, _mSe, _mSw;

        public Chunk N
        {
            get
            {
                if (_mN == null) _mN = World.ChunkManager.GetChunk(Index + Vector3I.OneZ, false);
                if (_mN != null) _mN._mS = this;
                return _mN;
            }
        }

        public Chunk S
        {
            get
            {
                if (_mS == null) _mS = World.ChunkManager.GetChunk(Index - Vector3I.OneZ, false);
                if (_mS != null) _mS._mN = this;
                return _mS;
            }
        }

        public Chunk E
        {
            get
            {
                if (_mE == null) _mE = World.ChunkManager.GetChunk(Index - Vector3I.OneX, false);
                if (_mE != null) _mE._mW = this;
                return _mE;
            }
        }

        public Chunk W
        {
            get
            {
                if (_mW == null) _mW = World.ChunkManager.GetChunk(Index + Vector3I.OneX, false);
                if (_mW != null) _mW._mE = this;
                return _mW;
            }
        }

        public Chunk Nw => _mNw ?? (_mNw = World.ChunkManager.GetChunk(Index.X + 1, Index.Y, Index.Z + 1, false));

        public Chunk Ne => _mNe ?? (_mNe = World.ChunkManager.GetChunk(Index.X - 1, Index.Y, Index.Z + 1, false));

        public Chunk Sw => _mSw ?? (_mSw = World.ChunkManager.GetChunk(Index.X + 1, Index.Y, Index.Z - 1, false));

        public Chunk Se => _mSe ?? (_mSe = World.ChunkManager.GetChunk(Index.X - 1, Index.Y, Index.Z - 1, false));

        public Chunk GetNeighbour(Cardinal c)
        {
            switch (c)
            {
                case Cardinal.N:
                    return N;
                case Cardinal.S:
                    return S;
                case Cardinal.E:
                    return E;
                case Cardinal.W:
                    return W;
                case Cardinal.Se:
                    return Se;
                case Cardinal.Sw:
                    return Sw;
                case Cardinal.Ne:
                    return Ne;
                case Cardinal.Nw:
                    return Nw;
            }
            throw new NotImplementedException();
        }


        public event EventHandler<BlockChangeEventArgs> BlockAdded;
        public event EventHandler<BlockChangeEventArgs> BlockRemoved;
    }
}