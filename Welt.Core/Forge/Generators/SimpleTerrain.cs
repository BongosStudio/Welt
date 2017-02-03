#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using Welt.API.Forge;

#endregion

namespace Welt.Core.Forge.Generators
{
    internal class SimpleTerrain : IChunkGenerator
    {
        #region build

        public virtual void Generate(IWorld world, IChunk chunk)
        {
            if (R == null) R = new Random(world.Seed);
            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                var worldX = chunk.Position.X + x + (uint) world.Seed;

                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var worldZ = chunk.Position.Z + z;
                    GenerateTerrain(world, chunk, x, z, worldX, worldZ);
                }
            }
        }

        #endregion

        #region generateTerrain

        protected virtual void GenerateTerrain(IWorld world, IChunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldY)
        {
            // The lower ground level is at least this high.
            var minimumGroundheight = Chunk.Size.Y/4;
            var minimumGroundDepth = (int) (Chunk.Size.Y*0.75f);

            var octave1 = PerlinSimplexNoise.Noise(worldX*0.0001f, worldY*0.0001f)*0.5f;
            var octave2 = PerlinSimplexNoise.Noise(worldX*0.0005f, worldY*0.0005f)*0.25f;
            var octave3 = PerlinSimplexNoise.Noise(worldX*0.005f, worldY*0.005f)*0.12f;
            var octave4 = PerlinSimplexNoise.Noise(worldX*0.01f, worldY*0.01f)*0.12f;
            var octave5 = PerlinSimplexNoise.Noise(worldX*0.03f, worldY*0.03f)*octave4;
            var lowerGroundHeight = octave1 + octave2 + octave3 + octave4 + octave5;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            var sunlit = true;

            var blockType = BlockType.NONE;

            for (var y = Chunk.Max.Y; y >= 0; y--)
            {
                if (y <= lowerGroundHeight)
                {
                    if (sunlit)
                    {
                        blockType = BlockType.GRASS;
                        sunlit = false;
                    }
                    else
                    {
                        blockType = BlockType.STONE;
                    }
                }
                chunk.SetBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));


                //  Debug.WriteLine(string.Format("chunk {0} : ({1},{2},{3})={4}", chunk.Position, blockXInChunk, y, blockZInChunk, blockType));
            }
        }

        #endregion

        #region BuildTree

        public virtual void BuildTree(IChunk chunk, byte tx, byte ty, byte tz)
        {
            // Trunk
            var height = (byte) (4 + (byte) R.Next(3));
            if ((ty + height) < Chunk.Max.Y)
            {
                for (var y = ty; y < ty + height; y++)
                {
                    chunk.SetBlock(tx, y, tz, new Block(BlockType.LOG));
                }
            }

            // Foliage
            var radius = 3 + R.Next(2);
            var ny = ty + height;
            for (var i = 0; i < 40 + R.Next(4); i++)
            {
                var lx = (byte)(tx + R.Next(radius) - R.Next(radius));
                var ly = (byte)(ny + R.Next(radius) - R.Next(radius));
                var lz = (byte)(tz + R.Next(radius) - R.Next(radius));
                unchecked //TODO foliage out of bound => new chunk.blockat or needs a chunk.setat
                {
                    if (Chunk.OutOfBounds(lx, ly, lz) == false)
                    {
                        if (chunk.GetBlock(lx, ly, lz).Id == BlockType.NONE)
                            chunk.SetBlock(lx, ly, lz, new Block(BlockType.LEAVES));
                    }
                    
                }
            }
        }

        #endregion

        #region MakeTreeTrunk

        private void MakeTreeTrunk(IChunk chunk, byte tx, byte ty, byte tz, int height)
        {
            Debug.WriteLine("New tree    at {0},{1},{2}={3}", tx, ty, tz, height);
            for (var y = ty; y < ty + height; y++)
            {
                chunk.SetBlock(tx, y, tz, new Block(BlockType.LOG));
            }
        }

        #endregion

        #region MakeTreeFoliage

        private void MakeTreeFoliage(IChunk chunk, int tx, int ty, int tz, int height)
        {
            Debug.WriteLine("New foliage at {0},{1},{2}={3}", tx, ty, tz, height);
            var start = ty + height - 4;
            var end = ty + height + 3;

            int rad;
            var radiusEnd = 2;
            var radiusMiddle = radiusEnd + 1;

            for (var y = start; y < end; y++)
            {
                if ((y > start) && (y < end - 1))
                {
                    rad = radiusMiddle;
                }
                else
                {
                    rad = radiusEnd;
                }

                for (var xoff = -rad; xoff < rad + 1; xoff++)
                {
                    for (var zoff = -rad; zoff < rad + 1; zoff++)
                    {
                        if (Chunk.OutOfBounds((byte) (tx + xoff), (byte) y, (byte) (tz + zoff)) == false)
                        {
                            chunk.SetBlock((byte) (tx + xoff), (byte) y, (byte) (tz + zoff), new Block(BlockType.LEAVES));
                            //Debug.WriteLine("rad={0},xoff={1},zoff={2},y={3},start={4},end={5}", rad, xoff, zoff, y, start, end);
                        }
                    }
                }
            }
        }

        #endregion

        #region Fields

        public const int Waterlevel = 64; //Chunk.SISE.Y/2
        public const int Snowlevel = 95;
        public const int Minimumgroundheight = 32; //Chunk.SIZE.Y / 4;

        public Random R;

        #endregion
    }
}