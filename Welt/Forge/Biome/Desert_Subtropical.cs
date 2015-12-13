#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge.Generators;

#endregion

namespace Welt.Forge.Biome
{
    internal class Desert_Subtropical : SimpleTerrain
    {
        public override void Generate(Chunk chunk)
        {
            base.Generate(chunk);
            //GenerateWaterSandLayer(chunk);
        }

        #region generateTerrain

        protected override sealed void generateTerrain(Chunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldZ)
        {
            var lowerGroundHeight = GetLowerGroundHeight(chunk, worldX, worldZ);
            var upperGroundHeight = GetUpperGroundHeight(chunk, worldX, worldZ, lowerGroundHeight);

            //bool sunlit = true;
            BlockType blockType;

            for (int y = Chunk.MAX.Y; y >= 0; y--)
            {
                blockType = BlockType.None;
                if (y > upperGroundHeight)
                {
                    blockType = BlockType.None;
                }
                else
                {
                    blockType = BlockType.Sand;
                }

                //if ( (y > lowerGroundHeight) && (y < upperGroundHeight) )
                //{
                //    sunlit = false;
                //}
                //else
                //{
                //    sunlit = true;
                //}
                chunk.setBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));
            }
        }

        #endregion

        #region GenerateWaterSandLayer

        private void GenerateWaterSandLayer(Chunk chunk)
        {
            BlockType blockType;

            var sunlit = true;

            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                for (byte z = 0; z < Chunk.SIZE.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.SIZE.Y;
                    for (byte y = WATERLEVEL + 9; y >= MINIMUMGROUNDHEIGHT; y--)
                    {
                        blockType = BlockType.None;
                        //if (chunk.Blocks[x, y, z].Type == BlockType.None)
                        if (chunk.Blocks[offset + y].Type == BlockType.None)
                        {
                            //    blockType = BlockType.Water;
                        }
                        else
                        {
                            //if (chunk.Blocks[x, y, z].Type == BlockType.Grass)
                            if (chunk.Blocks[offset + y].Type == BlockType.Grass)
                            {
                                blockType = BlockType.Sand;
                                if (y <= WATERLEVEL)
                                {
                                    sunlit = false;
                                }
                            }
                            break;
                        }

                        chunk.setBlock(x, y, z, new Block(blockType));
                    }

                    for (byte y = WATERLEVEL + 27; y >= WATERLEVEL; y--)
                    {
                        //if ((y > 11) && (chunk.Blocks[x, y, z].Type == BlockType.Grass)) chunk.SetBlock(x, y, z, new Block(BlockType.Sand, sunlit));
                        if ((y > 11) && (chunk.Blocks[offset + y].Type == BlockType.Grass))
                            chunk.setBlock(x, y, z, new Block(BlockType.Sand));

                        //if ((chunk.Blocks[x, y, z].Type == BlockType.Dirt) || (chunk.Blocks[x, y, z].Type == BlockType.Grass))
                        if ((chunk.Blocks[offset + y].Type == BlockType.Dirt) ||
                            (chunk.Blocks[offset + y].Type == BlockType.Grass))

                        {
                            chunk.setBlock(x, y, z, new Block(BlockType.Sand));
                        }
                    }
                }
            }
        }

        #endregion

        #region GetUpperGroundHeight

        private static int GetUpperGroundHeight(Chunk chunk, uint blockX, uint blockY, float lowerGroundHeight)
        {
            var octave1 = PerlinSimplexNoise.noise((blockX + 50)*0.0002f, blockY*0.0002f)*0.05f;
            var octave2 = PerlinSimplexNoise.noise((blockX + 50)*0.0005f, blockY*0.0005f)*0.135f;
            var octave3 = PerlinSimplexNoise.noise((blockX + 50)*0.0025f, blockY*0.0025f)*0.15f;
            var octave4 = PerlinSimplexNoise.noise((blockX + 50)*0.0125f, blockY*0.0125f)*0.05f;
            var octave5 = PerlinSimplexNoise.noise((blockX + 50)*0.025f, blockY*0.025f)*0.015f;
            var octave6 = PerlinSimplexNoise.noise((blockX + 50)*0.0125f, blockY*0.0125f)*0.04f;
            var octaveSum = octave1 + octave2 + octave3 + octave4 + octave5 + octave6;

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
            //float octave3 = PerlinSimplexNoise.noise(blockX * 0.02f, blockY * 0.02f) * 0.15f;
            var lowerGroundHeight = octave1 + octave2;
            //float lowerGroundHeight = octave1 + octave2 + octave3;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            return lowerGroundHeight;
        }

        #endregion
    }
}