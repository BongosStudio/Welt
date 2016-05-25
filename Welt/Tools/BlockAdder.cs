#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class BlockAdder : Tool
    {
        private ushort _mBlockType = BlockType.Water;

        public BlockAdder(Player player) : base(player) { }

        public override void Use(DateTime time)
        {
            if (Player.CurrentSelectedAdjacent.HasValue)
            {
                Player.World.SetBlock(Player.CurrentSelectedAdjacent.Value.Position, new Block(_mBlockType));
            }
        }

        public override void SwitchType(int delta)
        {

            if (delta >= 120)
            {
                _mBlockType++;
                if (_mBlockType == BlockType.Maximum) _mBlockType = BlockType.Maximum - 1;

            }
            else if (delta <= -120)
            {
                _mBlockType--;
                if (_mBlockType == BlockType.None) _mBlockType = 1;                
            }


        }
    }
}
