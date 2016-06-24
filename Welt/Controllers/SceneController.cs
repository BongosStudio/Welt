#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using EmptyKeys.UserInterface.Controls;
using Microsoft.Xna.Framework;
using Welt.Scenes;

namespace Welt.Controllers
{
    public class SceneController
    {
        private static Scene _mCurrent;
        private static UIRoot _ui;
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

        public static void Initialize(GraphicsDeviceManager manager, Scene scene)
        {
            GraphicsManager = manager;
            Load(scene);
        }
        public static void Update(GameTime gameTime)
        {
            _mCurrent.Update(gameTime);
            //_ui.Update(gameTime);
        }

        public static void Draw(GameTime gameTime)
        {
            _mCurrent.Draw(gameTime);
            //_ui.Draw(gameTime);
        }

        public static void Load(Scene scene)
        {
            _mCurrent?.Dispose();
            scene.Initialize();
            _mCurrent = scene;
            //_ui = scene.UI;
        }
    }
}