#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Controllers;
using Welt.UI;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        private readonly TextInputComponent _text;
        protected override Color BackColor => Color.GhostWhite;

        public MainMenuScene(Game game) : base(game)
        {
            _text = new TextInputComponent("Username", "username", 300, 100, game.GraphicsDevice)
            {
                Foreground = Color.Black
            };

            _text.EnterKeyPressed += (sender, args) =>
            {
                SceneController.Load(new PlayScene(game));
            };
        }

        public override void Initialize()
        {
            base.Initialize();
            _text.Initialize();
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _text.Draw(gameTime);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
            _text.Update(time);
        }

        public new void Dispose()
        {
            _text.Dispose();
            base.Dispose();
        }

        protected override void Dispose(bool disposing)
        {
            _text.Dispose();
            base.Dispose(disposing);
        }
    }
}
