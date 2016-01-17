#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Cameras;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using Welt.Profiling;
using Welt.UI;
using Keys = Microsoft.Xna.Framework.Input.Keys;

#endregion

namespace Welt
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class WeltGame : Game
    {
        #region Fields

        private readonly GraphicsDeviceManager _graphics;
        private World _world;
        private IRenderer _renderer;

        private KeyboardState _oldKeyboardState;

        private bool _releaseMouse;

        private readonly int _preferredBackBufferHeight;
        private readonly int _preferredBackBufferWidth;

        private HudRenderer _hud;

        private Player _player1; //wont add a player2 for some time, but naming like this helps designing  
        private PlayerRenderer _player1Renderer;

        private DiagnosticWorldRenderer _diagnosticWorldRenderer;
        private bool _diagnosticMode;
        private TextInputComponent _text;

        private SkyDomeRenderer _skyDomeRenderer;

        public static bool ThrowExceptions = true;
        public static WeltGame Instance;
        public static GameState CurrentGameState;

        #endregion

        public WeltGame()
        {
            CurrentGameState = GameState.Loading;
            Instance = this;
            //DeProfiler.Run();

            _graphics = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = 1000,
                PreferredBackBufferHeight = 750
            };

            _preferredBackBufferHeight = _graphics.PreferredBackBufferHeight;
            _preferredBackBufferWidth = _graphics.PreferredBackBufferWidth;

            //enter stealth mode at start
            //graphics.PreferredBackBufferHeight = 100;
            //graphics.PreferredBackBufferWidth = 160;

            Content.RootDirectory = "Content";
            _graphics.SynchronizeWithVerticalRetrace = true; // press f3 to set it to false at runtime 

            ShowDebugKeysHelp();
        }

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            _world = new World();

            _player1 = new Player(_world);

            _player1Renderer = new PlayerRenderer(GraphicsDevice, _player1);
            _player1Renderer.Initialize();

            _hud = new HudRenderer(GraphicsDevice, _world, _player1Renderer) {ShowMinimap = true};
            _hud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, world);
            _renderer = new SimpleRenderer(GraphicsDevice, _player1Renderer.Camera, _world);

            _diagnosticWorldRenderer = new DiagnosticWorldRenderer(GraphicsDevice, _player1Renderer.Camera, _world);
            _skyDomeRenderer = new SkyDomeRenderer(GraphicsDevice, _player1Renderer.Camera, _world);
            _renderer.Initialize();
            _diagnosticWorldRenderer.Initialize();
            _skyDomeRenderer.Initialize();

            #endregion

            _text = new TextInputComponent("Username", "text", 200, 100, GraphicsDevice)
            {
                Foreground = Color.White,
                Cursor = Cursors.IBeam,
                Margin = new BoundsBox(100, 0, 100, 0)
            };
            _text.Initialize(this, new GameTime());

            //TODO refactor WorldRenderer needs player position + view frustum 
            this.IsMouseVisible = true;
            base.Initialize();
        }

        #endregion

        protected override void OnExiting(object sender, EventArgs args)
        {
            _renderer.Stop();
            base.OnExiting(sender, args);
        }

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _renderer.LoadContent(Content);
            _diagnosticWorldRenderer.LoadContent(Content);
            _skyDomeRenderer.LoadContent(Content);
            _player1Renderer.LoadContent(Content);
            _hud.LoadContent(Content);
        }

        #endregion

        #region UnloadContent

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        #endregion

        #region DebugKeys

        private static void ShowDebugKeysHelp()
        {
            Console.WriteLine("Debug keys");
            Console.WriteLine("F1  = toggle freelook(fly) / player physics");
            Console.WriteLine("F3  = toggle vsync + fixedtimestep updates ");
            Console.WriteLine("F4  = toggle 100*160 window size");
            Console.WriteLine("F7  = toggle wireframe");
            Console.WriteLine("F8  = toggle chunk diagnostics");
            Console.WriteLine("F9  = toggle day cycle / day mode");
            Console.WriteLine("F10 = toggle day cycle / night mode");
            Console.WriteLine("F11 = toggle fullscreen");
            Console.WriteLine("F   = release / regain focus");
            Console.WriteLine("Esc = exit");
        }

        private void ProcessDebugKeys()
        {
            var keyState = Keyboard.GetState();

            //toggle fullscreen
            if (_oldKeyboardState.IsKeyUp(Keys.F11) && keyState.IsKeyDown(Keys.F11))
            {
                _graphics.ToggleFullScreen();
            }

            //freelook mode
            if (_oldKeyboardState.IsKeyUp(Keys.F1) && keyState.IsKeyDown(Keys.F1))
            {
                _player1Renderer.FreeCam = !_player1Renderer.FreeCam;
            }

            //minimap mode
            if (_oldKeyboardState.IsKeyUp(Keys.M) && keyState.IsKeyDown(Keys.M))
            {
                _hud.ShowMinimap = !_hud.ShowMinimap;
            }

            //wireframe mode
            if (_oldKeyboardState.IsKeyUp(Keys.F7) && keyState.IsKeyDown(Keys.F7))
            {
                _world.ToggleRasterMode();
            }

            //diagnose mode
            if (_oldKeyboardState.IsKeyUp(Keys.F8) && keyState.IsKeyDown(Keys.F8))
            {
                _diagnosticMode = !_diagnosticMode;
            }

            //day cycle/dayMode
            if (_oldKeyboardState.IsKeyUp(Keys.F9) && keyState.IsKeyDown(Keys.F9))
            {
                _world.DayMode = !_world.DayMode;
                //Debug.WriteLine("Day Mode is " + world.dayMode);
            }

            //day cycle/nightMode
            if (_oldKeyboardState.IsKeyUp(Keys.F10) && keyState.IsKeyDown(Keys.F10))
            {
                _world.NightMode = !_world.NightMode;
                //Debug.WriteLine("Day/Night Mode is " + world.nightMode);
            }

            // Allows the game to exit
            if (keyState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            // Release the mouse pointer
            if (_oldKeyboardState.IsKeyUp(Keys.F) && keyState.IsKeyDown(Keys.F))
            {
                _releaseMouse = !_releaseMouse;
                IsMouseVisible = !IsMouseVisible;
            }

            // fixed time step
            if (_oldKeyboardState.IsKeyUp(Keys.F3) && keyState.IsKeyDown(Keys.F3))
            {
                _graphics.SynchronizeWithVerticalRetrace = !_graphics.SynchronizeWithVerticalRetrace;
                IsFixedTimeStep = !IsFixedTimeStep;
                Debug.WriteLine("FixedTimeStep and vsync are " + IsFixedTimeStep);
                _graphics.ApplyChanges();
            }

            // stealth mode / keep screen space for profilers
            if (_oldKeyboardState.IsKeyUp(Keys.F4) && keyState.IsKeyDown(Keys.F4))
            {
                if (_graphics.PreferredBackBufferHeight == _preferredBackBufferHeight)
                {
                    _graphics.PreferredBackBufferHeight = 100;
                    _graphics.PreferredBackBufferWidth = 160;
                }
                else
                {
                    _graphics.PreferredBackBufferHeight = _preferredBackBufferHeight;
                    _graphics.PreferredBackBufferWidth = _preferredBackBufferWidth;
                }
                _graphics.ApplyChanges();
            }

            _oldKeyboardState = keyState;
        }

        #endregion

        #region UpdateTOD

        public virtual Vector3 UpdateTod(GameTime gameTime)
        {
            const long div = 20000;

            if (!_world.RealTime)
                _world.Tod += ((float) gameTime.ElapsedGameTime.Milliseconds/div);
            else
                _world.Tod = (DateTime.Now.Hour) + ((float) DateTime.Now.Minute)/60 +
                              (((float) DateTime.Now.Second)/60)/60;

            if (_world.Tod >= 24)
                _world.Tod = 0;

            if (_world.DayMode)
            {
                _world.Tod = 12;
                _world.NightMode = false;
            }
            else if (_world.NightMode)
            {
                _world.Tod = 0;
                _world.DayMode = false;
            }

            // Calculate the position of the sun based on the time of day.
            float x;
            float y;

            if (_world.Tod <= 12)
            {
                y = _world.Tod/12;
                x = 12 - _world.Tod;
            }
            else
            {
                y = (24 - _world.Tod)/12;
                x = 12 - _world.Tod;
            }

            x /= 10;

            _world.SunPos = new Vector3(-x, y, 0);

            return _world.SunPos;
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameState.Playing:
                    UpdatePlaying(gameTime);
                    break;
                case GameState.Loading:
                    UpdateLoading(gameTime);
                    break;
                case GameState.StartMenu:
                    UpdateStartMenu(gameTime);
                    break;
            }
        }

        private void UpdateLoading(GameTime gameTime)
        {
            _text.Update(gameTime);
        }

        private void UpdateStartMenu(GameTime gameTime)
        {
            
        }

        private void UpdatePlaying(GameTime gameTime)
        {
            ProcessDebugKeys();

            if (IsActive)
            {
                if (!_releaseMouse)
                {
                    _player1Renderer.Update(gameTime);
                }

                _skyDomeRenderer.Update(gameTime);
                _renderer.Update(gameTime);
                if (_diagnosticMode)
                {
                    _diagnosticWorldRenderer.Update(gameTime);
                }
                base.Update(gameTime);
            }
            UpdateTod(gameTime);
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch (CurrentGameState)
            {
                case GameState.StartMenu:
                    DrawStartMenu(gameTime);
                    break;
                case GameState.Loading:
                    DrawLoading(gameTime);
                    break;
                case GameState.Playing:
                    DrawPlaying(gameTime);
                    break;
            }
            base.Draw(gameTime);
        }

        private void DrawLoading(GameTime gameTime)
        {
            _text.Draw(gameTime);
        }

        private void DrawStartMenu(GameTime gameTime)
        {
            
        }

        private void DrawPlaying(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            _skyDomeRenderer.Draw(gameTime);
            _renderer.Draw(gameTime);
            if (_diagnosticMode)
            {
                _diagnosticWorldRenderer.Draw(gameTime);
            }
            _player1Renderer.Draw(gameTime);
            _hud.Draw(gameTime);
        }

        #endregion

        public enum GameState
        {
            Loading,
            StartMenu,
            Playing,
        }
    }
}
