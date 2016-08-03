#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Welt.API.Forge;
using Welt.Blocks;
using Welt.Forge;
using Welt.Models;

namespace Welt.Logic.Forge
{

    // THIS IS ALL BEING MOVED. 

    
    public class BlockLogic
    {
        public static bool GetRightClick(WorldObject world, Vector3 position, Player player)
        {
            var block = world.GetBlock(position);
            
            switch (block.Id)
            {
                case BlockType.RedFlower:
                    world.SetBlock(position, new Block(BlockType.Lava));
                    return true;
                default:
                    return false;
            }

        }

        public static Vector3 DetermineTarget(WorldObject world, Vector3 original, Vector3 adjacent)
        {
            var block = world.GetBlock(original).Id;
            if (IsCapBlock(block) || IsGrassBlock(block) || IsPlantBlock(block)) return original;
            return adjacent;
            // TODO: figure a better way for this shit lol
        }


        public static byte GetStackSize(ushort id)
        {
            return 64;
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
                case BlockType.Torch:
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
                case BlockType.Snow:
                    return new BoundingBox(position, position + new Vector3(1, 0.1f, 1));
                default:
                    return new BoundingBox(position, position + new Vector3(1, 1, 1));
            }
        }

        public static BlockTexture GetTexture(ushort blockType)
        {
            return GetTexture(blockType, BlockFaceDirection.None, BlockType.None);
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
            return false;
        }

        public static bool IsSolidBlock(ushort type)
        {
            return type != BlockType.Water && type != BlockType.None && type != BlockType.Snow && type != BlockType.RedFlower && type != BlockType.LongGrass;
        }

        public static bool IsPlantBlock(ushort type)
        {
            return type == BlockType.RedFlower;
        }

        public static bool IsGrassBlock(ushort type)
        {
            return type == BlockType.LongGrass;
        }

        public static bool IsTransparentBlock(ushort type)
        {
            return type == BlockType.None || type == BlockType.Water || type == BlockType.Leaves || type == BlockType.Snow ||
                   type == BlockType.RedFlower || type == BlockType.Rock || type == BlockType.LongGrass;
        }

        public static bool IsDiggable(ushort type)
        {
            return type != BlockType.Water;
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
                case BlockType.Torch:
                    return BlockTexture.Torch;
                default:
                    return BlockTexture.Rock;
            }
        }
        #endregion
    }
}