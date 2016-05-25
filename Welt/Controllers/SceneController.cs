#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Windows.Forms;
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

        public static void HandleExiting(object sender, EventArgs args)
        {
            _mCurrent.OnExiting(sender, args);
        }

        public static void Initialize(GraphicsDeviceManager manager, Scene scene)
        {
            GraphicsManager = manager;
            _mCurrent = scene;
            _mCurrent.Initialize();
        }

        public static void Update(GameTime gameTime)
        {
            _mCurrent.Update(gameTime);
        }

        public static void Draw(GameTime gameTime)
        {
            _mCurrent.Draw(gameTime);
        }
        
        public static void Load(Scene scene)
        {
            _mCurrent?.Dispose();
            //WeltGame.SetCursor(Cursors.Arrow);
            scene.Initialize();
            _mCurrent = scene;
        }
    }
}