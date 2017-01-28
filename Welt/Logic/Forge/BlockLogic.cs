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
                case BlockType.FLOWER_ROSE:
                    
                    return true;
                default:
                    return false;
            }

        }

        public static Vector3 DetermineTarget(World world, Vector3 original, Vector3 adjacent)
        {
            var block = world.GetBlock(original);
            if (Block.IsCapBlock(block.Id, block.Metadata) || Block.IsGrassBlock(block.Id) || Block.IsPlantBlock(block.Id)) return original;
            return adjacent;
            // TODO: figure a better way for this shit lol
        }
    }
}