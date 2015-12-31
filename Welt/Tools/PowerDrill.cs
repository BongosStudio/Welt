#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{

    //removes an entire column of blocks, just an example of what can be done with tools and how easy this is !
    // just assign it on one of the player 's tools in player class (   LeftTool = new PowerDrill(this); in player.cs) 
    public class PowerDrill : Tool
    {
        public PowerDrill(Player player) : base(player) { }

        public override void Use()
        {
            if (Player.CurrentSelection.HasValue)
            {
                var position =  Player.CurrentSelection.Value.Position;

                for (var y = position.Y; y > 0; y--)
                {
                    Player.World.SetBlock(position.X,y,position.Z, new Block(BlockType.None));
                }
                    
            }
        }

        public override void SwitchType(int delta) { }
    }
}
