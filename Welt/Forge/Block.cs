#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System.CodeDom;
using Microsoft.Xna.Framework;

namespace Welt.Forge
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
        public const ushort None = 0;
        public const ushort Dirt = 1;
        public const ushort Grass = 2;
        public const ushort Lava = 3;
        public const ushort Leaves = 4;
        public const ushort Rock = 5;
        public const ushort Sand = 6;
        public const ushort Tree = 7;
        public const ushort Snow = 8;
        public const ushort RedFlower = 9;
        public const ushort LongGrass = 10;
        public const ushort Water = 182;
        public const ushort Maximum = ushort.MaxValue;
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
        // Metadata will not be implemented due to the fact that we will have over 65k block spaces. 
        // Yes, it's useful for determining groupings, but it will cut data size down by 1 byte per
        // block. That's over 65k bytes and is completely worth it if you ask me. 

        public Block(ushort blockId)
        {
            Id = blockId;
            Sun = 0;
            R = 0;
            G = 0;
            B = 0;
        }

        public static bool operator ==(Block left, Block right)
        {
            return left.Id == right.Id;
        }

        public static bool operator !=(Block left, Block right)
        {
            return !(left == right);
        }

        public static void GetLightLevel(ushort id, out byte red, out byte green, out byte blue)
        {
            switch (id)
            {
                case BlockType.Lava:
                    red = 14;
                    green = 4;
                    blue = 0;
                    return;
                case BlockType.RedFlower:
                    red = 10;
                    green = 0;
                    blue = 0;
                    return;
                case BlockType.Snow:
                    red = 1;
                    green = 1;
                    blue = 1;
                    return;
                case BlockType.Water:
                    red = 0;
                    green = 1;
                    blue = 3;
                    return;
                default:
                    red = 0;
                    green = 0;
                    blue = 0;
                    return;
            }
        }

        public static BoundingBox GetBoundingBox(ushort id, Vector3 position)
        {
            switch (id)
            {
                case BlockType.Snow:
                    return new BoundingBox(position, position + new Vector3(1, 0.1f, 1));
                default:
                    return new BoundingBox(position, position + new Vector3(1, 1, 1));
            }
        }
    }

    #endregion
}