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
        private readonly double[,] _mNoiseValues;

        /// <summary>
        /// Constructor
        /// </summary>
        public PerlinNoise2D(int freq, float amp)
        {
            var rand = new Random(Environment.TickCount);
            _mNoiseValues = new double[freq, freq];
            Amplitude = amp;
            Frequency = freq;

            // Generate our Noise values
            for (var i = 0; i < freq; i++)
            {
                for (var k = 0; k < freq; k++)
                {
                    _mNoiseValues[i, k] = rand.NextDouble();
                }
            }
        }

        /// <summary>
        /// Get the interpolated point from the Noise graph using cosine interpolation
        /// </summary>
        /// <returns></returns>
        public double GetInterpolatedPoint(int xa, int xb, int ya, int yb, double x, double y)
        {
            var i1 = Interpolate(
                _mNoiseValues[xa%Frequency, ya%Frequency],
                _mNoiseValues[xb%Frequency, ya%Frequency]
                , x);

            var i2 = Interpolate(
                _mNoiseValues[xa%Frequency, yb%Frequency],
                _mNoiseValues[xb%Frequency, yb%Frequency]
                , x);

            return Interpolate(i1, i2, y);
        }

        /// <summary>
        /// Get the interpolated point from the Noise graph using cosine interpolation
        /// </summary>
        /// <returns></returns>
        private double Interpolate(double a, double b, double x)
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