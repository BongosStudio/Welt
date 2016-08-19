using System;
using System.Runtime.InteropServices;

namespace Welt.API.Forge
{
    [StructLayout(LayoutKind.Explicit, Pack = 0, Size = 24)]
    public struct LightStruct
    {
        [FieldOffset(0)]
        public byte R;
        [FieldOffset(1)]
        public byte G;
        [FieldOffset(2)]
        public byte B;

        public LightStruct(byte r, byte g, byte b)
        {
            R = r;
            G = g;
            B = b;
        }

        public LightStruct(int r, int g, int b)
        {
            R = (byte) r;
            G = (byte) g;
            B = (byte) b;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is LightStruct)) return false;
            var o = (LightStruct) obj;
            return R == o.R && G == o.G && B == o.B;
        }

        public bool Equals(LightStruct other)
        {
            return R == other.R && G == other.G && B == other.B;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode*397) ^ G.GetHashCode();
                hashCode = (hashCode*397) ^ B.GetHashCode();
                return hashCode;
            }
        }

        public static LightStruct operator +(LightStruct left, LightStruct right)
        {
            return new LightStruct(left.R + right.R, left.G + right.G, left.B + right.B);
        }

        public static LightStruct operator -(LightStruct left, LightStruct right)
        {
            return new LightStruct(left.R - right.R, left.G - right.G, left.B - right.B);
        }

        public static LightStruct operator +(LightStruct left, int right)
        {
            return new LightStruct(left.R + right, left.G + right, left.B + right);
        }

        public static LightStruct operator -(LightStruct left, int right)
        {
            return new LightStruct(left.R - right, left.G - right, left.B - right);
        }

        public static explicit operator LightStruct(ushort value)
        {
            var r = value & 0xF;
            var g = value >> 4 & 0xF;
            var b = value >> 8 & 0xF;
            return new LightStruct(r, g, b);
        }

        public static explicit operator ushort(LightStruct value)
        {
            var lite = 0;
            lite |= value.R;
            lite |= (ushort)(value.G << 4);
            lite |= (ushort)(value.B << 8);
            return (ushort) lite;
        }
    }
}