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
using Welt.Core.Forge;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        private readonly World _mWorld;
        private readonly IRenderer _mRenderer;
        private HudRenderer _mHud;
        private readonly Player _mPlayer = Player.Current;
        private readonly PlayerRenderer _mPlayerRenderer;
        private SimpleRenderer _mWorldRenderer;
        private bool _mDiagnosticMode;
        private bool _mReleaseMouse;
        private KeyboardState _mOldKeyboardState;
        private readonly SkyDomeRenderer _mSkyDomeRenderer;
        private Vector2 _mPreviousPauseMousePosition;
        
        protected override Color BackColor => Color.Black;
        internal override UIRoot UI => new Play();
        internal override ViewModelBase DataContext { get; set; }

        public PlayScene(WeltGame game, World worldToHandoff, IRenderer rendererToHandoff, SkyDomeRenderer skyToHandoff,
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
                    Mouse.SetPosition((int) _mPreviousPauseMousePosition.X, (int) _mPreviousPauseMousePosition.Y);
                    viewModel.PauseMenuVisibility = Visibility.Visible;
                }
                _mPlayer.IsPaused = !_mPlayer.IsPaused;
                WeltGame.Instance.IsMouseVisible = !WeltGame.Instance.IsMouseVisible;

            }, InputController.InputAction.Escape);
            
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
            // this needs to be redone
            _mHud = new HudRenderer(GraphicsDevice, _mWorld, _mPlayerRenderer);
            _mWorldRenderer = new SimpleRenderer(GraphicsDevice, _mPlayerRenderer.Camera, new Forge.Builders.WorldBuilder(GraphicsDevice, _mWorld, _mRenderer));
            
            base.Initialize();                  
            _mHud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, WorldObject);          
            _mWorldRenderer.Initialize();

            #endregion
            


            #region Initialize Keys

            

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
        
        #endregion

        #region UpdateTimeOfDay

        public virtual Vector3 UpdateTimeOfDay(GameTime gameTime)
        {
            // TODO
            //const long div = 20000;

            //_mWorld.TimeOfDay += gameTime.ElapsedGameTime.Milliseconds/div;

            //if (_mWorld.TimeOfDay >= 24)
            //    _mWorld.TimeOfDay = 0;

            //if (_mWorld.DayMode)
            //{
            //    _mWorld.TimeOfDay = 12;
            //    _mWorld.NightMode = false;
            //}
            //else if (_mWorld.NightMode)
            //{
            //    _mWorld.TimeOfDay = 0;
            //    _mWorld.DayMode = false;
            //}

            //// Calculate the position of the sun based on the time of day.
            //float x;
            //float y;

            //if (_mWorld.TimeOfDay <= 12)
            //{
            //    y = _mWorld.TimeOfDay / 12;
            //    x = 12 - _mWorld.TimeOfDay;
            //}
            //else
            //{
            //    y = (24 - _mWorld.TimeOfDay) / 12;
            //    x = 12 - _mWorld.TimeOfDay;
            //}

            //x /= 10;

            //_mWorld.SunPos = new Vector3(-x, y, 0);

            //return _mWorld.SunPos;
            return Vector3.Zero;
        }

        #endregion

        #region Update

        /// <summary>
        /// Allows the game to run logic such as updating the WorldObject,
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
            UpdateTimeOfDay(gameTime);
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
        }

        #endregion
    }
}
