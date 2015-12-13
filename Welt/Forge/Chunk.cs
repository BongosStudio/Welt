#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Blocks;
using Welt.Models;
using Welt.Types;

#endregion

namespace Welt.Forge
{
    public class Chunk
    {
        private const byte MAX_SUN_VALUE = 16;

        public Chunk(World world, Vector3I index)
        {
            this.world = world;
            Blocks = new Block[SIZE.X*SIZE.Z*SIZE.Y];
            vertexList = new List<VertexPositionTextureLight>();
            watervertexList = new List<VertexPositionTextureLight>();
            indexList = new List<short>();
            waterindexList = new List<short>();

            Assign(index);
            /* world.viewableChunks[index.X, index.Z] = this;
             dirty = true;
             this.Index = index;
             this.Position = new Vector3i(index.X * SIZE.X, index.Y * SIZE.Y, index.Z * SIZE.Z);
             this._boundingBox = new BoundingBox(new Vector3(Position.X, Position.Y, Position.Z), new Vector3(Position.X + SIZE.X, Position.Y + SIZE.Y, Position.Z + SIZE.Z));
             */
        }

        public ChunkState State { get; set; }
        public BoundingBox BoundingBox { get; private set; }

        public void Assign(Vector3I index)
        {
            //ensure world is set directly in here to have access to N S E W as soon as possible

            world.Chunks.Remove(Index.X, Index.Z);
            world.Chunks[index.X, index.Z] = this;

            dirty = true;
            //Array.Clear(Blocks, 0, Blocks.Length);
            Index = index;
            Position = new Vector3I(index.X*SIZE.X, index.Y*SIZE.Y, index.Z*SIZE.Z);
            BoundingBox = new BoundingBox(new Vector3(Position.X, Position.Y, Position.Z),
                new Vector3(Position.X + SIZE.X, Position.Y + SIZE.Y, Position.Z + SIZE.Z));

            //TODO next optimization step would be reusing the vertexbuffer
            //vertexList.Clear(); 
            //indexList.Clear();
        }

        public void Clear()
        {
            vertexList.Clear();
            indexList.Clear();

            watervertexList.Clear();
            waterindexList.Clear();

            VertexCount = 0;
            waterVertexCount = 0;
        }

        #region SetBlock

        public void setBlock(byte x, byte y, byte z, Block b)
        {
            if (b.Type == BlockType.Water)
            {
                if (lowestNoneBlock.Y > y)
                {
                    lowestNoneBlock = new Vector3B(x, y, z);
                }
            }

            if (b.Type == BlockType.None)
            {
                if (lowestNoneBlock.Y > y)
                {
                    lowestNoneBlock = new Vector3B(x, y, z);
                }
            }
            else if (highestSolidBlock.Y < y)
            {
                highestSolidBlock = new Vector3B(x, y, z);
            }

            //comment this line : you should have nothing on screen, else you ve been setting blocks directly in array !
            Blocks[x*FlattenOffset + z*SIZE.Y + y] = b;
            dirty = true;
        }

        #endregion

        public bool outOfBounds(byte x, byte y, byte z)
        {
            return (x < 0 || x >= SIZE.X || y < 0 || y >= SIZE.Y || z < 0 || z >= SIZE.Z);
        }

        #region GetBlock

        public Block BlockAt(int relx, int rely, int relz)
        {
            if (rely < 0 || rely > MAX.Y)
            {
                //infinite Y : y bounds currently set as rock for never rendering those y bounds
                return new Block(BlockType.Rock);
            }

            //handle the normal simple case
            if (relx >= 0 && relz >= 0 && relx < SIZE.X && relz < SIZE.Z)
            {
                var b = Blocks[relx*FlattenOffset + relz*SIZE.Y + rely];
                return b;
            }

            //handle all special cases

            int x = relx, z = relz;
            Chunk nChunk = null;

            //TODO chunk relative GetBlock could even handle more tha just -1 but -2 -3 ... -15 

            if (relx < 0) x = MAX.X;
            if (relz < 0) z = MAX.Z;
            if (relx > 15) x = 0;
            if (relz > 15) z = 0;


            if (x != relx && x == 0)
                if (z != relz && z == 0)
                    nChunk = NW;
                else if (z != relz && z == 15)
                    nChunk = SW;
                else
                    nChunk = W;
            else if (x != relx && x == 15)
                if (z != relz && z == 0)
                    nChunk = NE;
                else if (z != relz && z == 15)
                    nChunk = SE;
                else
                    nChunk = E;
            else if (z != relz && z == 0)
                nChunk = N;
            else if (z != relz && z == 15)
                nChunk = S;

            if (nChunk == null)
            {
                //happens at current world bounds
                return new Block(BlockType.Rock);
            }
            var block = nChunk.Blocks[x*FlattenOffset + z*SIZE.Y + rely];
            return block;
        }

        #endregion

        public override string ToString()
        {
            return ("chunk at index " + Index);
        }

        #region main as unit test for neighbours

