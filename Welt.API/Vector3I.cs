#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;
using System;

namespace Welt.API
{

    public struct Vector3I
    {
        public uint X;
        public uint Y;
        public uint Z;

        public Vector3I(uint x, uint y, uint z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3I(int x, int y, int z)
        {
            X = (uint) x;
            Y = (uint) y;
            Z = (uint) z;
        }

        public static Vector3I One = new Vector3I(1, 1, 1);
        public static Vector3I OneX = new Vector3I(1, 0, 0);
        public static Vector3I OneY = new Vector3I(0, 1, 0);
        public static Vector3I OneZ = new Vector3I(0, 0, 1);

        public static Vector3I Up = new Vector3I(0, 1, 0);
        public static Vector3I Down = new Vector3I(0, -1, 0);
        public static Vector3I Forward = new Vector3I(0, 0, 1);
        public static Vector3I Backward = new Vector3I(0, 0, -1);
        public static Vector3I Right = new Vector3I(1, 0, 0);
        public static Vector3I Left = new Vector3I(-1, 0, 0);

        public double DistanceTo(Vector3I other)
        {
            return Math.Sqrt(Square(other.X - X) +
                             Square(other.Y - Y) +
                             Square(other.Z - Z));
        }

        private uint Square(uint num)
        {
            return num * num;
        }

        public static implicit operator Vector3I(Vector3 value)
        {
            return new Vector3I((uint)value.X, (uint)value.Y, (uint)value.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3I)
            {
                var other = (Vector3I)obj;
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(Vector3I a, Vector3I b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3I a, Vector3I b)
        {
            return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static Vector3I operator +(Vector3I a, Vector3I b)
        {
            return new Vector3I(a.X + b.X, a.Y + b.Y, a.Z + b.Z);
        }

        public static Vector3I operator -(Vector3I a, Vector3I b)
        {
            return new Vector3(a.X - b.X, a.Y - b.Y, a.Z - b.Z);
        }

        public static Vector3I operator %(Vector3I v, uint by)
        {
            return new Vector3I(v.X % by, v.Y % by, v.Z % by);
        }

        public static Vector3I operator %(Vector3I v, int by)
        {
            return new Vector3I((uint)(v.X % by), (uint)(v.Y % by), (uint)(v.Z % by));
        }

        public static uint DistanceSquared(Vector3I value1, Vector3I value2)
        {
            var x = value1.X - value2.X;
            var y = value1.Y - value2.Y;
            var z = value1.Z - value2.Z;

            return (x * x) + (y * y) + (z * z);
        }

        public override int GetHashCode()
        {
            //TODO check this hashcode impl           
            return (((int) X * 251) + (int) Y) * 251 + (int) Z;
        }

        public override string ToString()
        {
            return ("vector3i (" + X + "," + Y + "," + Z + ")");
        }

        public static implicit operator Vector3(Vector3I value)
        {
            return new Vector3(value.X, value.Y, value.Z);
        }

        internal Vector3I Add(SignedVector3I sv)
        {
            return new Vector3I(X + (uint)sv.X, Y + (uint)sv.Y, Z + (uint)sv.Z);
        }
    }
}