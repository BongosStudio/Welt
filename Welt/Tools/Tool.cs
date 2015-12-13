﻿#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Models;

namespace Welt.Tools
{
    public abstract class Tool
    {
        protected Player player;
        
        public Tool(Player player)
        {
            this.player = player;
        }

        public abstract void Use();

        public abstract void switchType(int delta );

    }
}
