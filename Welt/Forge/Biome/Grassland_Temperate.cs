#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge.Generators;

#endregion

namespace Welt.Forge.Biome
{
    internal class GrasslandTemperate : SimpleTerrain
    {
        public override void Generate(Chunk chunk)
        {
            base.Generate(chunk);
            //GenerateWaterSandLayer(chunk);
        }

        #region generateTerrain

        protected override sealed void GenerateTerrain(Chunk chunk, byte blockXInChunk, byte blockZInChunk, uint worldX,
            uint worldZ)
        {
            var lowerGroundHeight = GetLowerGroundHeight(chunk, worldX, worldZ);
            var upperGroundHeight = GetUpperGroundHeight(chunk, worldX, worldZ, lowerGroundHeight);

            var sunlit = true;
            ushort blockType;

            for (int y = Chunk.Max.Y; y >= 0; y--)
            {
                blockType = BlockType.None;
                if (y > upperGroundHeight)
                {
                    blockType = BlockType.None;
                }
                else
                {
                    blockType = BlockType.Grass;
                }

                if ((y > lowerGroundHeight) && (y < upperGroundHeight))
                {
                    sunlit = false;
                }
                else
                {
                    sunlit = true;
                }
                chunk.SetBlock(blockXInChunk, (byte) y, blockZInChunk, new Block(blockType));
            }
        }

        #endregion

        #region GenerateWaterSandLayer

        private void GenerateWaterSandLayer(Chunk chunk)
        {
            ushort blockType;

            var sunlit = true;

            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y;
                    for (byte y = Waterlevel + 9; y >= Minimumgroundheight; y--)
                    {
                        blockType = BlockType.None;
                        //if (chunk.Blocks[x, y, z].Id == BlockType.None)
                        if (chunk.Blocks[offset + y].Id == BlockType.None)
                        {
                            //    blockType = BlockType.Water;
                        }
                        else
                        {
                            //if (chunk.Blocks[x, y, z].Id == BlockType.Grass)
                            if (chunk.Blocks[offset + y].Id == BlockType.Grass)
                            {
                                blockType = BlockType.Grass;
                                if (y <= Waterlevel)
                                {
                                    sunlit = false;
                                }
                            }
                            break;
                        }

                        chunk.SetBlock(x, y, z, new Block(blockType));
                    }

                    for (byte y = Waterlevel + 27; y >= Waterlevel; y--)
                    {
                        //if ((y > 11) && (chunk.Blocks[x, y, z].Id == BlockType.Grass)) chunk.SetBlock(x, y, z, new Block(BlockType.Grass, sunlit));
                        if ((y > 11) && (chunk.Blocks[offset + y].Id == BlockType.Grass))
                            chunk.SetBlock(x, y, z, new Block(BlockType.Grass));

                        //if ((chunk.Blocks[x, y, z].Id == BlockType.Dirt) || (chunk.Blocks[x, y, z].Id == BlockType.Grass))
                        if ((chunk.Blocks[offset + y].Id == BlockType.Dirt) ||
                            (chunk.Blocks[offset + y].Id == BlockType.Grass))
                        {
                            chunk.SetBlock(x, y, z, new Block(BlockType.Grass));
                        }
                    }
                }
            }
        }

        #endregion

        #region GetUpperGroundHeight

        private static int GetUpperGroundHeight(Chunk chunk, uint blockX, uint blockY, float lowerGroundHeight)
        {
            var octave1 = PerlinSimplexNoise.Noise((blockX + 50)*0.0002f, blockY*0.0002f)*0.05f;
            var octave2 = PerlinSimplexNoise.Noise((blockX + 50)*0.0005f, blockY*0.0005f)*0.135f;
            var octave3 = PerlinSimplexNoise.Noise((blockX + 50)*0.0025f, blockY*0.0025f)*0.15f;
            var octave4 = PerlinSimplexNoise.Noise((blockX + 50)*0.0125f, blockY*0.0125f)*0.05f;
            var octave5 = PerlinSimplexNoise.Noise((blockX + 50)*0.025f, blockY*0.025f)*0.015f;
            var octave6 = PerlinSimplexNoise.Noise((blockX + 50)*0.0125f, blockY*0.0125f)*0.04f;

            var octaveSum = octave1 + octave2 + octave3 + octave4 + octave5 + octave6;

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
            //float octave3 = PerlinSimplexNoise.Noise(blockX * 0.02f, blockY * 0.02f) * 0.15f;
            var lowerGroundHeight = octave1 + octave2;
            //float lowerGroundHeight = octave1 + octave2 + octave3;

            lowerGroundHeight = lowerGroundHeight*minimumGroundDepth + minimumGroundheight;

            return lowerGroundHeight;
        }

        #endregion
    }
}