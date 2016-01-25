#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.UI;

namespace Welt.Scenes
{
    public class SplashScene : Scene
    {
        private const string SPLASH = "Images/splashscreen";
        protected override Color BackColor => Color.GhostWhite;

        public SplashScene(Game game) : base(game)
        {
            AddComponent(new ImageComponent(SPLASH, "splash", -1, -1, game.GraphicsDevice)
            {
                Opacity = 0f
            });
            Schedule(() =>
            {
                SceneController.Load(new MainMenuScene(game));
            }, TimeSpan.FromSeconds(5));
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            if (GetComponent("splash").Opacity < 1f)
                GetComponent("splash")
                    .SetPropertyValue(UIComponent.OpacityProperty,
                        (float) GetComponent("splash").GetPropertyValue(UIComponent.OpacityProperty) + 0.05f);
        }
    }
}
