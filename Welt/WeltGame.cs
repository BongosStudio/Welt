#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;
using System.Windows.Forms;
using Welt.Components;
using Welt.Controllers;
using Welt.Core.Forge;
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
        public GraphicsManager GraphicsManager { get; private set; }
        public bool IsRunning { get; private set; }
        
        private readonly GraphicsDeviceManager m_Graphics;
        private MonoGameEngine m_UiEngine;
        private RenderTarget2D m_RenderTarget;
        private SpriteBatch m_SpriteBatch;

        #endregion Fields

        public WeltGame(string username, string key)
        {
            Instance = this;
            GameSettings = new GameSettings();
            m_Graphics = CreateGraphicsDevice();
            Content.RootDirectory = "Content";
            Audio = new AudioFactory(this);
            TaskManager = new TaskManagerComponent(this);
            GraphicsManager = new GraphicsManager(this);
            Components.Add(TaskManager);
            Exiting += (sender, args) =>
            {
                IsRunning = false;
            };
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
            m_RenderTarget = new RenderTarget2D(
                GraphicsDevice,
                GraphicsDevice.PresentationParameters.BackBufferWidth,
                GraphicsDevice.PresentationParameters.BackBufferHeight,
                false,
                GraphicsDevice.PresentationParameters.BackBufferFormat,
                GraphicsDevice.PresentationParameters.DepthStencilFormat);
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
            base.LoadContent();
        }

        private GraphicsDeviceManager CreateGraphicsDevice()
        {
            var fullscreen = GameSettings.DisplayMode == WindowDisplayMode.TrueFullScreen;
            int height;
            int width;
            var allowResize = false;
            if (GameSettings.DisplayMode == WindowDisplayMode.Windowed)
            {
                height = 600;
                width = 800;
                allowResize = true;
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
            }
            Window.AllowUserResizing = allowResize;
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
            e.GraphicsDeviceInformation.PresentationParameters.RenderTargetUsage = RenderTargetUsage.PreserveContents;
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
            IsMouseVisible = true;
            Window.ClientSizeChanged += (sender, args) =>
            {
                Height = Window.ClientBounds.Height;
                Width = Window.ClientBounds.Width;
                GraphicsDevice.PresentationParameters.BackBufferWidth = Width;
                GraphicsDevice.PresentationParameters.BackBufferHeight = Height;
                GraphicsDevice.Viewport = new Viewport(0, 0, Width, Height, 0, 1);
            };
            base.Initialize();
            Height = Window.ClientBounds.Height;
            Width = Window.ClientBounds.Width;
            SetCursor(Cursors.Default);
            SceneController.Initialize(m_Graphics, new MainMenuScene(this));
            // TODO: load audio 
            // TODO: move these to a splash screen
            GraphicsManager.Initialize();
            BlockProvider.CreateProviders();
        }

        #endregion Initialize

        #region Update

        protected override void Update(GameTime gameTime)
        {
            var kstate = Keyboard.GetState();
            if (kstate.IsKeyDown(Keys.F6)) TakeScreenshot();
            SceneController.Update(gameTime);
            //m_UiRoot?.Update(gameTime);
            base.Update(gameTime);
        }

        #endregion Update

        #region Draw

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(m_RenderTarget);
            GraphicsDevice.Clear(Color.Black);
            SceneController.Draw(gameTime);
            base.Draw(gameTime);
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            m_SpriteBatch.Begin();
            m_SpriteBatch.Draw(m_RenderTarget, Vector2.Zero, Color.White);
            m_SpriteBatch.End();
        }

        #endregion Draw

        public void ResetRenderTarget()
        {
            GraphicsDevice.SetRenderTarget(m_RenderTarget);
            GraphicsDevice.Clear(Color.Black);
        }

        public void TakeScreenshot()
        {
            var title = $"screenshot-{DateTime.Now.Month}_{DateTime.Now.Day}_{DateTime.Now.Year}-{DateTime.Now.Hour}_{DateTime.Now.Minute}_{DateTime.Now.Second}";
            using (var stream = File.Create(title + ".png"))
            {
                m_RenderTarget.SaveAsPng(stream, Width, Height);
            }
        }
    }
}