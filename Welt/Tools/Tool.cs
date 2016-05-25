#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Welt.Models;

namespace Welt.Tools
{
    public abstract class Tool
    {
        protected Player Player;
        protected virtual int CoolDown { get; }
        protected bool IsCooledDown;
        private DateTime _mCoolDownReset;
        
        protected Tool(Player player, int cooldown)
        {
            this.Player = player;
            CoolDown = cooldown;
            IsCooledDown = true;
        }

        protected Tool(Player player) : this(player, 5000)
        {
            
        }

        public virtual void Use(DateTime time)
        {
            IsCooledDown = _mCoolDownReset <= time;
            if (!IsCooledDown) return;
            _mCoolDownReset = time.AddMilliseconds(CoolDown);
        }
        public abstract void SwitchType(int delta );

    }
}
