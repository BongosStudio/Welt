#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;

namespace Welt.Forge.Generators
{
    /// <summary>
    /// 2D Perlin Noise function
    /// </summary>
    public class PerlinNoise2D
    {
        private readonly double[,] noiseValues;

        /// <summary>
        /// Constructor
        /// </summary>
        public PerlinNoise2D(int freq, float _amp)
        {
            var rand = new Random(Environment.TickCount);
            noiseValues = new double[freq, freq];
            Amplitude = _amp;
            Frequency = freq;

            // Generate our noise values
            for (var i = 0; i < freq; i++)
            {
                for (var k = 0; k < freq; k++)
                {
                    noiseValues[i, k] = rand.NextDouble();
                }
            }
        }

        /// <summary>
        /// Get the interpolated point from the noise graph using cosine interpolation
        /// </summary>
        /// <returns></returns>
        public double getInterpolatedPoint(int _xa, int _xb, int _ya, int _yb, double x, double y)
        {
            var i1 = interpolate(
                noiseValues[_xa%Frequency, _ya%Frequency],
                noiseValues[_xb%Frequency, _ya%Frequency]
                , x);

            var i2 = interpolate(
                noiseValues[_xa%Frequency, _yb%Frequency],
                noiseValues[_xb%Frequency, _yb%Frequency]
                , x);

            return interpolate(i1, i2, y);
        }

        /// <summary>
        /// Get the interpolated point from the noise graph using cosine interpolation
        /// </summary>
        /// <returns></returns>
        private double interpolate(double a, double b, double x)
        {
            var ft = x*Math.PI;
            var f = (1 - Math.Cos(ft))*.5;

            // Returns a Y value between 0 and 1
            return a*(1 - f) + b*f;
        }

        #region Accessors

        public float Amplitude { get; } = 1;
        public int Frequency { get; } = 1;

        #endregion
    }
}