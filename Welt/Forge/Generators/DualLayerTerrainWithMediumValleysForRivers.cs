#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Models;

#endregion

namespace Welt.Forge.Generators
{
    internal class DualLayerTerrainWithMediumValleysForRivers : SimpleTerrain
    {
        private float lowerGroundHeight;
        private int upperGroundHeight;

        public override void Generate(Chunk chunk)
        {
            base.Generate(chunk);
            GenerateWaterSandLayer(chunk);
            GenerateTreesFlowers(chunk);
            chunk.State = ChunkState.AwaitingBuild;
        }

        #region generateTerrain

        protected override sealed void generateTerrain(Chunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldZ)
        {
            lowerGroundHeight = GetLowerGroundHeight(chunk, worldX, worldZ);
            upperGroundHeight = GetUpperGroundHeight(chunk, worldX, worldZ, lowerGroundHeight);

            var sunlit = true;

            for (int y = Chunk.MAX.Y; y >= 0; y--)
            {
                // Everything above ground height...is air.
                BlockType blockType;
                if (y > upperGroundHeight)
                {
                    blockType = BlockType.None;
                }
                // Are we above the lower ground height?
                else if (y > lowerGroundHeight)
                {
                    // Let's see about some caves er valleys!
                    var caveNoise = PerlinSimplexNoise.noise(worldX*0.01f, worldZ*0.01f, y*0.01f)*(0.015f*y) + 0.1f;
                    caveNoise += PerlinSimplexNoise.noise(worldX*0.01f, worldZ*0.01f, y*0.1f)*0.06f + 0.1f;
                    caveNoise += PerlinSimplexNoise.noise(worldX*0.2f, worldZ*0.2f, y*0.2f)*0.03f + 0.01f;
                    // We have a cave, draw air here.
                    if (caveNoise > 0.2f)
                    {
                        blockType = BlockType.None;
                    }
                    else
                    {
                        blockType = BlockType.None;
                        if (sunlit)
                        {
                            if (y > SNOWLEVEL + r.Next(3))
                            {
                                blockType = BlockType.Snow;
                            }
                            else
                            {
                                blockType = BlockType.Grass;
                            }
                            sunlit = false;
                        }
                        else
                        {
                            blockType = BlockType.Dirt;
                        }
                    }
                }
                else
                {
                    // We are at the lower ground level
                    if (sunlit)
                    {
                        blockType = BlockType.Grass;
                        sunlit = false;
                    }
                    else
                    {
                        blockType = BlockType.Dirt;
                    }
                }

                if (blockType == BlockType.None && y <= WATERLEVEL)
                {
                    //if (y <= WATERLEVEL)
                    //{
                    blockType = BlockType.Lava;
                    sunlit = false;
                    //}
                }
                chunk.setBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));
            }
        }

        #endregion

        #region GenerateWaterSandLayer

        private void GenerateWaterSandLayer(Chunk chunk)
        {
            //BlockType blockType;
            //bool sunlit = true;

            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                for (byte z = 0; z < Chunk.SIZE.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.SIZE.Y;
                    //for (byte y = WATERLEVEL + 9; y >= MINIMUMGROUNDHEIGHT; y--)
                    for (byte y = WATERLEVEL + 9; y >= (byte) lowerGroundHeight; y--)
                    {
                        //blockType = chunk.Blocks[offset + y].Type;
                        if (chunk.Blocks[offset + y].Type == BlockType.None)
                        {
                            chunk.setBlock(x, y, z, new Block(BlockType.Water));
                            //blockType = BlockType.Water;
                        }
                        //else
                        //{
                        //    if (chunk.Blocks[offset + y].Type == BlockType.Grass)
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
                    for (byte y = WATERLEVEL + 11; y >= WATERLEVEL; y--)
                    {
                        if ((chunk.Blocks[offset + y].Type == BlockType.Dirt) ||
                            (chunk.Blocks[offset + y].Type == BlockType.Grass) ||
                            (chunk.Blocks[offset + y].Type == BlockType.Lava))
                        {
                            chunk.setBlock(x, y, z, new Block(BlockType.Sand));
                        }
                    }
                }
            }
        }

        #endregion

        #region GenerateTreesFlowers

        private void GenerateTreesFlowers(Chunk chunk)
        {
            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                for (byte z = 0; z < Chunk.SIZE.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.SIZE.Y;
                    for (var y = upperGroundHeight + 1; y >= WATERLEVEL + 9; y--)
                    {
                        if (chunk.Blocks[offset + y].Type == BlockType.Grass)
                        {
                            if (r.Next(700) == 1)
                            {
                                BuildTree(chunk, x, (byte) y, z);
                            }
                            else if (r.Next(50) == 1)
                            {
                                y++;
                                chunk.setBlock(x, (byte) y, z, new Block(BlockType.RedFlower));
                            }
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
            var octave1 = PerlinSimplexNoise.noise((blockX + 100)*0.001f, blockY*0.001f)*0.5f;
            var octave2 = PerlinSimplexNoise.noise((blockX + 100)*0.002f, blockY*0.002f)*0.25f;
            var octave3 = PerlinSimplexNoise.noise((blockX + 100)*0.01f, blockY*0.01f)*0.25f;
            var octaveSum = octave1 + octave2 + octave3;

            return (int) (octaveSum*(Chunk.SIZE.Y/2f)) + (int) (lowerGroundHeight);
        }

        #endregion

        #region GetLowerGroundHeight

        private static float GetLowerGroundHeight(Chunk chunk, uint blockX, uint blockY)
        {
            var minimumGroundheight = Chunk.SIZE.Y/4;
            var minimumGroundDepth = (int) (Chunk.SIZE.Y*0.5f);

            var octave1 = PerlinSimplexNoise.noise(blockX*0.0001f, blockY*0.0001f)*0.5f;
            var octave2 = PerlinSimplexNoise.noise(blockX*0.0005f, blockY*0.0005f)*0.35f;
            var octave3 = PerlinSimplexNoise.noise(blockX*0.02f, blockY*0.02f)*0.15f;
            var lowerGroundHeight = octave1 + octave2 + octave3;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            return lowerGroundHeight;
        }

        #endregion
    }
}