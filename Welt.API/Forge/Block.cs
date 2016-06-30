#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

namespace Welt.API.Forge
{
    //public enum BlockType : ushort
    //{
    //    None = 0,
    //    Dirt,
    //    Grass,
    //    Lava,
    //    Leaves,
    //    Rock,
    //    Sand,
    //    Tree,
    //    Snow,
    //    RedFlower,
    //    LongGrass,

    //    Water = 182,
    //    Maximum
    //}

    public class BlockType
    {
        public const ushort NONE = 0;
        public const ushort DIRT = 1;
        public const ushort GRASS = 2;
        public const ushort LAVA = 3;
        public const ushort LEAVES = 4;
        public const ushort ROCK = 5;
        public const ushort SAND = 6;
        public const ushort TREE = 7;
        public const ushort SNOW = 8;
        public const ushort RED_FLOWER = 9;
        public const ushort LONG_GRASS = 10;
        public const ushort TORCH = 90;
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
                hashCode = (hashCode*397) ^ Metadata;
                return hashCode;
            }
        }

        public byte R, G, B;
        public byte Sun;
        public ushort Id;
        public byte Metadata;
        // Metadata will not be implemented due to the fact that we will have over 65k block spaces. 
        // Yes, it's useful for determining groupings, but it will cut data size down by 1 byte per
        // block. That's over 65k bytes and is completely worth it if you ask me. 

        public Block(ushort blockId) : this(blockId, 0x00)
        {

        }

        public Block(ushort blockId, byte metadata)
        {
            Id = blockId;
            Sun = 0;
            R = 0;
            G = 0;
            B = 0;
            Metadata = metadata;
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Id == right.Id &&
                   left.Metadata == right.Metadata &&
                   left.R == right.R &&
                   left.G == right.G &&
                   left.B == right.B;
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }
    }

    #endregion
}