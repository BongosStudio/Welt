#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.UI;

namespace Welt.Scenes
{
    public class SplashScene : Scene
    {
        private const string SPLASH = "Images/splashscreen";
        private float _o;
        private BoundsBox _box;
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
            if (_o < 1f)
            {
                GetComponent("splash").SetPropertyValue(UIComponent.OpacityProperty, _o);
                _o += 0.02f;
            }
            
        }
    }
}
