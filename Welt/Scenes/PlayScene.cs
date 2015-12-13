#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Cameras;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        protected override Color BackColor => Color.Black;

        private World _world;
        private IRenderer _renderer;
        private HudRenderer _hud;

        private Player _player1; //wont add a player2 for some time, but naming like this helps designing  
        private PlayerRenderer _player1Renderer;

        private DiagnosticWorldRenderer _diagnosticWorldRenderer;
        private bool _diagnosticMode;
        private bool _releaseMouse;
        private KeyboardState _oldKeyboardState;

        private SkyDomeRenderer _skyDomeRenderer;

        public PlayScene(Game game) : base(game)
        {
            //var frameRate = new FrameRateCounter(game) { DrawOrder = 1 };
            //Game.Components.Add(frameRate);
        }

        public override void Initialize()
        {
            ShowDebugKeysHelp();
            Game.Content.Unload();
            _world = new World();

            _player1 = new Player(_world);

            _player1Renderer = new PlayerRenderer(Controller.Game.GraphicsDevice, _player1);
            _player1Renderer.Initialize();

            _hud = new HudRenderer(Controller.Game.GraphicsDevice, _world, _player1Renderer.Camera);
            _hud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, world);
            _renderer = new SimpleRenderer(Controller.Game.GraphicsDevice, _player1Renderer.Camera, _world);

            _diagnosticWorldRenderer = new DiagnosticWorldRenderer(Controller.Game.GraphicsDevice, _player1Renderer.Camera, _world);
            _skyDomeRenderer = new SkyDomeRenderer(Controller.Game.GraphicsDevice, _player1Renderer.Camera, _world);
            _renderer.Initialize();
            _diagnosticWorldRenderer.Initialize();
            _skyDomeRenderer.Initialize();
            #endregion

            _renderer.LoadContent(Game.Content);
            _diagnosticWorldRenderer.LoadContent(Game.Content);
            _skyDomeRenderer.LoadContent(Game.Content);
            _player1Renderer.LoadContent(Game.Content);
            _hud.LoadContent(Game.Content);

            _oldKeyboardState = Keyboard.GetState();
            //TODO refactor WorldRenderer needs player position + view frustum 
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            ProcessDebugKeys();
            if (Game.IsActive)
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

        public override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackColor);
            _skyDomeRenderer.Draw(gameTime);
            _renderer.Draw(gameTime);
            if (_diagnosticMode)
            {
                _diagnosticWorldRenderer.Draw(gameTime);
            }
            _player1Renderer.Draw(gameTime);
            _hud.Draw(gameTime);
            base.Draw(gameTime);
        }

        #region DebugKeys

        private void ShowDebugKeysHelp()
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
                Controller.GraphicsManager.ToggleFullScreen();
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
                Game.Exit();
            }

            // Release the mouse pointer
            if (_oldKeyboardState.IsKeyUp(Keys.F) && keyState.IsKeyDown(Keys.F))
            {
                _releaseMouse = !_releaseMouse;
                Game.IsMouseVisible = !Game.IsMouseVisible;
            }

            // fixed time step
            if (_oldKeyboardState.IsKeyUp(Keys.F3) && keyState.IsKeyDown(Keys.F3))
            {
                Controller.GraphicsManager.SynchronizeWithVerticalRetrace = !Controller.GraphicsManager.SynchronizeWithVerticalRetrace;
                Game.IsFixedTimeStep = !Game.IsFixedTimeStep;
                Debug.WriteLine("FixedTimeStep and vsync are " + Game.IsFixedTimeStep);
                Controller.GraphicsManager.ApplyChanges();
            }

            // stealth mode / keep screen space for profilers
            if (_oldKeyboardState.IsKeyUp(Keys.F4) && keyState.IsKeyDown(Keys.F4))
            {
                if (Controller.GraphicsManager.PreferredBackBufferHeight == 750)
                {
                    Controller.GraphicsManager.PreferredBackBufferHeight = 100;
                    Controller.GraphicsManager.PreferredBackBufferWidth = 160;
                }
                else
                {
                    Controller.GraphicsManager.PreferredBackBufferHeight = 750;
                    Controller.GraphicsManager.PreferredBackBufferWidth = 1000;
                }
                Controller.GraphicsManager.ApplyChanges();
            }

            _oldKeyboardState = keyState;
        }
        #endregion

        public virtual Vector3 UpdateTod(GameTime gameTime)
        {
            long div = 20000;

            if (!_world.RealTime)
                _world.Tod += ((float) gameTime.ElapsedGameTime.Milliseconds/div);
            else
                _world.Tod = ((float) DateTime.Now.Hour) + ((float) DateTime.Now.Minute)/60 +
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
            float x = 0;
            float y = 0;
            float z = 0;

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

            _world.SunPos = new Vector3(-x, y, z);

            return _world.SunPos;
        }
    }
}
