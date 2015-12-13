#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class BlockAdder : Tool
    {
        private BlockType blockType = BlockType.LongGrass;

        public BlockAdder(Player player) : base(player) { }

        public override void Use()
        {
            if (player.CurrentSelectedAdjacent.HasValue)
            {
                player.World.SetBlock(player.CurrentSelectedAdjacent.Value.Position, new Block(blockType));
            }
        }

        public override void switchType(int delta)
        {

            if (delta >= 120)
            {
                blockType++;
                if (blockType == BlockType.MAXIMUM) blockType = BlockType.MAXIMUM - 1;

            }
            else if (delta <= -120)
            {
                blockType--;
                if (blockType == BlockType.None) blockType = (BlockType)1;                
            }


        }
    }
}
