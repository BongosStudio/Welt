#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements



#endregion

using Microsoft.Xna.Framework;
using System.Linq;

namespace Welt.Types
{

    public struct Vector3B
    {
        public byte X;
        public byte Y;
        public byte Z;

        public Vector3B(byte x, byte y, byte z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3B)
            {
                var other = (Vector3B)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(Vector3B a, Vector3B b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3B a, Vector3B b)
        {
            return a.X != b.X || a.Y != b.Y || a.Z != b.Z;
        }

        public static bool operator ==(Vector3B v, byte value)
        {
            return v.X == value && v.Y == value && v.Z == value;
        }

        public static bool operator !=(Vector3B v, byte value)
        {
            return v.X != value || v.Y != value || v.Z != value;
        }

        public static bool operator >(Vector3B a, Vector3B b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }

        public static bool operator <(Vector3B a, Vector3B b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        public static bool operator >=(Vector3B a, Vector3B b)
        {
            return a > b || a == b;
        }

        public static bool operator <=(Vector3B a, Vector3B b)
        {
            return a < b || a == b;
        }

        public static bool operator >(Vector3B v, byte value)
        {
            return v.X > value && v.Y > value && v.Z > value;
        }

        public static bool operator <(Vector3B v, byte value)
        {
            return v.X < value && v.Y < value && v.Z < value;
        }

        public static bool operator >=(Vector3B v, byte value)
        {
            return v > value || v == value;
        }

        public static bool operator <=(Vector3B v, byte value)
        {
            return v < value || v == value;
        }

        public static Vector3B operator +(Vector3B a, Vector3B b)
        {
            return new Vector3B((byte)(a.X + b.X), (byte)(a.Y + b.Y), (byte)(a.Z + b.Z));
        }

        public static explicit operator Color(Vector3B v)
        {
            return new Color(v.X, v.Y, v.Z);
        }

        public static Vector3B Average(params Vector3B[] values)
        {
            var x = 0;
            var y = 0;
            var z = 0;
            foreach (var value in values)
            {
                x += value.X;
                y += value.Y;
                z += value.Z;
            }

            return new Vector3B((byte)(x / values.Length), (byte)(y / values.Length), (byte)(z / values.Length));
        }
       
        public override int GetHashCode()
        {
            //TODO check this hashcode impl - here should be ok, no overflow problem           
            return (int)(X ^ Y ^ Z);
        }

        public override string ToString()
        {
            return ("vector3b (" + X + "," + Y + "," + Z + ")");
        }

    }
}
