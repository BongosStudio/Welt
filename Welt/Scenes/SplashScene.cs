#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Welt.Controllers;

namespace Welt.Scenes
{
    public class SplashScene : Scene
    {
        private const string SPLASH = "Content/Images/splashscreen.png";
        protected override Color BackColor => Color.GhostWhite;


        public SplashScene(Game game) : base(game)
        {
            Schedule(() =>
            {
                SceneController.Load(new MainMenuScene(game));
            }, TimeSpan.FromSeconds(5));
        }
    }
}
