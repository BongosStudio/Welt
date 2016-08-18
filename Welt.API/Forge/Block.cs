#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;

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
        public const ushort Torch = 90;
        public const ushort Water = 182;
        public const ushort Maximum = ushort.MaxValue;
    }

    #region Block structure

    public abstract class Block
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
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode*397) ^ Metadata;
                return hashCode;
            }
        }

        // NOTE: change this whole class to BlockAttribute and just have blocks made as
        // [Block(2, 0x0, "stone", Hardness = 0.5f, Opaque = false, Light = new LightPact(0, 0, 0))]
        // public static object StoneObject;

        public static List<Type> RegisteredTypes = new List<Type>();
        public readonly ushort Id;
        public readonly byte Metadata;

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
            if (!RegisteredTypes.Contains(GetType())) RegisteredTypes.Add(GetType());
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