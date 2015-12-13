#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class BlockRemover : Tool
    {

        public BlockRemover(Player player) : base(player){}

        public override void Use() {

            if (player.CurrentSelection.HasValue)
            {
                player.World.SetBlock(player.CurrentSelection.Value.Position, new Block(BlockType.None));
            }
        
        }

        public override void switchType(int delta) { }

    }
}
