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
            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte y = 0; y < Chunk.Size.Y; y++)
                {
                    for (byte z = 0; z < Chunk.Size.Z; z++)
                    {
                        chunk.SetBlock(x, y, z, new Block(BlockType.NONE));
                    }
                }
            }
        }

        #endregion

        #region SumNoiseFunctions

        /// <summary>
        /// Get the interpolated points for the Noise function
        /// </summary>
        /// <param name="noiseFn"></param>
        /// <returns></returns>
        public static double[,] SumNoiseFunctions(int width, int height, List<PerlinNoise2D> noiseFunctions)
        {
            var summedValues = new double[width, height];

            // Sum each of the Noise functions
            for (var i = 0; i < noiseFunctions.Count; i++)
            {
                double xStep = width/(float) noiseFunctions[i].Frequency;
                double yStep = height/(float) noiseFunctions[i].Frequency;

                for (var x = 0; x < width; x++)
                {
                    for (var y = 0; y < height; y++)
                    {
                        var a = (int) (x/xStep);
                        var b = a + 1;
                        var c = (int) (y/yStep);
                        var d = c + 1;

                        var intplVal = noiseFunctions[i].GetInterpolatedPoint(a, b, c, d, (x/xStep) - a,
                            (y/yStep) - c);
                        summedValues[x, y] += intplVal*noiseFunctions[i].Amplitude;
                    }
                }
            }
            return summedValues;
        }

        #endregion
    }
}