#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Welt.Scenes;

namespace Welt.Controllers
{
    public class SceneController
    {
        private static Scene _mCurrent;
        public static GraphicsDeviceManager GraphicsManager;

        public SceneController(Game game, GraphicsDeviceManager gdm)
        {
            GraphicsManager = gdm;
            Scene.Controller = this;
        }

        public void HandleExiting(object sender, EventArgs args)
        {
            _mCurrent.OnExiting(sender, args);
        }

        public void Initialize(Scene scene)
        {
            _mCurrent = scene;
            _mCurrent.Initialize();
            //_mCurrent.LoadContent();
        }

        public void Update(GameTime gameTime)
        {
            _mCurrent.Update(gameTime);
        }

        public void Draw(GameTime gameTime)
        {
            _mCurrent.Draw(gameTime);
        }
        
        public static void Load(Scene scene)
        {
            _mCurrent?.Dispose();
            _mCurrent = scene;
            _mCurrent?.Initialize();
        }
    }
}