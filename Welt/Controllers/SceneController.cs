#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Welt.Scenes;

namespace Welt.Controllers
{
    public class SceneController : DrawableGameComponent
    {
        private Scene _mCurrent;
        public GraphicsDeviceManager GraphicsManager;

        public SceneController(Game game, GraphicsDeviceManager gdm) : base(game)
        {
            GraphicsManager = gdm;
            Scene.Controller = this;
        }

        public void HandleExiting(object sender, EventArgs args)
        {
            _mCurrent.OnExiting(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            _mCurrent.Update(gameTime);
            base.Update(gameTime);
        }

        public new void Draw(GameTime gameTime)
        {
            _mCurrent.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void Load(Scene scene)
        {
            _mCurrent?.Dispose();
            _mCurrent = scene;
            _mCurrent?.Initialize();
        }
    }
}