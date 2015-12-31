#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Models;

namespace Welt.Tools
{
    public abstract class Tool
    {
        protected Player Player;
        
        public Tool(Player player)
        {
            this.Player = player;
        }

        public abstract void Use();

        public abstract void SwitchType(int delta );

    }
}
