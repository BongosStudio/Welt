#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Welt.API.Forge;
using Welt.Core.Forge;
using Welt.Core.Services;
using Welt.Forge;
using Welt.Forge.Builders;
using Welt.Models;
using static Welt.Forge.Builders.TextureBuilder;

namespace Welt.Logic.Forge
{

    // THIS IS ALL BEING COMPLETELY REDONE. TRUST NONE OF IT.

    
    public class BlockLogic
    {
        public static bool GetRightClick(World world, Types.Vector3I position, Player player)
        {
            var block = world.GetBlock(position.X, position.Y, position.Z);
            
            switch (block.Id)
            {
                default:
                    return false;
            }

        }

        public static Vector3 DetermineTarget(World world, Types.Vector3I original, Vector3 adjacent)
        {
            var block = world.GetBlock(original.X, original.Y, original.Z).Id;
            if (IsCapBlock(block) || IsGrassBlock(block) || IsPlantBlock(block)) return original;
            return adjacent;
            // TODO: figure a better way for this shit lol
        }


        public static byte GetStackSize(ushort id)
        {
            return 64;
        }

        public static void GetLightLevel(ushort id, byte metadata, out byte red, out byte green, out byte blue)
        {
            var name = BlockService.GetBlockName(id, metadata);
            switch (name)
            {
                case "lava":
                    red = 14;
                    green = 4;
                    blue = 0;
                    return;
                case "rose":
                    red = 10;
                    green = 0;
                    blue = 0;
                    return;
                case "snow":
                    red = 1;
                    green = 1;
                    blue = 1;
                    return;
                case "water":
                    red = 0;
                    green = 1;
                    blue = 3;
                    return;
                case "torch":
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

        public static BoundingBox GetBoundingBox(ushort id, byte metadata, Vector3 position)
        {
            var block = BlockService.GetBlock(id, metadata);
            return new BoundingBox(position, position + new Vector3(block.W, block.H, block.D));
        }

        public static BlockTexture GetTexture(ushort blockType)
        {
            return GetTexture(blockType, BlockFaceDirection.None);
        }

        public static BlockTexture GetTexture(ushort blockType, BlockFaceDirection faceDir)
        {
            return GetTexture(blockType, faceDir);
        }

        public static uint GetCost(ushort type)
        {
            return 1;
        }

        public static bool IsCapBlock(ushort type)
        {
            var block = BlockService.GetBlock(type, 0);
            return block.H <= 0.1f;
        }

        public static bool IsHalfBlock(ushort type)
        {
            return false;
        }

        public static bool IsSolidBlock(ushort type)
        {
            return BlockService.GetBlock(type, 0).C;
        }

        public static bool IsPlantBlock(ushort type)
        {
            var b = BlockService.GetBlock(type, 0);
            return b.Name == "rose"; // TODO: lol
        }

        public static bool IsGrassBlock(ushort type)
        {
            var b = BlockService.GetBlock(type, 0);
            return b.Name == "grass"; // TODO: lol
        }

        public static bool IsTransparentBlock(ushort type)
        {
            return BlockService.GetBlock(type, 0).O;
        }

        public static bool IsDiggable(ushort type)
        {
            return BlockService.GetBlock(type, 0).Hrd > 0;
        }

        #region GetTexture

        /// <summary>
        /// Return the appropriate texture to render a given face of a block
        /// </summary>
        /// <param name="blockType"></param>
        /// <param name="faceDir"></param>
        /// <param name="blockAbove">Reserved for blocks which behave differently if certain blocks are above them</param>
        /// <returns></returns>
        public static BlockTexture GetTexture(ushort id, byte metadata, BlockFaceDirection faceDir, ushort blockAbove)
        {
            // all of this is lol. 
            // TODO:
            // TODO:
            // TO FUCKING DO:
            var b = BlockService.GetBlock(id, metadata).Name;
            switch (b)
            {
                case "dirt":
                    return BlockTexture.Dirt;
                case "grass_block":
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
                            return BlockTexture.Stone;
                    }
                case "lava":
                    return BlockTexture.Lava;
                case "leaves":
                    return BlockTexture.Leaves;
                case "stone":
                    return BlockTexture.Stone;
                case "sand":
                    return BlockTexture.Sand;
                case "snow":
                    return BlockTexture.Snow;
                case "wood":
                    switch (faceDir)
                    {
                        case BlockFaceDirection.XIncreasing:
                        case BlockFaceDirection.XDecreasing:
                        case BlockFaceDirection.ZIncreasing:
                        case BlockFaceDirection.ZDecreasing:
                            return BlockTexture.Wood;
                        case BlockFaceDirection.YIncreasing:
                        case BlockFaceDirection.YDecreasing:
                            return BlockTexture.WoodTop;
                        default:
                            return BlockTexture.Stone;
                    }
                case "water":
                    return BlockTexture.Water;
                case "rose":
                    return BlockTexture.Rose;
                case "long_grass":
                    return BlockTexture.Grass;
                //case "torch":
                //    return BlockTexture.Torch;
                default:
                    return BlockTexture.Stone;
            }
        }
        #endregion
    }
}