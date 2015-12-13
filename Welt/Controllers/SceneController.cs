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
        private Scene _current;
        public GraphicsDeviceManager GraphicsManager;

        public SceneController(Game game, GraphicsDeviceManager gdm) : base(game)
        {
            GraphicsManager = gdm;
            Scene.Controller = this;
            
        }

        public void HandleExiting(object sender, EventArgs args)
        {
            _current.OnExiting(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            _current.Update(gameTime);
            base.Update(gameTime);
        }

        public new void Draw(GameTime gameTime)
        {
            _current.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void Load(Scene scene)
        {
            _current?.Dispose();
            _current = scene;
            _current?.Initialize();
        }
    }
}