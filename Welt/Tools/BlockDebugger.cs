#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class BlockDebugger : Tool
    {

        public BlockDebugger(Player player) : base(player) { }

        public override void Use()
        {
            System.Diagnostics.Debug.WriteLine(player.CurrentSelection);
            if (!player.CurrentSelection.HasValue) return;
            System.Diagnostics.Debug.WriteLine(player.TargetPoint);
            var b = player.CurrentSelection.Value;
            var pos = b.Position;
            var c = this.player.World.ChunkAt(pos);
            Debug(c, "current");
            /*foreach (Cardinal card in Enum.GetValues(typeof(Cardinal)))
                {
                    debug(c.GetNeighbour(card), card.ToString());
                }*/
        }

        public void Debug(Chunk c, string id)
        {
            Console.WriteLine(
                $"BlockDebugger - {id} {c?.ToString() ?? "null"} , state: {(c == null ? "" : "" + c.State)} ");
        }

        public override void switchType(int delta) { }

    }
}
