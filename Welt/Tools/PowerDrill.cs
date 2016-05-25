#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class PowerDrill : Tool
    {
        public PowerDrill(Player player) : base(player) { }

        public override void Use(DateTime time)
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
