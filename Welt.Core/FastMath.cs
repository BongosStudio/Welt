#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Welt.API;

namespace Welt.Core
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

        private static readonly Random m_Random = new Random();
        private static readonly object m_Rlock = new object();

        #endregion

        #region Static Methods

        /// <summary>
        /// Maps a float from 0...360 to 0...255
        /// </summary>
        /// <param name="value"></param>
        public static sbyte CreateRotationByte(float value)
        {
            return (sbyte)(((value % 360) / 360) * 256);
        }

        public static float UnpackRotationByte(sbyte value)
        {
            return (value / 256f) * 360f;
        }

        public static int CreateAbsoluteInt(double value)
        {
            return (int)(value * 32);
        }

        public static double ToRadians(double degrees)
        {
            return degrees * 0.017453292519943295769236907684886;
        }

        public static Vector3 RotateX(Vector3 vector, double rotation)
        {
            rotation = -rotation;
            return new Vector3(
                vector.X,
                vector.Y * (float)Math.Cos(rotation) - vector.Z * (float)Math.Sin(rotation),
                vector.Y * (float)Math.Sin(rotation) + vector.Z * (float)Math.Cos(rotation));
        }

        public static Vector3 RotateY(Vector3 vector, double rotation)
        {
            rotation = -rotation;
            return new Vector3(
                vector.Z * (float)Math.Sin(rotation) + vector.X * (float)Math.Cos(rotation),
                vector.Y,
                vector.Z * (float)Math.Cos(rotation) - vector.X * (float)Math.Sin(rotation));
        }

        public static Vector3 RotateZ(Vector3 vector, double rotation)
        {
            rotation = -rotation; 
            return new Vector3(
                vector.X * (float)Math.Cos(rotation) - vector.Y * (float)Math.Sin(rotation),
                vector.X * (float)Math.Sin(rotation) + vector.Y * (float)Math.Cos(rotation),
                vector.Z);
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double RadiansToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static int Get4DHash(int x, int y, int z, int w)
        {
            unchecked
            {
                var hash = x.GetHashCode();
                hash = (hash * 397) ^ y.GetHashCode();
                hash = (hash * 397) ^ z.GetHashCode();
                hash = (hash * 397) ^ w.GetHashCode();
                return hash;
            }
        }

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

        public static double DistanceTo(float ax, float az, float bx, float bz)
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
            lock (m_Rlock)
            {
                return m_Random.Next(max);
            }
        }

        public static uint NextRandom(uint max)
        {
            lock (m_Rlock)
            {
                return (uint) m_Random.Next((int) max);
            }
        }

        public static int NextRandom(int min, int max)
        {
            lock (m_Rlock)
            {
                return m_Random.Next(min, max);
            }
        }

        public static uint NextRandom(uint min, uint max)
        {
            lock (m_Rlock)
            {
                return (uint)m_Random.Next((int) min, (int)max);
            }
        }

        public static double NextRandomDouble()
        {
            lock (m_Rlock)
            {
                return m_Random.NextDouble();
            }
        }

        public static bool NextRandomBoolean()
        {
            lock (m_Rlock)
            {
                return m_Random.Next(2) == 1;
            }
        }

        public static float NextRandomBetween(float lower, float upper)
        {
            var rand = NextRandom((int)Math.Floor(lower), (int)Math.Ceiling(upper));
            var randD = (float)NextRandomDouble();
            if (rand + randD > upper) return rand - randD;
            if (rand - randD < lower) return rand + randD;
            return NextRandomBoolean() ? rand + randD : rand - randD;
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

        public static Vector3I BlockFaceToCoordinates(BlockFaceDirection face)
        {
            switch (face)
            {
                case BlockFaceDirection.YDecreasing:
                    return Vector3I.Down;
                case BlockFaceDirection.YIncreasing:
                    return Vector3I.Up;
                case BlockFaceDirection.ZDecreasing:
                    return Vector3I.Backward;
                case BlockFaceDirection.ZIncreasing:
                    return Vector3I.Forward;
                case BlockFaceDirection.XDecreasing:
                    return Vector3I.Left;
                default:
                    return Vector3I.Right;
            }
        }

        public static BlockFaceDirection CoordinatesToBlockFace(Vector3I face)
        {
            if (face == Vector3I.Down)
                return BlockFaceDirection.YDecreasing;
            else if (face == Vector3I.Up)
                return BlockFaceDirection.YIncreasing;
            else if (face == Vector3I.Backward)
                return BlockFaceDirection.ZDecreasing;
            else if (face == Vector3I.Forward)
                return BlockFaceDirection.ZIncreasing;
            else if (face == Vector3I.Left)
                return BlockFaceDirection.XDecreasing;
            else
                return BlockFaceDirection.XIncreasing;
        }

        public static double Distance2D(double a1, double a2, double b1, double b2)
        {
            return Math.Sqrt(Math.Pow(b1 - a1, 2) + Math.Pow(b2 - a2, 2));
        }

        //public static Direction DirectionByRotationFlat(float yaw, bool invert = false)
        //{
        //    byte direction = (byte)((int)Math.Floor((yaw * 4F) / 360F + 0.5D) & 3);
        //    if (invert)
        //        switch (direction)
        //        {
        //            case 0: return Direction.North;
        //            case 1: return Direction.East;
        //            case 2: return Direction.South;
        //            case 3: return Direction.West;
        //        }
        //    else
        //        switch (direction)
        //        {
        //            case 0: return Direction.South;
        //            case 1: return Direction.West;
        //            case 2: return Direction.North;
        //            case 3: return Direction.East;
        //        }
        //    return 0;
        //}

        #endregion
    }
}
