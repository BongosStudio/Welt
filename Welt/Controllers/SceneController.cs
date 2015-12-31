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
        private Scene m_current;
        public GraphicsDeviceManager GraphicsManager;

        public SceneController(Game game, GraphicsDeviceManager gdm) : base(game)
        {
            GraphicsManager = gdm;
            Scene.Controller = this;
        }

        public void HandleExiting(object sender, EventArgs args)
        {
            m_current.OnExiting(sender, args);
        }

        public override void Update(GameTime gameTime)
        {
            m_current.Update(gameTime);
            base.Update(gameTime);
        }

        public new void Draw(GameTime gameTime)
        {
            m_current.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void Load(Scene scene)
        {
            m_current?.Dispose();
            m_current = scene;
            m_current?.Initialize();
        }
    }
}