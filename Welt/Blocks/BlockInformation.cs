#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Forge;

namespace Welt.Blocks
{
    public class BlockInformation
    {
        public static BlockTexture GetTexture(ushort blockType)
        {
            return GetTexture(blockType, BlockFaceDirection.Maximum, BlockType.None);
        }

        public static BlockTexture GetTexture(ushort blockType, BlockFaceDirection faceDir)
        {
            return GetTexture(blockType, faceDir, BlockType.None);
        }

        public static uint GetCost(ushort type)
        {
            return 1;
        }

        public static bool IsCapBlock(ushort type)
        {
            if (type == BlockType.Snow) return true;
            return false;
        }

        public static bool IsHalfBlock(ushort type)
        {
            if (type == BlockType.Rock) return true;
            return false;
        }

        public static bool IsSolidBlock(ushort type)
        {
            if (type == BlockType.Water || type == BlockType.None || type == BlockType.Snow ||
                type == BlockType.RedFlower || type == BlockType.LongGrass) return false;
            return true;
        }

        public static bool IsPlantBlock(ushort type)
        {
            if (type == BlockType.RedFlower) return true;
            return false;
        }

        public static bool IsGrassBlock(ushort type)
        {
            if (type == BlockType.LongGrass) return true;
            return false;
        }

        public static bool IsTransparentBlock(ushort type)
        {
            if (type == BlockType.None || type == BlockType.Water || type == BlockType.Leaves || type == BlockType.Snow ||
                type == BlockType.RedFlower || type == BlockType.Rock || type == BlockType.LongGrass) return true;
            return false;
        }

        public static bool IsDiggable(ushort type)
        {
            if (type == BlockType.Water) return false;
            return true;
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
                case BlockType.Dirt:
                    return BlockTexture.Dirt;
                case BlockType.Grass:
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
                case BlockType.Lava:
                    return BlockTexture.Lava;
                case BlockType.Leaves:
                    return BlockTexture.Leaves;
                case BlockType.Rock:
                    return BlockTexture.Rock;
                case BlockType.Sand:
                    return BlockTexture.Sand;
                case BlockType.Snow:
                    return BlockTexture.Snow;
                case BlockType.Tree:
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
                case BlockType.Water:
                    return BlockTexture.Water;
                case BlockType.RedFlower:
                    return BlockTexture.Rose;
                case BlockType.LongGrass:
                    return BlockTexture.Grass;
                default:
                    return BlockTexture.Rock;
            }
        }

        #endregion
    }
}