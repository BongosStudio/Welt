#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge;

namespace Welt.Forge
{
    public static class BlockLogic
    {
        public static byte GetStackSize(ushort id)
        {
            return 64;
        }

        public static Vector3B GetLightLevel(ushort id, byte metadata)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(id).GetLightLevel(metadata);

        }

        public static Vector3B GetLightLevel(ItemStack blocks)
        {
            return GetLightLevel(blocks.Block.Id, blocks.Block.Metadata);
        }

        public static BoundingBox? GetBoundingBox(ushort id, byte meta, Vector3 position)
        {
            var b = BlockProvider.BlockRepository.GetBlockProvider(id);
            if (!b.GetBoundingBox(meta).HasValue) return null;
            return new BoundingBox(position + b.GetBoundingBox(meta).Value.Min, position + b.GetBoundingBox(meta).Value.Max);
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
            return BlockProvider.BlockRepository.GetBlockProvider(type).GetBoundingBox(meta).Value.Contains(
                new BoundingBox(new Vector3(0, 0.21f, 0), new Vector3(1, 1, 1))) == ContainmentType.Contains;
        }

        public static bool IsHalfBlock(ushort type, byte meta)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(type).GetBoundingBox(meta).Value.Contains(
                new BoundingBox(new Vector3(0, 0.51f, 0), new Vector3(1, 1, 1))) == ContainmentType.Contains;
        }

        public static bool HasCollision(ushort type)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(type).IsSolid;
        }

        public static bool IsSolidBlock(ushort type)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(type).IsSolid;
        }
        
        public static bool IsOpaqueBlock(ushort type)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(type).IsOpaque;
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
            return BlockProvider.BlockRepository.GetBlockProvider(blockType).GetTexture(faceDir, 0, blockAbove);
        }

        #endregion
    }
}