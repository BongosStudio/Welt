#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.Models;
using Welt.Types;

namespace Welt.Tools
{
    public class TorchTool : Tool
    {
        public TorchTool(Player player, int cooldown) : base(player, cooldown)
        {
        }

        public TorchTool(Player player) : base(player)
        {
        }

        public override void SwitchType(int delta)
        {
            
        }

        public override void Use(DateTime time)
        {
            if (!Player.CurrentSelectedAdjacent.HasValue) return;
            var x = Player.CurrentSelectedAdjacent.Value.Position.X;
            var y = Player.CurrentSelectedAdjacent.Value.Position.Y;
            var z = Player.CurrentSelectedAdjacent.Value.Position.Z;
            Player.World.SetLightAt(x, y, z, new Vector3B(10, 10, 10));
        }
    }
}