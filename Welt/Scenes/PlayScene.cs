#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        private World _mWorld;
        private IRenderer _mRenderer;
        private HudRenderer _mHud;
        private Player _mPlayer1; //wont add a player2 for some time, but naming like this helps designing  
        private PlayerRenderer _mPlayer1Renderer;
        private DiagnosticWorldRenderer _mDiagnosticWorldRenderer;
        private bool _mDiagnosticMode;
        private bool _mReleaseMouse;
        private KeyboardState _mOldKeyboardState;
        private SkyDomeRenderer _mSkyDomeRenderer;
        
        protected override Color BackColor => Color.Black;

        public PlayScene(Game game) : base(game)
        {

        }

        #region Initialize

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        public override void Initialize()
        {
            _mWorld = new World();
            _mPlayer1 = new Player(_mWorld);
            _mPlayer1Renderer = new PlayerRenderer(GraphicsDevice, _mPlayer1);
            _mHud = new HudRenderer(GraphicsDevice, _mWorld, _mPlayer1Renderer);
            _mRenderer = new SimpleRenderer(GraphicsDevice, _mPlayer1Renderer.Camera, _mWorld);
            _mDiagnosticWorldRenderer = new DiagnosticWorldRenderer(GraphicsDevice, _mPlayer1Renderer.Camera, _mWorld);
            _mSkyDomeRenderer = new SkyDomeRenderer(GraphicsDevice, _mPlayer1Renderer.Camera, _mWorld);

            base.Initialize();           
            _mPlayer1Renderer.Initialize();        
            _mHud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, world);          
            _mRenderer.Initialize();
            _mDiagnosticWorldRenderer.Initialize();
            _mSkyDomeRenderer.Initialize();

            #endregion

            Game.IsMouseVisible = false;
            //TODO refactor WorldRenderer needs player position + view frustum
            
        }

        #endregion

        public override void OnExiting(object sender, EventArgs args)
        {
            _mRenderer.Stop();
            base.OnExiting(sender, args);
        }

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            _mRenderer.LoadContent(WeltGame.Instance.Content);
            _mDiagnosticWorldRenderer.LoadContent(WeltGame.Instance.Content);
            _mSkyDomeRenderer.LoadContent(WeltGame.Instance.Content);
            _mPlayer1Renderer.LoadContent(WeltGame.Instance.Content);
            _mHud.LoadContent(WeltGame.Instance.Content);
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
            if (_mOldKeyboardState.IsKeyUp(Keys.F11) && keyState.IsKeyDown(Keys.F11))
            {
                SceneController.GraphicsManager.ToggleFullScreen();
            }

            //freelook mode
            if (_mOldKeyboardState.IsKeyUp(Keys.F1) && keyState.IsKeyDown(Keys.F1))
            {
                _mPlayer1Renderer.FreeCam = !_mPlayer1Renderer.FreeCam;
            }

            //minimap mode
            if (_mOldKeyboardState.IsKeyUp(Keys.M) && keyState.IsKeyDown(Keys.M))
            {
                _mHud.ShowMinimap = !_mHud.ShowMinimap;
            }

            //wireframe mode
            if (_mOldKeyboardState.IsKeyUp(Keys.F7) && keyState.IsKeyDown(Keys.F7))
            {
                _mWorld.ToggleRasterMode();
            }

            //diagnose mode
            if (_mOldKeyboardState.IsKeyUp(Keys.F8) && keyState.IsKeyDown(Keys.F8))
            {
                _mDiagnosticMode = !_mDiagnosticMode;
            }

            //day cycle/dayMode
            if (_mOldKeyboardState.IsKeyUp(Keys.F9) && keyState.IsKeyDown(Keys.F9))
            {
                _mWorld.DayMode = !_mWorld.DayMode;
                //Debug.WriteLine("Day Mode is " + world.dayMode);
            }

            //day cycle/nightMode
            if (_mOldKeyboardState.IsKeyUp(Keys.F10) && keyState.IsKeyDown(Keys.F10))
            {
                _mWorld.NightMode = !_mWorld.NightMode;
                //Debug.WriteLine("Day/Night Mode is " + world.nightMode);
            }

            // Allows the game to exit
            if (keyState.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            // Release the mouse pointer
            if (_mOldKeyboardState.IsKeyUp(Keys.F) && keyState.IsKeyDown(Keys.F))
            {
                _mReleaseMouse = !_mReleaseMouse;
                Game.IsMouseVisible = !Game.IsMouseVisible;
            }

            // fixed time step
            if (_mOldKeyboardState.IsKeyUp(Keys.F3) && keyState.IsKeyDown(Keys.F3))
            {
                SceneController.GraphicsManager.SynchronizeWithVerticalRetrace = !SceneController.GraphicsManager.SynchronizeWithVerticalRetrace;
                Game.IsFixedTimeStep = !Game.IsFixedTimeStep;
                Debug.WriteLine("FixedTimeStep and vsync are " + Game.IsFixedTimeStep);
                SceneController.GraphicsManager.ApplyChanges();
            }

            // stealth mode / keep screen space for profilers
            if (_mOldKeyboardState.IsKeyUp(Keys.F4) && keyState.IsKeyDown(Keys.F4))
            {
                if (SceneController.GraphicsManager.PreferredBackBufferHeight == 750)
                {
                    SceneController.GraphicsManager.PreferredBackBufferHeight = 100;
                    SceneController.GraphicsManager.PreferredBackBufferWidth = 160;
                }
                else
                {
                    SceneController.GraphicsManager.PreferredBackBufferHeight = 750;
                    SceneController.GraphicsManager.PreferredBackBufferWidth = 1000;
                }
                SceneController.GraphicsManager.ApplyChanges();
            }

            _mOldKeyboardState = keyState;
        }

        #endregion

        #region UpdateTOD

        public virtual Vector3 UpdateTod(GameTime gameTime)
        {
            const long div = 20000;

            if (!_mWorld.RealTime)
                _mWorld.Tod += ((float)gameTime.ElapsedGameTime.Milliseconds / div);
            else
                _mWorld.Tod = (DateTime.Now.Hour) + ((float)DateTime.Now.Minute) / 60 +
                              (((float)DateTime.Now.Second) / 60) / 60;

            if (_mWorld.Tod >= 24)
                _mWorld.Tod = 0;

            if (_mWorld.DayMode)
            {
                _mWorld.Tod = 12;
                _mWorld.NightMode = false;
            }
            else if (_mWorld.NightMode)
            {
                _mWorld.Tod = 0;
                _mWorld.DayMode = false;
            }

            // Calculate the position of the sun based on the time of day.
            float x;
            float y;

            if (_mWorld.Tod <= 12)
            {
                y = _mWorld.Tod / 12;
                x = 12 - _mWorld.Tod;
            }
            else
            {
                y = (24 - _mWorld.Tod) / 12;
                x = 12 - _mWorld.Tod;
            }

            x /= 10;

            _mWorld.SunPos = new Vector3(-x, y, 0);

            return _mWorld.SunPos;
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Update(GameTime gameTime)
        {
            ProcessDebugKeys();

            if (Game.IsActive)
            {
                if (!_mReleaseMouse)
                {
                    _mPlayer1Renderer.Update(gameTime);
                }

                _mSkyDomeRenderer.Update(gameTime);
                _mRenderer.Update(gameTime);
                if (_mDiagnosticMode)
                {
                    _mDiagnosticWorldRenderer.Update(gameTime);
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
        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _mSkyDomeRenderer.Draw(gameTime);
            _mRenderer.Draw(gameTime);
            if (_mDiagnosticMode)
            {
                _mDiagnosticWorldRenderer.Draw(gameTime);
            }
            _mPlayer1Renderer.Draw(gameTime);
            _mHud.Draw(gameTime);
        }

        #endregion
    }
}
