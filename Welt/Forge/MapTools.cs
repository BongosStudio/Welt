#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System.Collections.Generic;
using Welt.Forge.Generators;

#endregion

namespace Welt.Forge
{
    public class MapTools
    {
        #region ClearChunkBlocks

        public static void ClearChunkBlocks(Chunk chunk)
        {
            for (byte x = 0; x < Chunk.SIZE.X; x++)
            {
                for (byte y = 0; y < Chunk.SIZE.Y; y++)
                {
                    for (byte z = 0; z < Chunk.SIZE.Z; z++)
                    {
                        chunk.setBlock(x, y, z, new Block(BlockType.None));
                    }
                }
            }
        }

        #endregion

        #region SumNoiseFunctions

        /// <summary>
        /// Get the interpolated points for the noise function
        /// </summary>
        /// <param name="noiseFn"></param>
        /// <returns></returns>
        public static double[,] SumNoiseFunctions(int width, int height, List<PerlinNoise2D> noiseFunctions)
        {
            var summedValues = new double[width, height];

            // Sum each of the noise functions
            for (var i = 0; i < noiseFunctions.Count; i++)
            {
                double x_step = width/(float) noiseFunctions[i].Frequency;
                double y_step = height/(float) noiseFunctions[i].Frequency;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var a = (int) (x/x_step);
                        var b = a + 1;
                        var c = (int) (y/y_step);
                        var d = c + 1;

                        var intpl_val = noiseFunctions[i].getInterpolatedPoint(a, b, c, d, (x/x_step) - a,
                            (y/y_step) - c);
                        summedValues[x, y] += intpl_val*noiseFunctions[i].Amplitude;
                    }
                }
            }
            return summedValues;
        }

        #endregion
    }
}