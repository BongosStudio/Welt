#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Media;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Renderers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Sec;
using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Windows.Forms;
using Welt.Components;
using Welt.Controllers;
using Welt.Graphics;
using Welt.Scenes;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Welt
{
    public class WeltGame : Game
    {
        // All hail Trump, the glorious overlord. 
        // Let the winds be fair and the tides be gentle.
        #region Fields

        public static int Height;
        public static WeltGame Instance;
        public static bool ThrowExceptions = true;
        public static int Width;
        
        public GameSettings GameSettings { get; private set; }
        public AudioFactory Audio { get; private set; }
        public TaskManagerComponent TaskManager { get; private set; }
        
        private readonly GraphicsDeviceManager m_Graphics;
        private MonoGameEngine m_UiEngine;

        #endregion Fields

        public WeltGame(string username, string key)
        {
            Instance = this;
            GameSettings = new GameSettings();
            m_Graphics = CreateGraphicsDevice();
            Content.RootDirectory = "Content";
            Audio = new AudioFactory(this);
            TaskManager = new TaskManagerComponent(this);
            Components.Add(TaskManager);
            // initialize the player
        }

        public static void SetCursor(Cursor cursor)
        {
            Cursor.Current = cursor;
        }

        protected override void LoadContent()
        {
            FontManager.Instance.LoadFonts(Content, "Fonts/");
            FontManager.DefaultFont =
                Engine.Instance.Renderer.CreateFont(Content.Load<SpriteFont>("Fonts/Code_7x5_13.5_Regular"));

            ImageManager.Instance.LoadImages(Content, "Images/");
            
            base.LoadContent();
        }

        private GraphicsDeviceManager CreateGraphicsDevice()
        {
            var fullscreen = GameSettings.DisplayMode == WindowDisplayMode.TrueFullScreen;
            int height;
            int width;
            if (GameSettings.DisplayMode == WindowDisplayMode.Windowed)
            {
                height = 600;
                width = 800;
            }
            else
            {
                height = Screen.PrimaryScreen.Bounds.Height;
                width = Screen.PrimaryScreen.Bounds.Width;
            }
            if (GameSettings.DisplayMode == WindowDisplayMode.FakeFullScreen)
            {
                Window.Position = new Point(0);
                Window.IsBorderless = true;
                Window.AllowUserResizing = false;
            }
            var graphics = new GraphicsDeviceManager(this)
            {
                IsFullScreen = fullscreen,
                PreferredBackBufferWidth = width,
                PreferredBackBufferHeight = height,
                SynchronizeWithVerticalRetrace = true
            };
            graphics.DeviceCreated += Graphics_DeviceCreated;
            graphics.PreparingDeviceSettings += Graphics_PreparingDeviceSettings;
            return graphics;
        }

        private void Graphics_DeviceCreated(object sender, EventArgs e)
        {
            CreateUiEngine();
            
        }

        private void Graphics_PreparingDeviceSettings(object sender, PreparingDeviceSettingsEventArgs e)
        {
            Width = m_Graphics.PreferredBackBufferWidth;
            Height = m_Graphics.PreferredBackBufferHeight;
        }

        private void CreateUiEngine()
        {
            var width = GraphicsDevice.PresentationParameters.IsFullScreen ?
                Screen.PrimaryScreen.Bounds.Width :
                Width;
            var height = GraphicsDevice.PresentationParameters.IsFullScreen ?
                Screen.PrimaryScreen.Bounds.Height :
                Height;
            m_UiEngine = new MonoGameEngine(GraphicsDevice, Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            //Engine.Instance.Renderer.ResetNativeSize();
        }

        #region Initialize

        protected override void Initialize()
        {
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                ThrowHelper.Throw<Exception>(args.ExceptionObject,
                    args.IsTerminating ? ThrowType.Severe : ThrowType.Error);
            };
            //TextureMap.LoadTextures(GraphicsDevice, "textures");
            IsMouseVisible = true;
            Window.ClientSizeChanged += (sender, args) =>
            {
                Height = Window.ClientBounds.Height;
                Width = Window.ClientBounds.Width;
                GraphicsDevice.PresentationParameters.BackBufferWidth = Width;
                GraphicsDevice.PresentationParameters.BackBufferHeight = Height;
                GraphicsDevice.Viewport = new Viewport(0, 0, Width, Height, 0, 1);
                //CreateUiEngine();
            };
            base.Initialize();
            Height = Window.ClientBounds.Height;
            Width = Window.ClientBounds.Width;
            SetCursor(Cursors.Default);
            SceneController.Initialize(m_Graphics, new MainMenuScene(this));
            // TODO: load audio 
            //TextureMap.LoadTextures(GraphicsDevice, "textures");
            IsMouseVisible = true;
        }

        #endregion Initialize

        #region Update

        protected override void Update(GameTime gameTime)
        {
            SceneController.Update(gameTime);
            //m_UiRoot?.Update(gameTime);
            base.Update(gameTime);
        }

        #endregion Update

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            SceneController.Draw(gameTime);
            // Idk if we'll need to switch ui draw and base draw. UI should always be on top but
            // again, idk if there is anything base is drawing.
            //m_UiRoot?.Draw(gameTime);
            base.Draw(gameTime);
        }

        #endregion Draw
    }
}