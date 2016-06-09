#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.Models;

namespace Welt.Logic.Forge
{
    public class BlockLogic
    {
        public static bool GetRightClick(World world, Vector3 position, Player player)
        {
            var block = world.GetBlock(position);
            
            switch (block.Id)
            {
                case BlockType.RED_FLOWER:
                    world.SetBlock(position, new Block(BlockType.LAVA));
                    return true;
                default:
                    return false;
            }

        }

        public static Vector3 DetermineTarget(World world, Vector3 original, Vector3 adjacent)
        {
            var block = world.GetBlock(original).Id;
            if (Block.IsCapBlock(block) || Block.IsGrassBlock(block) || Block.IsPlantBlock(block)) return original;
            return adjacent;
            // TODO: figure a better way for this shit lol
        }
    }
}