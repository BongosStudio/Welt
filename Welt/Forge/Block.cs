#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System.CodeDom;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Types;
using Welt.Models;
using Welt.Forge.BlockProviders;

namespace Welt.Forge
{
    public class BlockType
    {
        public const ushort NONE = 0;
        public const ushort DIRT = 1;
        public const ushort GRASS = 2;
        public const ushort LAVA = 3;
        public const ushort LEAVES = 4;
        public const ushort ROCK = 5;
        public const ushort SAND = 6;
        public const ushort LOG = 7;
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

        public static byte GetStackSize(ushort id)
        {
            return 64;
        }

        public static Vector3B GetLightLevel(ushort id, byte metadata)
        {
            switch (id)
            {
                case BlockType.LAVA:
                    return new Vector3B(14, 0, 0);
                case BlockType.TORCH:
                    return TorchBlockProvider.GetLightLevel(metadata);
                default:
                    return new Vector3B();
            }
        }

        public static Vector3B GetLightLevel(BlockStack blocks)
        {
            return GetLightLevel(blocks.Block.Id, blocks.Block.Metadata);
        }

        public static BoundingBox GetBoundingBox(ushort id, Vector3 position)
        {
            switch (id)
            {
                case BlockType.SNOW:
                    return new BoundingBox(position, position + new Vector3(1, 0.1f, 1));
                case BlockType.TORCH:
                case BlockType.RED_FLOWER:
                    return new BoundingBox(position + new Vector3(0.4f, 0, 0.4f), position + new Vector3(0.6f));
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
            switch (type)
            {
                case BlockType.NONE:
                case BlockType.WATER:
                case BlockType.SNOW:
                case BlockType.RED_FLOWER:
                case BlockType.LONG_GRASS:
                case BlockType.TORCH:
                    return false;
                default:
                    return true;
            }
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
            switch (type)
            {
                case BlockType.NONE:
                case BlockType.WATER:
                case BlockType.LEAVES:
                case BlockType.SNOW:
                case BlockType.RED_FLOWER:
                case BlockType.LONG_GRASS:
                case BlockType.TORCH:
                    return true;
                default:
                    return false;
            }
        }

        public static bool WillForceRenderSide(ushort type, BlockFaceDirection face, ushort blockAtFace)
        {
            switch(type)
            {
                case BlockType.LEAVES:
                    return true;
                case BlockType.WATER:
                    return face == BlockFaceDirection.YIncreasing && blockAtFace != BlockType.WATER;
                case BlockType.LOG:
                    return IsTransparentBlock(blockAtFace);
                default:
                    return false;
            }
        }

        public static bool CanPlaceAt(ushort type, ushort below, ushort occupant)
        {
            switch (type)
            {
                case BlockType.RED_FLOWER:
                case BlockType.LONG_GRASS:
                    if (occupant == BlockType.WATER) return false;
                    return IsSolidBlock(below);
                default:
                    return !IsSolidBlock(occupant);
            }
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
                case BlockType.LOG:
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