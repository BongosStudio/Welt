#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Welt.Controllers;
using Welt.Managers;
using Welt.Models;
using Welt.Scenes;

namespace Welt
{
    public class WeltGame : Game
    {
        #region Fields

        private SceneController _sceneController;
        private readonly GraphicsDeviceManager _graphics;
        
        public static bool ThrowExceptions = true;
        public static WeltGame Instance;
        public static int Width;
        public static int Height;

        internal static float WidthViewRatio = 1f;
        internal static float HeightViewRatio = 1f;

        public readonly Player Player;

        #endregion
        
        public WeltGame(string username, string key)
        {
            Instance = this;

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 750
            };

            Content.RootDirectory = "Content";
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime
            Player.CreatePlayer(username, key);
        }

        #region Initialize
        
        protected override void Initialize()
        {
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
            SceneController.Initialize(_graphics, new SplashScene(this));
            AudioManager.Initialize(this);
            
            IsMouseVisible = true;
            
            base.Initialize();
        }
        
        #endregion
        
        #region Update
      
        protected override void Update(GameTime gameTime)
        {
            SceneController.Update(gameTime);
            base.Update(gameTime);
           
        }
       
        #endregion

        #region Draw
        
        protected override void Draw(GameTime gameTime)
        {
            SceneController.Draw(gameTime);
            base.Draw(gameTime);
        }
        
        #endregion

        public static void SetCursor(Cursor cursor)
        {
            Instance.__SetCursor(cursor);
        }

        protected void __SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
        }
    }
}
