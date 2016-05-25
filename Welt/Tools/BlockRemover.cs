﻿#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.Forge;
using Welt.Models;

namespace Welt.Tools
{
    public class BlockRemover : Tool
    {

        public BlockRemover(Player player) : base(player){}

        public override void Use(DateTime time)
        {
            if (!IsCooledDown) return;
            if (Player.CurrentSelection.HasValue)
            {
                Player.World.SetBlock(Player.CurrentSelection.Value.Position, new Block(BlockType.None));
            }
        
        }

        public override void SwitchType(int delta) { }

    }
}