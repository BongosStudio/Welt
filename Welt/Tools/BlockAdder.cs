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
        private ushort m_blockType = BlockType.Water;

        public BlockAdder(Player player) : base(player) { }

        public override void Use(DateTime time)
        {
            if (Player.CurrentSelectedAdjacent.HasValue)
            {
                Player.World.SetBlock(Player.CurrentSelectedAdjacent.Value.Position, new Block(m_blockType));
            }
        }

        public override void SwitchType(int delta)
        {

            if (delta >= 120)
            {
                m_blockType++;
                if (m_blockType == BlockType.Maximum) m_blockType = BlockType.Maximum - 1;

            }
            else if (delta <= -120)
            {
                m_blockType--;
                if (m_blockType == BlockType.None) m_blockType = 1;                
            }


        }
    }
}
