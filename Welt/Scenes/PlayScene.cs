#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Diagnostics;
using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Mvvm;
using GameUILibrary.Models;
using Microsoft.Xna.Framework;
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
        private readonly World _mWorld;
        private readonly IRenderer _mRenderer;
        private HudRenderer _mHud;
        private readonly Player _mPlayer = Player.Current;
        private readonly PlayerRenderer _mPlayerRenderer;
        private DiagnosticWorldRenderer _mWorldRenderer;
        private bool _mDiagnosticMode;
        private bool _mReleaseMouse;
        private KeyboardState _mOldKeyboardState;
        private readonly SkyDomeRenderer _mSkyDomeRenderer;
        private Vector2 _mPreviousPauseMousePosition;
        
        protected override Color BackColor => Color.Black;
        internal override UIRoot UI => new Play();
        internal override ViewModelBase DataContext { get; set; }

        public PlayScene(Game game, World worldToHandoff, IRenderer rendererToHandoff, SkyDomeRenderer skyToHandoff,
            PlayerRenderer playerToHandoff) : base(game)
        {
            _mWorld = worldToHandoff;
            _mRenderer = rendererToHandoff;
            _mSkyDomeRenderer = skyToHandoff;
            _mPlayerRenderer = playerToHandoff;
            _mPreviousPauseMousePosition = new Vector2(FirstPersonCameraController.DefaultMouseState.X,
                FirstPersonCameraController.DefaultMouseState.Y);

            var viewModel = new PlayViewModel();
            AssignKeyToEvent(() =>
            {
                if (_mPlayer.IsPaused)
                {
                    _mPreviousPauseMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
                    Mouse.SetPosition(FirstPersonCameraController.DefaultMouseState.X,
                        FirstPersonCameraController.DefaultMouseState.Y);
                    viewModel.PauseMenuVisibility = Visibility.Collapsed;
                }
                else
                {
                    Mouse.SetPosition((int)_mPreviousPauseMousePosition.X, (int)_mPreviousPauseMousePosition.Y);
                    viewModel.PauseMenuVisibility = Visibility.Visible;
                }
                _mPlayer.IsPaused = !_mPlayer.IsPaused;
                WeltGame.Instance.IsMouseVisible = !WeltGame.Instance.IsMouseVisible;

                return true;
            }, Keys.Escape);
            DataContext = viewModel;
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
            _mHud = new HudRenderer(GraphicsDevice, _mWorld, _mPlayerRenderer);
            _mWorldRenderer = new DiagnosticWorldRenderer(GraphicsDevice, _mPlayerRenderer.Camera, _mWorld);
            
            base.Initialize();                  
            _mHud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, world);          
            _mWorldRenderer.Initialize();

            #endregion
            


            #region Initialize Keys

            AssignKeyToEvent(() =>
            {
                SceneController.GraphicsManager.ToggleFullScreen();
                return true;
            }, Keys.F11);
            AssignKeyToEvent(() =>
            {
                _mPlayerRenderer.FreeCam = !_mPlayerRenderer.FreeCam;
                return true;
            }, Keys.F1);
            AssignKeyToEvent(() =>
            {
                Game.Exit();
                return true;
            }, Keys.LeftShift, Keys.Escape);
            AssignKeyToEvent(() =>
            {
                // TODO: show console
                return true;
            }, Keys.OemTilde);
            
            //AssignKeyToEvent(() =>
            //{
            //    if (_mPlayer.IsPaused)
            //    {
            //        _mPreviousPauseMousePosition = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            //        Mouse.SetPosition(FirstPersonCameraController.DefaultMouseState.X,
            //            FirstPersonCameraController.DefaultMouseState.Y);
            //    }
            //    else
            //    {
            //        Mouse.SetPosition((int) _mPreviousPauseMousePosition.X, (int) _mPreviousPauseMousePosition.Y);
            //    }
            //    _mPlayer.IsPaused = !_mPlayer.IsPaused;
            //    WeltGame.Instance.IsMouseVisible = !WeltGame.Instance.IsMouseVisible;
                 
            //    return true;
            //}, Keys.Escape);
            
            AssignHotbarKeys();

            #endregion

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
            
            _mWorldRenderer.LoadContent(WeltGame.Instance.Content);
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
                _mPlayerRenderer.FreeCam = !_mPlayerRenderer.FreeCam;
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
            if (keyState.IsKeyDown(Keys.Escape) && keyState.IsKeyDown(Keys.LeftShift))
            {
                Game.Exit();
                return;
            }

            // Release the mouse pointer
            if (_mOldKeyboardState.IsKeyUp(Keys.Escape) && keyState.IsKeyDown(Keys.Escape))
            {
                _mReleaseMouse = !_mReleaseMouse;
                Game.IsMouseVisible = !Game.IsMouseVisible;
                _mPlayer.IsPaused = !_mPlayer.IsPaused;
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
            if (Game.IsActive)
            {
                _mPlayerRenderer.Update(gameTime);
                _mSkyDomeRenderer.Update(gameTime);
                _mRenderer.Update(gameTime);
                if (_mDiagnosticMode)
                {
                    _mWorldRenderer.Update(gameTime);
                }
                base.Update(gameTime);
            }
            WeltGame.Instance.IsMouseVisible = _mPlayer.IsPaused;
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
                _mWorldRenderer.Draw(gameTime);
            }
            _mPlayerRenderer.Draw(gameTime);
            _mHud.Draw(gameTime);
        }

        #endregion

        #region Private methods

        private void AssignHotbarKeys()
        {
            AssignKeyToEvent(() => true, Keys.D1);
            AssignKeyToEvent(() => true, Keys.D2);
        }

        #endregion
    }
}