        private static void Main(string[] args)
        {
            var world = new World();

            uint n = 4, s = 6, w = 4, e = 6;

            var cw = new Chunk(world, new Vector3I(w, 5, 5));
            var c = new Chunk(world, new Vector3I(5, 5, 5));
            var ce = new Chunk(world, new Vector3I(e, 5, 5));

            var cn = new Chunk(world, new Vector3I(5, 5, n));
            var cs = new Chunk(world, new Vector3I(5, 5, s));
            var cne = new Chunk(world, new Vector3I(e, 5, n));
            var cnw = new Chunk(world, new Vector3I(w, 5, n));
            var cse = new Chunk(world, new Vector3I(e, 5, s));
            var csw = new Chunk(world, new Vector3I(w, 5, s));


            c.setBlock(0, 0, 0, new Block(BlockType.Dirt));
            cw.setBlock(15, 0, 0, new Block(BlockType.Grass));

            var w15 = c.BlockAt(-1, 0, 0);
            Debug.Assert(w15.Type == BlockType.Grass);

            ce.setBlock(0, 0, 0, new Block(BlockType.Tree));
            var e0 = c.BlockAt(16, 0, 0);
            Debug.Assert(e0.Type == BlockType.Tree);

            csw.setBlock(15, 0, 0, new Block(BlockType.Lava));
            var swcorner = c.BlockAt(-1, 0, 16);
            Debug.Assert(swcorner.Type == BlockType.Lava);

            cne.setBlock(0, 0, 15, new Block(BlockType.Leaves));
            var necorner = c.BlockAt(16, 0, -1);
            Debug.Assert(necorner.Type == BlockType.Leaves);
        }

        #endregion

        #region Fields

        private Chunk _N, _S, _E, _W, _NE, _NW, _SE, _SW;
            //TODO infinite y would require Top , Bottom, maybe vertical diagonals

        public static Vector3B SIZE = new Vector3B(16, 128, 16);
        public static Vector3B MAX = new Vector3B(15, 127, 15);

        public VertexBuffer VertexBuffer;
        public VertexBuffer waterVertexBuffer;

        public IndexBuffer IndexBuffer;
        public IndexBuffer waterIndexBuffer;

        public List<short> indexList;
        public List<short> waterindexList;

        public List<VertexPositionTextureLight> vertexList;
        public List<VertexPositionTextureLight> watervertexList;

        public short VertexCount;
        public short waterVertexCount;

        /// <summary>
        /// Contains blocks as a flattened array.
        /// </summary>
        public Block[] Blocks;

        /* 
        For accessing array for x,z,y coordianate use the pattern: Blocks[x * Chunk.FlattenOffset + z * Chunk.SIZE.Y + y]
        For allowing sequental access on blocks using iterations, the blocks are stored as [x,z,y]. So basically iterate x first, z then and y last.
        Consider the following pattern;
        for (int x = 0; x < Chunk.WidthInBlocks; x++)
        {
            for (int z = 0; z < Chunk.LenghtInBlocks; z++)
            {
                int offset = x * Chunk.FlattenOffset + z * Chunk.HeightInBlocks; // we don't want this x-z value to be calculated each in in y-loop!
                for (int y = 0; y < Chunk.HeightInBlocks; y++)
                {
                    var block=Blocks[offset + y].Type 
        */

        /// <summary>
        /// Used when accessing flatten blocks array.
        /// </summary>
        public static int FlattenOffset = SIZE.Z*SIZE.Y;

        public Vector3I Position;
        public Vector3I Index;

        public bool dirty;
        //public bool visible;
        //public bool generated;
        //public bool built;

        public bool broken;

        public readonly World world;

        public Vector3B highestSolidBlock = new Vector3B(0, 0, 0);
        //highestNoneBlock starts at 0 so it will be adjusted. if you start at highest it will never be adjusted ! 

        public Vector3B lowestNoneBlock = new Vector3B(0, SIZE.Y, 0);

        #endregion

        #region N S E W NE NW SE SW Neighbours accessors

        //this neighbours check can not be done in constructor, there would be some holes => it has to be done at access time 
        //seems there is no mem leak so no need for weakreferences
        public Chunk N
        {
            get
            {
                if (_N == null) _N = world.Chunks[Index.X, Index.Z + 1];
                if (_N != null) _N._S = this;
                return _N;
            }
        }

        public Chunk S
        {
            get
            {
                if (_S == null) _S = world.Chunks[Index.X, Index.Z - 1];
                if (_S != null) _S._N = this;
                return _S;
            }
        }

        public Chunk E
        {
            get
            {
                if (_E == null) _E = world.Chunks[Index.X - 1, Index.Z];
                if (_E != null) _E._W = this;
                return _E;
            }
        }

        public Chunk W
        {
            get
            {
                if (_W == null) _W = world.Chunks[Index.X + 1, Index.Z];
                if (_W != null) _W._E = this;
                return _W;
            }
        }

        public Chunk NW
        {
            get { return _NW != null ? _NW : _NW = world.Chunks[Index.X + 1, Index.Z + 1]; }
        }

        public Chunk NE
        {
            get { return _NE != null ? _NE : _NE = world.Chunks[Index.X - 1, Index.Z + 1]; }
        }

        public Chunk SW
        {
            get { return _SW != null ? _SW : _SW = world.Chunks[Index.X + 1, Index.Z - 1]; }
        }

        public Chunk SE
        {
            get { return _SE != null ? _SE : _SE = world.Chunks[Index.X - 1, Index.Z - 1]; }
        }

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
                case Cardinal.SE:
                    return SE;
                case Cardinal.SW:
                    return SW;
                case Cardinal.NE:
                    return NE;
                case Cardinal.NW:
                    return NW;
            }
            throw new NotImplementedException();
        }

        #endregion
    }
}