#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
<<<<<<< HEAD
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Controllers;
using Welt.Models;
using Welt.Scenes;
using Welt.UI;
=======
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.Scenes;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt
{
    public class WeltGame : Game
    {
        #region Fields
<<<<<<< HEAD
        
=======

        private SceneController _sceneController;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        private readonly GraphicsDeviceManager _graphics;
        
        public static bool ThrowExceptions = true;
        public static WeltGame Instance;
<<<<<<< HEAD
        public static int Width;
        public static int Height;

        internal static float WidthViewRatio = 1f;
        internal static float HeightViewRatio = 1f;

        public readonly Player Player;

        #endregion

        public WeltGame(string username, string key)
=======

        #endregion

        public WeltGame()
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        {          
            Instance = this;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 750
            };

            Content.RootDirectory = "Content";
<<<<<<< HEAD
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime
            Player.CreatePlayer(username, key);
=======
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime 
            Console.WriteLine(Color.Black);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }

        #region Initialize
        
        protected override void Initialize()
        {
<<<<<<< HEAD
            SceneController.Initialize(_graphics, new SplashScene(this));
            
            IsMouseVisible = true;
            Window.ClientSizeChanged += (sender, args) =>
            {
                WidthViewRatio = (float) Width/Window.ClientBounds.Width;
                HeightViewRatio = (float) Height/Window.ClientBounds.Height;
                Height = Window.ClientBounds.Height;
                Width = Window.ClientBounds.Width;
                GraphicsDevice.Viewport = new Viewport(0, 0, Width, Height, 0, 1);
            };
            base.Initialize();
            Height = Window.ClientBounds.Height;
            Width = Window.ClientBounds.Width;
            SetCursor(Cursors.Default);
=======
            _sceneController = new SceneController(this, _graphics);
            _sceneController.Initialize(new SplashScene(this));
            
            IsMouseVisible = true;
            
            base.Initialize();
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }
        
        #endregion
        
        #region Update
      
        protected override void Update(GameTime gameTime)
        {
<<<<<<< HEAD
            SceneController.Update(gameTime);
=======
            _sceneController.Update(gameTime);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            base.Update(gameTime);
           
        }
       
        #endregion

        #region Draw
        
        protected override void Draw(GameTime gameTime)
        {
<<<<<<< HEAD
            SceneController.Draw(gameTime);
=======
            _sceneController.Draw(gameTime);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            base.Draw(gameTime);
        }
        
        #endregion

        public static void SetCursor(Cursor cursor)
        {
<<<<<<< HEAD
            Instance.__SetCursor(cursor);
        }

        protected void __SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
            
=======
            ((Form) Control.FromHandle(Instance.Window.Handle)).Cursor = cursor;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }
    }
}
