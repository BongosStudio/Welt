#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
<<<<<<< HEAD
using System.Windows.Forms;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
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

<<<<<<< HEAD
        public static void HandleExiting(object sender, EventArgs args)
=======
        public void HandleExiting(object sender, EventArgs args)
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        {
            _mCurrent.OnExiting(sender, args);
        }

<<<<<<< HEAD
        public static void Initialize(GraphicsDeviceManager manager, Scene scene)
        {
            GraphicsManager = manager;
=======
        public void Initialize(Scene scene)
        {
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            _mCurrent = scene;
            _mCurrent.Initialize();
        }

<<<<<<< HEAD
        public static void Update(GameTime gameTime)
=======
        public void Update(GameTime gameTime)
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        {
            _mCurrent.Update(gameTime);
        }

<<<<<<< HEAD
        public static void Draw(GameTime gameTime)
=======
        public void Draw(GameTime gameTime)
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        {
            _mCurrent.Draw(gameTime);
        }
        
        public static void Load(Scene scene)
        {
            _mCurrent?.Dispose();
<<<<<<< HEAD
            //WeltGame.SetCursor(Cursors.Arrow);
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            scene.Initialize();
            _mCurrent = scene;
        }
    }
}