#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion


namespace Welt.API.Forge
{
    public class BlockType
    {
        public const ushort NONE = 0;
        public const ushort DIRT = 1;
        public const ushort GRASS = 2;
        public const ushort LAVA = 3;
        public const ushort LEAVES = 4;
        public const ushort STONE = 5;
        public const ushort SAND = 6;
        public const ushort LOG = 7;
        public const ushort SNOW = 8;
        public const ushort FLOWER_ROSE = 9;
        public const ushort LONG_GRASS = 10;
        public const ushort TORCH = 90;
        public const ushort LADDER = 91;
        public const ushort WATER = 182;
        public const ushort MAXIMUM = ushort.MaxValue;
    }

    #region Block structure

    public struct Block
    {
        public bool Equals(Block other)
        {
            return R == other.R && G == other.G && B == other.B && Sun == other.Sun && Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Block && Equals((Block) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = R.GetHashCode();
                hashCode = (hashCode*397) ^ G.GetHashCode();
                hashCode = (hashCode*397) ^ B.GetHashCode();
                hashCode = (hashCode*397) ^ Sun.GetHashCode();
                hashCode = (hashCode*397) ^ Id;
                return hashCode;
            }
        }

        public byte R, G, B;
        public byte Sun;
        public ushort Id;
        public byte Metadata;

        public Block(ushort blockId) : this(blockId, 0)
        {
            
        }

        public Block(ushort blockId, byte metadata) : this(blockId, metadata, 0, 0, 0, 0)
        {
            
        }

        public Block(ushort blockId, byte metadata, byte r, byte g, byte b, byte s)
        {
            Id = blockId;
            Metadata = metadata;
            R = r;
            G = g;
            B = b;
            Sun = s;
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Id == right.Id;
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }
    }

    #endregion
}