#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Themes;
using GameUILibrary.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Welt.Console;
using Welt.Controllers;
using Welt.Forge;
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
        private static UIRoot _m;
        private static ViewModelBase _v;

        public static bool ThrowExceptions = true;
        public static WeltGame Instance;
        public static int Width;
        public static int Height;

        internal static float WidthViewRatio = 1f;
        internal static float HeightViewRatio = 1f;
        


        #endregion

        public WeltGame(string username, string key)
        {
            Instance = this;

            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreparingDeviceSettings += graphics_PreparingDeviceSettings;
            _graphics.DeviceCreated += graphics_DeviceCreated;

            Content.RootDirectory = "Content";
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime
            Player.CreatePlayer(username, key);
        }

        protected override void LoadContent()
        {
            FontManager.Instance.LoadFonts(Content, "Fonts/");
            FontManager.DefaultFont =
                Engine.Instance.Renderer.CreateFont(Content.Load<SpriteFont>("Fonts/Code_7x5_13.5_Regular"));

            ImageManager.Instance.LoadImages(Content, "Images/");
            base.LoadContent();
        }

        private void graphics_DeviceCreated(object sender, EventArgs e)
        {
            Engine engine = new MonoGameEngine(GraphicsDevice, Width, Height);
        }

        private void graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            Width = _graphics.PreferredBackBufferWidth;
            Height = _graphics.PreferredBackBufferHeight;

            _graphics.PreferredBackBufferWidth = 800;
            _graphics.PreferredBackBufferHeight = 600;
        }


        #region Initialize

        protected override void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                ThrowHelper.Throw<Exception>(args.ExceptionObject,
                    args.IsTerminating ? ThrowType.Severe : ThrowType.Error);
            };

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
            SceneController.Initialize(_graphics, new MainMenuScene(this));
            AudioManager.Initialize(this);
            
            IsMouseVisible = true;
        }
        
        #endregion
        
        #region Update
      
        protected override void Update(GameTime gameTime)
        {
            SceneController.Update(gameTime);
            _m.Update(gameTime);
            base.Update(gameTime);
           
        }
       
        #endregion

        #region Draw
        
        protected override void Draw(GameTime gameTime)
        {
            SceneController.Draw(gameTime);
            _m.Draw(gameTime);
            base.Draw(gameTime);
        }
        
        #endregion

        public static void SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
        }

        public static void SetUI(UIRoot root, ViewModelBase viewmodel)
        {
            _m = root;
            _v = viewmodel;
            _m.DataContext = _v;
        }
    }
}
