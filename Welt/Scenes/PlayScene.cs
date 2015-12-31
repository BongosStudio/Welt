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

        private World m_world;
        private IRenderer m_renderer;
        private HudRenderer m_hud;

        private Player m_player1; //wont add a player2 for some time, but naming like this helps designing  
        private PlayerRenderer m_player1Renderer;

        private DiagnosticWorldRenderer m_diagnosticWorldRenderer;
        private bool m_diagnosticMode;
        private bool m_releaseMouse;
        private KeyboardState m_oldKeyboardState;

        private SkyDomeRenderer m_skyDomeRenderer;

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
            m_world = new World();
            m_player1 = new Player(m_world);
            m_player1Renderer = new PlayerRenderer(GraphicsDevice, m_player1);
            m_hud = new HudRenderer(GraphicsDevice, m_world, m_player1Renderer);
            m_renderer = new SimpleRenderer(GraphicsDevice, m_player1Renderer.Camera, m_world);
            m_diagnosticWorldRenderer = new DiagnosticWorldRenderer(GraphicsDevice, m_player1Renderer.Camera, m_world);
            m_skyDomeRenderer = new SkyDomeRenderer(GraphicsDevice, m_player1Renderer.Camera, m_world);

            base.Initialize();
            
            m_player1Renderer.Initialize();
            
            m_hud.Initialize();

            #region choose renderer

            //renderer = new ThreadedWorldRenderer(GraphicsDevice, player1Renderer.camera, world);          
            m_renderer.Initialize();
            m_diagnosticWorldRenderer.Initialize();
            m_skyDomeRenderer.Initialize();

            #endregion

            //TODO refactor WorldRenderer needs player position + view frustum 
        }

        #endregion

        public override void OnExiting(object sender, EventArgs args)
        {
            m_renderer.Stop();
            base.OnExiting(sender, args);
        }

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_renderer.LoadContent(Game.Content);
            m_diagnosticWorldRenderer.LoadContent(Game.Content);
            m_skyDomeRenderer.LoadContent(Game.Content);
            m_player1Renderer.LoadContent(Game.Content);
            m_hud.LoadContent(Game.Content);
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
            if (m_oldKeyboardState.IsKeyUp(Keys.F11) && keyState.IsKeyDown(Keys.F11))
            {
                Controller.GraphicsManager.ToggleFullScreen();
            }

            //freelook mode
            if (m_oldKeyboardState.IsKeyUp(Keys.F1) && keyState.IsKeyDown(Keys.F1))
            {
                m_player1Renderer.FreeCam = !m_player1Renderer.FreeCam;
            }

            //minimap mode
            if (m_oldKeyboardState.IsKeyUp(Keys.M) && keyState.IsKeyDown(Keys.M))
            {
                m_hud.ShowMinimap = !m_hud.ShowMinimap;
            }

            //wireframe mode
            if (m_oldKeyboardState.IsKeyUp(Keys.F7) && keyState.IsKeyDown(Keys.F7))
            {
                m_world.ToggleRasterMode();
            }

            //diagnose mode
            if (m_oldKeyboardState.IsKeyUp(Keys.F8) && keyState.IsKeyDown(Keys.F8))
            {
                m_diagnosticMode = !m_diagnosticMode;
            }

            //day cycle/dayMode
            if (m_oldKeyboardState.IsKeyUp(Keys.F9) && keyState.IsKeyDown(Keys.F9))
            {
                m_world.DayMode = !m_world.DayMode;
                //Debug.WriteLine("Day Mode is " + world.dayMode);
            }

            //day cycle/nightMode
            if (m_oldKeyboardState.IsKeyUp(Keys.F10) && keyState.IsKeyDown(Keys.F10))
            {
                m_world.NightMode = !m_world.NightMode;
                //Debug.WriteLine("Day/Night Mode is " + world.nightMode);
            }

            // Allows the game to exit
            if (keyState.IsKeyDown(Keys.Escape))
            {
                Game.Exit();
            }

            // Release the mouse pointer
            if (m_oldKeyboardState.IsKeyUp(Keys.F) && keyState.IsKeyDown(Keys.F))
            {
                m_releaseMouse = !m_releaseMouse;
                Game.IsMouseVisible = !Game.IsMouseVisible;
            }

            // fixed time step
            if (m_oldKeyboardState.IsKeyUp(Keys.F3) && keyState.IsKeyDown(Keys.F3))
            {
                Controller.GraphicsManager.SynchronizeWithVerticalRetrace = !Controller.GraphicsManager.SynchronizeWithVerticalRetrace;
                Game.IsFixedTimeStep = !Game.IsFixedTimeStep;
                Debug.WriteLine("FixedTimeStep and vsync are " + Game.IsFixedTimeStep);
                Controller.GraphicsManager.ApplyChanges();
            }

            // stealth mode / keep screen space for profilers
            if (m_oldKeyboardState.IsKeyUp(Keys.F4) && keyState.IsKeyDown(Keys.F4))
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

            m_oldKeyboardState = keyState;
        }

        #endregion

        #region UpdateTOD

        public virtual Vector3 UpdateTod(GameTime gameTime)
        {
            const long div = 20000;

            if (!m_world.RealTime)
                m_world.Tod += ((float)gameTime.ElapsedGameTime.Milliseconds / div);
            else
                m_world.Tod = (DateTime.Now.Hour) + ((float)DateTime.Now.Minute) / 60 +
                              (((float)DateTime.Now.Second) / 60) / 60;

            if (m_world.Tod >= 24)
                m_world.Tod = 0;

            if (m_world.DayMode)
            {
                m_world.Tod = 12;
                m_world.NightMode = false;
            }
            else if (m_world.NightMode)
            {
                m_world.Tod = 0;
                m_world.DayMode = false;
            }

            // Calculate the position of the sun based on the time of day.
            float x;
            float y;

            if (m_world.Tod <= 12)
            {
                y = m_world.Tod / 12;
                x = 12 - m_world.Tod;
            }
            else
            {
                y = (24 - m_world.Tod) / 12;
                x = 12 - m_world.Tod;
            }

            x /= 10;

            m_world.SunPos = new Vector3(-x, y, 0);

            return m_world.SunPos;
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
                if (!m_releaseMouse)
                {
                    m_player1Renderer.Update(gameTime);
                }

                m_skyDomeRenderer.Update(gameTime);
                m_renderer.Update(gameTime);
                if (m_diagnosticMode)
                {
                    m_diagnosticWorldRenderer.Update(gameTime);
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
            GraphicsDevice.Clear(BackColor);
            m_skyDomeRenderer.Draw(gameTime);
            m_renderer.Draw(gameTime);
            if (m_diagnosticMode)
            {
                m_diagnosticWorldRenderer.Draw(gameTime);
            }
            m_player1Renderer.Draw(gameTime);
            m_hud.Draw(gameTime);
            base.Draw(gameTime);
        }

        #endregion
    }
}
