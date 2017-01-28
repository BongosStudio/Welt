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

        public static byte GetStackSize(ushort id)
        {
            return 64;
        }

        public static Vector3B GetLightLevel(ushort id, byte metadata)
        {
            return BlockProvider.GetProvider(id).GetLightLevel(metadata);
        }

        public static Vector3B GetLightLevel(BlockStack blocks)
        {
            return GetLightLevel(blocks.Block.Id, blocks.Block.Metadata);
        }

        public static BoundingBox GetBoundingBox(ushort id, byte meta, Vector3 position)
        {
            var b = BlockProvider.GetProvider(id);
            return new BoundingBox(position + b.GetBoundingBox(meta).Min, position + b.GetBoundingBox(meta).Max);
        }

        public static Vector2[] GetTexture(ushort blockType)
        {
            return GetTexture(blockType, BlockFaceDirection.Maximum, BlockType.NONE);
        }

        public static Vector2[] GetTexture(ushort blockType, BlockFaceDirection faceDir)
        {
            return GetTexture(blockType, faceDir, BlockType.NONE);
        }

        public static uint GetCost(ushort type)
        {
            return 1;
        }

        public static bool IsCapBlock(ushort type, byte meta)
        {
            return BlockProvider.GetProvider(type).GetBoundingBox(meta).Contains(
                new BoundingBox(new Vector3(0, 0.21f, 0), new Vector3(1, 1, 1))) == ContainmentType.Contains;
        }

        public static bool IsHalfBlock(ushort type, byte meta)
        {
            return BlockProvider.GetProvider(type).GetBoundingBox(meta).Contains(
                new BoundingBox(new Vector3(0, 0.51f, 0), new Vector3(1, 1, 1))) == ContainmentType.Contains;
        }

        public static bool HasCollision(ushort type)
        {
            return BlockProvider.GetProvider(type).HasCollision;
        }

        public static bool IsSolidBlock(ushort type)
        {
            return BlockProvider.GetProvider(type).IsSolid;
        }

        public static bool IsPlantBlock(ushort type)
        {
            return BlockProvider.GetProvider(type).IsPlantBlock;
        }

        public static bool IsGrassBlock(ushort type)
        {
            return type == BlockType.LONG_GRASS;
        }

        public static bool IsOpaqueBlock(ushort type)
        {
            return BlockProvider.GetProvider(type).IsOpaque;
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
                    return IsOpaqueBlock(blockAtFace);
                default:
                    return !IsSolidBlock(blockAtFace);
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
        public static Vector2[] GetTexture(ushort blockType, BlockFaceDirection faceDir, ushort blockAbove)
        {
            return BlockProvider.GetProvider(blockType).GetTexture(faceDir, blockAbove);
        }

        #endregion
    }

    #endregion
}