#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;

namespace Welt.Extensions
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

        public static RelayCommand CreateButtonCommand(this Action action)
        {
            return new RelayCommand((o) => 
            {
                WeltGame.Instance.Audio.ButtonSound.Play();
                action.Invoke();
            });
        }
    }
}