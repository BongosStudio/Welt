#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using Welt.Core;

namespace Welt.Types
{
    public struct Vector3B
    {
        public readonly byte X;
        public readonly byte Y;
        public readonly byte Z;

        public Vector3B(byte x, byte y, byte z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3B(int x, int y, int z)
        {
            FastMath.Adjust(byte.MinValue, byte.MaxValue, ref x);
            FastMath.Adjust(byte.MinValue, byte.MaxValue, ref y);
            FastMath.Adjust(byte.MinValue, byte.MaxValue, ref z);
            X = (byte) x;
            Y = (byte) y;
            Z = (byte) z;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3B)
            {
                var other = (Vector3B) obj;
                return X == other.X && Y == other.Y && Z == other.Z;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(Vector3B a, Vector3B b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(Vector3B a, Vector3B b)
        {
            return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static Vector3B operator +(Vector3B a, Vector3B b)
        {
            return new Vector3B((byte) (a.X + b.X), (byte) (a.Y + b.Y), (byte) (a.Z + b.Z));
        }

        public override int GetHashCode()
        {
            //TODO check this hashcode impl - here should be ok, no overflow problem
            return (int) (X ^ Y ^ Z);
        }

        public override string ToString()
        {
            return ("vector3b (" + X + "," + Y + "," + Z + ")");
        }
    }
}