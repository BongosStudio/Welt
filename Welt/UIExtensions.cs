#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using EmptyKeys.UserInterface.Controls;
using Microsoft.Xna.Framework;

namespace Welt
{
    public static class UIExtensions
    {
        public static void Update(this UIRoot root, GameTime time)
        {
            root.UpdateInput(time.ElapsedGameTime.TotalMilliseconds);
            root.UpdateLayout(time.ElapsedGameTime.TotalMilliseconds);
        }

        public static void Draw(this UIRoot root, GameTime time)
        {
            root.Draw(time.ElapsedGameTime.TotalMilliseconds);
        }
    }
}