#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Microsoft.Xna.Framework;

namespace Welt.Types
{

    public struct Vector3I
    {
        public readonly uint X;
        public readonly uint Y;
        public readonly uint Z;

        public Vector3I(uint x, uint y, uint z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static implicit operator Vector3I(Vector3 value)
        {
            return new Vector3I((uint) value.X, (uint) value.Y, (uint) value.Z);
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector3I)
            {
                var other = (Vector3I)obj;
                return this.X == other.X && this.Y == other.Y && this.Z == other.Z;
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
            return new Vector3I(a.X + b.X ,a.Y +b.Y ,a.Z + b.Z);
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
            return (int)(X ^ Y ^ Z);
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
