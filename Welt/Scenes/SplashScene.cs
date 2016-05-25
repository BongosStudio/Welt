#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Welt.Controllers;
using Welt.UI;
<<<<<<< HEAD
using Welt.UI.Components;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt.Scenes
{
    public class SplashScene : Scene
    {
        private const string SPLASH = "Images/splashscreen";
        private float _o;

        protected override Color BackColor => Color.GhostWhite;

        public SplashScene(Game game) : base(game)
        {
<<<<<<< HEAD
            AddComponent(new ImageComponent(SPLASH, "splash", game.GraphicsDevice)
            {
                Opacity = 0f,
                HorizontalAlignment = HorizontalAlignment.Left
=======
            AddComponent(new ImageComponent(SPLASH, "splash", -1, -1, game.GraphicsDevice)
            {
                Opacity = 0f,
                HorizontalAlignment = HorizontalAlignment.Center
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            });
            Schedule(() =>
            {
                SceneController.Load(new MainMenuScene(game));
            }, TimeSpan.FromSeconds(5));

            var song = game.Content.Load<Song>("Music/feather");
<<<<<<< HEAD
            //MediaPlayer.Play(song);
        }

        public override void Initialize()
        {
            base.Initialize();
            GetComponent("splash").Value.FadeIn(this, TimeSpan.FromSeconds(2));
=======
            MediaPlayer.Play(song);
        }
        
        public override void Update(GameTime time)
        {
            base.Update(time);
            if (_o < 1f)
            {
                GetComponent("splash").Value.SetPropertyValue(UIComponent.OpacityProperty, _o);
                _o += 0.02f;
            }
            
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }
    }
}
