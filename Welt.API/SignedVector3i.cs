#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;

namespace Welt.API
{

    public struct SignedVector3I
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Z;

        public SignedVector3I(int x, int y, int z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public SignedVector3I(Vector3 vector3)
        {
            X = (int)vector3.X;
            Y = (int)vector3.Y;
            Z = (int)vector3.Z;
        }

        public override bool Equals(object obj)
        {
            if (obj is SignedVector3I)
            {
                var other = (SignedVector3I)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
            }
            return base.Equals(obj);
        }

        public static bool operator ==(SignedVector3I a, SignedVector3I b)
        {
            return a.X == b.X && a.Y == b.Y && a.Z == b.Z;
        }

        public static bool operator !=(SignedVector3I a, SignedVector3I b)
        {
            return !(a.X == b.X && a.Y == b.Y && a.Z == b.Z);
        }

        public static SignedVector3I operator +(SignedVector3I a, SignedVector3I b)
        {
            return new SignedVector3I(a.X + b.X ,a.Y +b.Y ,a.Z + b.Z);
        }

        public static SignedVector3I operator *(SignedVector3I v, byte b)
        {
            return new SignedVector3I(v.X * b, v.Y * b, v.Z * b);
        }

        public static int DistanceSquared(SignedVector3I value1, SignedVector3I value2)
        {
            var x = value1.X - value2.X;
            var y = value1.Y - value2.Y;
            var z = value1.Z - value2.Z;

            return (x * x) + (y * y) + (z * z);
        }

        public override int GetHashCode()
        {
            //TODO check this hashcode impl           
            return (int)(X ^ Y ^ Z);
        }

        public override string ToString()
        {
            return ("SignedVector3i (" + X + "," + Y + "," + Z + ")");
        }

        public Vector3 AsVector3()
        {
            return new Vector3(X, Y, Z);
        }

        

    }
}
