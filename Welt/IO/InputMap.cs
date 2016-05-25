#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework.Input;

namespace Welt.IO
{
    public struct InputMap
    {
        public readonly Keys[] Keys;
        public readonly Action Action;
        public TimeSpan Cooldown;
        public bool IsDown;

        public InputMap(Keys[] keys, Action action, TimeSpan cooldown)
        {
            Keys = keys;
            Action = action;
            Cooldown = cooldown;
            IsDown = false;
        }

        public void Update(TimeSpan elapsed)
        {
            if (Cooldown <= TimeSpan.Zero)
            {
                Cooldown = TimeSpan.Zero;
                return;
            }
            Cooldown -= elapsed;
        }

        public bool CanPress() => Cooldown <= TimeSpan.Zero;
    }
}