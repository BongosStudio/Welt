#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using Welt.Models;

#endregion

namespace Welt.Forge.Generators
{
    internal class SimpleTerrain : IChunkGenerator
    {
        #region build

        public virtual void Generate(Chunk chunk)
        {
            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                var worldX = chunk.Position.X + x + (uint) World.Seed;

                for (byte z = 0; z < Chunk.SIZE.Z; z++)
                {
                    var worldZ = chunk.Position.Z + z;
                    generateTerrain(chunk, x, z, worldX, worldZ);
                }
            }
            chunk.State = ChunkState.AwaitingBuild;
            //chunk.generated = true;
        }

        #endregion

        #region generateTerrain

        protected virtual void generateTerrain(Chunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldY)
        {
            // The lower ground level is at least this high.
            var minimumGroundheight = Chunk.SIZE.Y/4;
            var minimumGroundDepth = (int) (Chunk.SIZE.Y*0.75f);

            var octave1 = PerlinSimplexNoise.noise(worldX*0.0001f, worldY*0.0001f)*0.5f;
            var octave2 = PerlinSimplexNoise.noise(worldX*0.0005f, worldY*0.0005f)*0.25f;
            var octave3 = PerlinSimplexNoise.noise(worldX*0.005f, worldY*0.005f)*0.12f;
            var octave4 = PerlinSimplexNoise.noise(worldX*0.01f, worldY*0.01f)*0.12f;
            var octave5 = PerlinSimplexNoise.noise(worldX*0.03f, worldY*0.03f)*octave4;
            var lowerGroundHeight = octave1 + octave2 + octave3 + octave4 + octave5;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            var sunlit = true;

            var blockType = BlockType.None;

            for (int y = Chunk.MAX.Y; y >= 0; y--)
            {
                if (y <= lowerGroundHeight)
                {
                    if (sunlit)
                    {
                        blockType = BlockType.Grass;
                        sunlit = false;
                    }
                    else
                    {
                        blockType = BlockType.Rock;
                    }
                }


                chunk.setBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));


                //  Debug.WriteLine(string.Format("chunk {0} : ({1},{2},{3})={4}", chunk.Position, blockXInChunk, y, blockZInChunk, blockType));
            }
        }

        #endregion

        #region BuildTree

        public virtual void BuildTree(Chunk chunk, byte tx, byte ty, byte tz)
        {
            // Trunk
            var height = (byte) (4 + (byte) r.Next(3));
            if ((ty + height) < Chunk.MAX.Y)
            {
                for (var y = ty; y < ty + height; y++)
                {
                    chunk.setBlock(tx, y, tz, new Block(BlockType.Tree));
                }
            }

            // Foliage
            var radius = 3 + r.Next(2);
            var ny = ty + height;
            for (var i = 0; i < 40 + r.Next(4); i++)
            {
                var lx = tx + r.Next(radius) - r.Next(radius);
                var ly = ny + r.Next(radius) - r.Next(radius);
                var lz = tz + r.Next(radius) - r.Next(radius);
                unchecked //TODO foliage out of bound => new chunk.blockat or needs a chunk.setat
                {
                    if (chunk.outOfBounds((byte) lx, (byte) ly, (byte) lz) == false)
                    {
                        //if (chunk.Blocks[lx, ly, lz].Type == BlockType.None)
                        if (chunk.Blocks[lx*Chunk.FlattenOffset + lz*Chunk.SIZE.Y + ly].Type == BlockType.None)
                            chunk.setBlock((byte) lx, (byte) ly, (byte) lz, new Block(BlockType.Leaves));
                    }
                }
            }
        }

        #endregion

        #region MakeTreeTrunk

        private void MakeTreeTrunk(Chunk chunk, byte tx, byte ty, byte tz, int height)
        {
            Debug.WriteLine("New tree    at {0},{1},{2}={3}", tx, ty, tz, height);
            for (var y = ty; y < ty + height; y++)
            {
                chunk.setBlock(tx, y, tz, new Block(BlockType.Tree));
            }
        }

        #endregion

        #region MakeTreeFoliage

        private void MakeTreeFoliage(Chunk chunk, int tx, int ty, int tz, int height)
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
                        if (chunk.outOfBounds((byte) (tx + xoff), (byte) y, (byte) (tz + zoff)) == false)
                        {
                            chunk.setBlock((byte) (tx + xoff), (byte) y, (byte) (tz + zoff), new Block(BlockType.Leaves));
                            //Debug.WriteLine("rad={0},xoff={1},zoff={2},y={3},start={4},end={5}", rad, xoff, zoff, y, start, end);
                        }
                    }
                }
            }
        }

        #endregion

        #region Fields

        public const int WATERLEVEL = 64; //Chunk.SISE.Y/2
        public const int SNOWLEVEL = 95;
        public const int MINIMUMGROUNDHEIGHT = 32; //Chunk.SIZE.Y / 4;

        public Random r = new Random(World.Seed);

        #endregion
    }
}