#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Welt.IO;
using Welt.Models;

#endregion

namespace Welt.Forge.Generators
{
    internal class DualLayerTerrainWithMediumValleysForRivers : SimpleTerrain
    {
        private float _mLowerGroundHeight;
        private int _mUpperGroundHeight;

        public override void Generate(World world, Chunk chunk)
        {
            base.Generate(world, chunk);
            GenerateWaterSandLayer(chunk);
            GenerateTreesFlowers(chunk);
            chunk.State = ChunkState.AwaitingBuild;
        }

        #region generateTerrain

        protected sealed override void GenerateTerrain(World world, Chunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldZ)
        {
            _mLowerGroundHeight = GetLowerGroundHeight(chunk, worldX, worldZ);
            _mUpperGroundHeight = GetUpperGroundHeight(chunk, worldX, worldZ, _mLowerGroundHeight);

            var sunlit = true;

            for (int y = (int) Chunk.Max.Y; y >= 0; y--)
            {
                // Everything above ground height...is air.
                ushort blockType;
                if (y > _mUpperGroundHeight)
                {
                    blockType = BlockType.NONE;
                }
                // Are we above the lower ground height?
                else if (y > _mLowerGroundHeight)
                {
                    // Let's see about some caves er valleys!
                    var caveNoise = PerlinSimplexNoise.Noise(worldX*0.01f, worldZ*0.01f, y*0.01f)*(0.015f*y) + 0.1f;
                    caveNoise += PerlinSimplexNoise.Noise(worldX*0.01f, worldZ*0.01f, y*0.1f)*0.06f + 0.1f;
                    caveNoise += PerlinSimplexNoise.Noise(worldX*0.2f, worldZ*0.2f, y*0.2f)*0.03f + 0.01f;
                    // We have a cave, draw air here.
                    if (caveNoise > 0.2f)
                    {
                        blockType = BlockType.NONE;
                    }
                    else
                    {
                        if (sunlit)
                        {
                            blockType = y > Snowlevel + R.Next(3) ? BlockType.SNOW : BlockType.GRASS;
                            sunlit = false;
                        }
                        else
                        {
                            blockType = BlockType.DIRT;
                        }
                    }
                }
                else
                {
                    // We are at the lower ground level
                    if (sunlit)
                    {
                        blockType = BlockType.GRASS;
                        sunlit = false;
                    }
                    else
                    {
                        blockType = BlockType.DIRT;
                    }
                }

                if (blockType == BlockType.NONE && y <= Waterlevel)
                {
                    //if (y <= WATERLEVEL)
                    //{
                    blockType = BlockType.LAVA;
                    sunlit = false;
                    //}
                }
                chunk.SetBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));
            }
        }

        #endregion

        #region GenerateWaterSandLayer

        private void GenerateWaterSandLayer(Chunk chunk)
        {
            //BlockType blockType;
            //bool sunlit = true;
            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y;
                    //for (byte y = WATERLEVEL + 9; y >= MINIMUMGROUNDHEIGHT; y--)
                    for (byte y = Waterlevel + 9; y >= (byte) _mLowerGroundHeight; y--)
                    {
                        //blockType = chunk.Blocks[offset + y].Id;
                        if (chunk.Blocks[offset + y].Id == BlockType.NONE)
                        {
                            chunk.SetBlock(x, y, z, new Block(BlockType.WATER));
                            //blockType = BlockType.Water;
                        }
                        //else
                        //{
                        //    if (chunk.Blocks[offset + y].Id == BlockType.Grass)
                        //    {
                        //        blockType = BlockType.Sand;
                        //        //if (y <= WATERLEVEL)
                        //        //{
                        //        //    sunlit = false;
                        //        //}
                        //    }
                        //    break;
                        //}
                        //chunk.SetBlock(x, y, z, new Block(blockType));
                    }
                    for (byte y = Waterlevel + 11; y >= Waterlevel; y--)
                    {
                        if ((chunk.Blocks[offset + y] == new Block(BlockType.DIRT)) ||
                            (chunk.Blocks[offset + y] == new Block(BlockType.GRASS) ||
                            (chunk.Blocks[offset + y] == new Block(BlockType.LAVA))))
                        {
                            chunk.SetBlock(x, y, z, new Block(BlockType.SAND, 0));
                        }
                    }
                }
            }
        }

        #endregion

        #region GenerateTreesFlowers

        private void GenerateTreesFlowers(Chunk chunk)
        {
            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y;
                    for (var y = _mUpperGroundHeight; y >= Waterlevel + 9; y--)
                    {
                        if (chunk.Blocks[offset + y] == new Block(BlockType.GRASS))
                        {
                            if (R.Next(700) == 1)
                            {
                                BuildTree(chunk, x, (byte) y, z);
                            }
                            else if (R.Next(50) == 1)
                            {
                                y++;
                                chunk.SetBlock(x, (byte) y, z, new Block(BlockType.FLOWER_ROSE));
                            }
                            //else if (R.Next(5) == 1)
                            //{
                            //    y++;
                            //    chunk.SetBlock(x, (byte)y, z, new Block(BlockType.LONG_GRASS));
                            //}
                            //else if (r.Next(2) == 1)
                            //{
                            //    y++;
                            //    chunk.SetBlock(x, y, z, new Block(BlockType.LongGrass));
                            //}
                        }
                    }
                }
            }
        }

        #endregion

        #region GetUpperGroundHeight

        private static int GetUpperGroundHeight(Chunk chunk, uint blockX, uint blockY, float lowerGroundHeight)
        {
            var octave1 = PerlinSimplexNoise.Noise((blockX + 100)*0.001f, blockY*0.001f)*0.5f;
            var octave2 = PerlinSimplexNoise.Noise((blockX + 100)*0.002f, blockY*0.002f)*0.25f;
            var octave3 = PerlinSimplexNoise.Noise((blockX + 100)*0.01f, blockY*0.01f)*0.25f;
            var octaveSum = octave1 + octave2 + octave3;

            return (int) (octaveSum*(Chunk.Size.Y/2f)) + (int) (lowerGroundHeight);
        }

        #endregion

        #region GetLowerGroundHeight

        private static float GetLowerGroundHeight(Chunk chunk, uint blockX, uint blockY)
        {
            var minimumGroundheight = Chunk.Size.Y/4;
            var minimumGroundDepth = (int) (Chunk.Size.Y*0.5f);

            var octave1 = PerlinSimplexNoise.Noise(blockX*0.0001f, blockY*0.0001f)*0.5f;
            var octave2 = PerlinSimplexNoise.Noise(blockX*0.0005f, blockY*0.0005f)*0.35f;
            var octave3 = PerlinSimplexNoise.Noise(blockX*0.02f, blockY*0.02f)*0.15f;
            var lowerGroundHeight = octave1 + octave2 + octave3;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            return lowerGroundHeight;
        }

        #endregion
    }
}