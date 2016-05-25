#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;

namespace Welt.UI.Framework
{
    public static class UISystem
    {
        private static Game _game;

        public static void Create(Game game)
        {
            _game = game;
        }
    }
}