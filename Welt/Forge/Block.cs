#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System.CodeDom;
using Microsoft.Xna.Framework;
using Welt.Blocks;

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

        public static byte GetStackSize(ushort id)
        {
            return 64;
        }

        public static void GetLightLevel(ushort id, out byte red, out byte green, out byte blue)
        {
            switch (id)
            {
                case BlockType.LAVA:
                    red = 14;
                    green = 4;
                    blue = 0;
                    return;
                case BlockType.RED_FLOWER:
                    red = 10;
                    green = 0;
                    blue = 0;
                    return;
                case BlockType.SNOW:
                    red = 1;
                    green = 1;
                    blue = 1;
                    return;
                case BlockType.WATER:
                    red = 0;
                    green = 1;
                    blue = 3;
                    return;
                case BlockType.TORCH:
                    red = 10;
                    green = 0;
                    blue = 10;
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
                case BlockType.SNOW:
                    return new BoundingBox(position, position + new Vector3(1, 0.1f, 1));
                default:
                    return new BoundingBox(position, position + new Vector3(1, 1, 1));
            }
        }

        public static BlockTexture GetTexture(ushort blockType)
        {
            return GetTexture(blockType, BlockFaceDirection.Maximum, BlockType.NONE);
        }

        public static BlockTexture GetTexture(ushort blockType, BlockFaceDirection faceDir)
        {
            return GetTexture(blockType, faceDir, BlockType.NONE);
        }

        public static uint GetCost(ushort type)
        {
            return 1;
        }

        public static bool IsCapBlock(ushort type)
        {
            if (type == BlockType.SNOW) return true;
            return false;
        }

        public static bool IsHalfBlock(ushort type)
        {
            return false;
        }

        public static bool IsSolidBlock(ushort type)
        {
            return type != BlockType.WATER && type != BlockType.NONE && type != BlockType.SNOW && type != BlockType.RED_FLOWER && type != BlockType.LONG_GRASS;
        }

        public static bool IsPlantBlock(ushort type)
        {
            return type == BlockType.RED_FLOWER;
        }

        public static bool IsGrassBlock(ushort type)
        {
            return type == BlockType.LONG_GRASS;
        }

        public static bool IsTransparentBlock(ushort type)
        {
            return type == BlockType.NONE || type == BlockType.WATER || type == BlockType.LEAVES || type == BlockType.SNOW ||
                   type == BlockType.RED_FLOWER || type == BlockType.ROCK || type == BlockType.LONG_GRASS;
        }

        public static bool IsDiggable(ushort type)
        {
            return type != BlockType.WATER;
        }

        #region GetTexture

        /// <summary>
        /// Return the appropriate texture to render a given face of a block
        /// </summary>
        /// <param name="blockType"></param>
        /// <param name="faceDir"></param>
        /// <param name="blockAbove">Reserved for blocks which behave differently if certain blocks are above them</param>
        /// <returns></returns>
        public static BlockTexture GetTexture(ushort blockType, BlockFaceDirection faceDir, ushort blockAbove)
        {
            switch (blockType)
            {
                case BlockType.DIRT:
                    return BlockTexture.Dirt;
                case BlockType.GRASS:
                    switch (faceDir)
                    {
                        case BlockFaceDirection.XIncreasing:
                        case BlockFaceDirection.XDecreasing:
                        case BlockFaceDirection.ZIncreasing:
                        case BlockFaceDirection.ZDecreasing:
                            return BlockTexture.GrassSide;
                        case BlockFaceDirection.YIncreasing:
                            return BlockTexture.GrassTop;
                        case BlockFaceDirection.YDecreasing:
                            return BlockTexture.Dirt;
                        default:
                            return BlockTexture.Rock;
                    }
                case BlockType.LAVA:
                    return BlockTexture.Lava;
                case BlockType.LEAVES:
                    return BlockTexture.Leaves;
                case BlockType.ROCK:
                    return BlockTexture.Rock;
                case BlockType.SAND:
                    return BlockTexture.Sand;
                case BlockType.SNOW:
                    return BlockTexture.Snow;
                case BlockType.TREE:
                    switch (faceDir)
                    {
                        case BlockFaceDirection.XIncreasing:
                        case BlockFaceDirection.XDecreasing:
                        case BlockFaceDirection.ZIncreasing:
                        case BlockFaceDirection.ZDecreasing:
                            return BlockTexture.TreeHorizontal;
                        case BlockFaceDirection.YIncreasing:
                        case BlockFaceDirection.YDecreasing:
                            return BlockTexture.TreeVertical;
                        default:
                            return BlockTexture.Rock;
                    }
                case BlockType.WATER:
                    return BlockTexture.Water;
                case BlockType.RED_FLOWER:
                    return BlockTexture.Rose;
                case BlockType.LONG_GRASS:
                    return BlockTexture.Grass;
                case BlockType.TORCH:
                    return BlockTexture.Torch;
                default:
                    return BlockTexture.Rock;
            }
        }

        #endregion
    }

    #endregion
}