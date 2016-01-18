#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;

namespace Welt.IO
{
    public class KeyAdvanceMap
    {
        // the main goal of this map is to allow all key inputs but also take into account individual 
        // key refresh rates.

        private static readonly byte[] _data = new byte[byte.MaxValue];
        private const byte MAP_REFRESH_RATE = 250;

        public static void Update(KeyboardState state)
        {
            for (byte i = 0; i < _data.Length; i++)
            {
                if (IsModifierKey((Keys) i)) continue;
                if (state.GetPressedKeys().Contains((Keys) i)) continue;
                _data[i] = 0;
            }
        }

        public static bool Process(Keys key)
        {
            if (IsModifierKey(key)) return true;
            var i = (byte) key;
            if (_data[i] == 0)
            {
                // this means the key hasn't been pressed within the refresh rate
                _data[i] = MAP_REFRESH_RATE;
                return true;
            }
            _data[i]--;
            return false;
        }

        private static bool IsModifierKey(Keys key)
        {
            return
                key == Keys.LeftShift ||
                key == Keys.RightShift ||
                key == Keys.CapsLock ||
                key == Keys.LeftControl ||
                key == Keys.RightControl ||
                key == Keys.LeftAlt ||
                key == Keys.RightAlt;
        }
    }
}