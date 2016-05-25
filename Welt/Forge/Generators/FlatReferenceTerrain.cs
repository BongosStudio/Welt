#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
namespace Welt.Forge.Generators
{
    internal class FlatReferenceTerrain : IChunkGenerator
    {
        #region build

        public void Generate(Chunk chunk)
        {
            int sizeY = Chunk.Size.Y;
            int sizeX = Chunk.Size.X;
            int sizeZ = Chunk.Size.Z;

            for (byte y = 0; y < sizeY; y++)
            {
                for (byte x = 0; x < sizeX; x++)
                {
                    for (byte z = 0; z < sizeZ; z++)
                    {
                        var block = new Block(BlockType.None);

                        if (y < sizeY/4)
                            block.Id = BlockType.Lava;
                        /*
                         * else if (y == (sizeY / 2) - 1) // test caves visibility 
                         * block.Id = Id.empty;
                         */
                        else if (y < sizeY/2)
                            block.Id = BlockType.Rock;
                        else if (y == sizeY/2)
                        {
                            var i = chunk.Index.X%2 == 0
                                ? chunk.Index.X ^ chunk.Index.Y
                                : chunk.Index.X/(chunk.Index.Y + 1);

                            block.Id = (ushort) (i%(BlockType.Maximum - 1));
                        }
                        else
                        {
                            if (y == sizeY/2 + 1 && (x == 0 || x == sizeX - 1 || z == 0 || z == sizeZ - 1))
                                block.Id = BlockType.Sand;
                            else
                                block.Id = BlockType.None;
                        }

                        var h = (byte) (chunk.Index.Z%2 == 0 && y > 1 ? y - 1 : y);

                        chunk.SetBlock(x, h, z, block);
                    }
                }
            }
        }

        #endregion
    }
}