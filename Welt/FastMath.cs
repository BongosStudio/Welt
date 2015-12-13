#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Linq;

namespace Welt
{
    public static class FastMath
    {
        #region Constants

        public const float E = 2.718282f;
        public const float Log2E = 1.442695f;
        public const float Log10E = 0.4342945f;
        public const float Pi = 3.141593f;
        public const float TwoPi = 6.283185f;
        public const float PiOver2 = 1.570796f;
        public const float PiOver4 = 0.7853982f;

        #endregion

        #region Private Properties

        private static readonly Random _random = new Random();
        private static readonly object _rLock = new object();

        #endregion

        #region Static Methods

        public static void Flip(ref int value)
        {
            value = -value;
        }

        public static void Flip(ref long value)
        {
            value = -value;
        }

        public static void Flip(ref float value)
        {
            value = -value;
        }

        public static void Flip(ref double value)
        {
            value = -value;
        }
        
        public static double DistanceTo(int ax, int az, int bx, int bz)
        {
            var a = Math.Abs(ax - bx);
            var b = Math.Abs(az - bz);
            return Math.Sqrt((a*a) + (b*b));
        }

        public static int Average(params int[] values)
        {
            return values.Sum()/values.Length;
        }

        public static long Average(params long[] values)
        {
            return values.Sum()/values.Length;
        }

        public static float Average(params float[] values)
        {
            return values.Sum()/values.Length;
        }

        public static double Average(params double[] values)
        {
            return values.Sum()/values.Length;
        }

        public static int NextRandom(int max)
        {
            lock (_rLock)
            {
                return _random.Next(max);
            }
        }

        public static int NextRandom(int min, int max)
        {
            lock (_rLock)
            {
                return _random.Next(min, max);
            }
        }

        public static double NextRandomDouble()
        {
            lock (_rLock)
            {
                return _random.NextDouble();
            }
        }

        public static bool NextRandomBoolean()
        {
            lock (_rLock)
            {
                return _random.Next(2) == 1;
            }
        }

        public static float FRand()
        {
            var b = NextRandomBoolean();
            var value = (float) NextRandomDouble();
            if (b) Flip(ref value);
            return value;
        }

        public static int Floor(float value)
        {
            if (value > 0) return (int) value;
            return (int) value - 1;
        }

        public static int Ceiling(float value)
        {
            if (value > 0) return (int) value + 1;
            return (int) value;
        }

        public static int Floor(double value)
        {
            if (value > 0) return (int) value;
            return (int) value - 1;
        }

        public static int Ceiling(double value)
        {
            if (value > 0) return (int) value + 1;
            return (int) value;
        }

        public static bool WithinBounds(int lower, int upper, int value)
        {
            return value >= lower && value <= upper;
        }

        public static bool WithinBounds(float lower, float upper, float value)
        {
            return value >= lower && value <= upper;
        }

        public static bool WithinBounds(double lower, double upper, double value)
        {
            return value >= lower && value <= upper;
        }

        public static bool WithinBounds(uint lower, uint upper, uint value)
        {
            return value >= lower && value <= upper;
        }

        public static bool WithinBounds(short lower, short upper, short value)
        {
            return value >= lower && value <= upper;
        }

        public static bool WithinBounds(ushort lower, ushort upper, ushort value)
        {
            return value >= lower && value <= upper;
        }

        public static void Adjust(float min, float max, ref float value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
        }

        public static void Adjust(int min, int max, ref int value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
        }

        public static void Adjust(double min, double max, ref double value)
        {
            if (value < min) value = min;
            if (value > max) value = max;
        }
        
        public static float ToRadians(float degrees)
        {
            return degrees*0.01745329f;
        }

        public static float ToDegrees(float radians)
        {
            return radians*57.29578f;
        }

        public static int BytesToInt(byte[] bytes, int offset)
        {
            var ret = 0;

            for (var i = 0; i < 4 && i + offset < bytes.Length; i++)
            {
                ret <<= 8;
                ret |= bytes[i] & 0xFF;
            }

            return ret;
        }

        #endregion
    }
}