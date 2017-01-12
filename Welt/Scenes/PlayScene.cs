#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Generated;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface;
using GameUILibrary.Models;
using Welt.Extensions;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using Welt.Particles;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        private Play PlUi => new Play
        {
            DataContext = PlVm
        };
        private PlayViewModel PlVm => new PlayViewModel
        {
            QuitButtonCommand = new Action(() => Next(new MainMenuScene(Game))).CreateButtonCommand(),
            OptionsButtonCommand = new Action(SwitchToSettings).CreateButtonCommand(),
            ResumeButtonCommand = new Action(() => m_Player.IsPaused = false).CreateButtonCommand()
        };

        private SettingsMenu SeUi => new SettingsMenu
        {
            DataContext = SeVm
        };
        private SettingsModel SeVm => new SettingsModel
        {
            ExitCommand =
                new Action(SwitchToPause).CreateButtonCommand()
        };

        private World m_World;
        private IRenderer m_Renderer;
        private HudRenderer m_Hud;
        private Player m_Player;
        private PlayerRenderer m_PlayerRenderer;
        private SkyRenderer m_SkyRenderer;
        private Vector2 m_PreviousMousePosition;
        private FramesPerSecondCounterComponent m_Fps;
        
        internal override Color BackColor => Color.GhostWhite;
        internal override UIRoot UI { get; set; } = new Play();
        internal override ViewModelBase DataContext { get; set; }

        public PlayScene(WeltGame game, World worldToHandoff, IRenderer rendererToHandoff, SkyRenderer skyToHandoff,
            PlayerRenderer playerToHandoff) : base(game)
        {
            m_Fps = new FramesPerSecondCounterComponent(game);
            m_World = worldToHandoff;
            m_Renderer = rendererToHandoff;
            m_SkyRenderer = skyToHandoff;
            m_PlayerRenderer = playerToHandoff;
            m_Player = playerToHandoff.Player;
            m_PreviousMousePosition = new Vector2(FirstPersonCameraController.DefaultMouseState.X,
                FirstPersonCameraController.DefaultMouseState.Y);
            m_Hud = new HudRenderer(GraphicsDevice, m_World, m_PlayerRenderer);
            
            Input.Assign(() =>
            {
                m_Player.IsPaused = !m_Player.IsPaused;
                PlVm.PauseMenuVisibility = m_Player.IsPaused ?
                        Visibility.Visible : Visibility.Collapsed;
            }, Keys.Escape);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 0;
            }, Keys.D1);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 1;
            }, Keys.D2);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 2;
            }, Keys.D3);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 3;
            }, Keys.D4);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 4;
            }, Keys.D5);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 5;
            }, Keys.D6);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 6;
            }, Keys.D7);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 7;
            }, Keys.D8);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 8;
            }, Keys.D9);
            Input.Assign(() =>
            {
                m_Player.HotbarIndex = 9;
            }, Keys.D0);
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
            // all world/sky renderers will be initialized before this in the loading scene
            
            m_Hud.Initialize();
            m_Fps.Initialize();
        }

        #endregion

        public override void OnExiting(object sender, EventArgs args)
        {
            m_Renderer.Stop();
            base.OnExiting(sender, args);
        }

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            m_Renderer.LoadContent(Game.Content);
            m_Hud.LoadContent(Game.Content);
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
        
        #region UpdateTOD

        public virtual Vector3 UpdateTod(GameTime gameTime)
        {
            const long div = 20000;

            if (!m_World.RealTime)
                m_World.Tod += ((float)gameTime.ElapsedGameTime.Milliseconds / div);
            else
                m_World.Tod = (DateTime.Now.Hour) + ((float)DateTime.Now.Minute) / 60 +
                              (((float)DateTime.Now.Second) / 60) / 60;

            if (m_World.Tod >= 24)
                m_World.Tod = 0;

            if (m_World.DayMode)
            {
                m_World.Tod = 12;
                m_World.NightMode = false;
            }
            else if (m_World.NightMode)
            {
                m_World.Tod = 0;
                m_World.DayMode = false;
            }

            // Calculate the position of the sun based on the time of day.
            float x;
            float y;

            if (m_World.Tod <= 12)
            {
                y = m_World.Tod / 12;
                x = 12 - m_World.Tod;
            }
            else
            {
                y = (24 - m_World.Tod) / 12;
                x = 12 - m_World.Tod;
            }

            x /= 10;

            m_World.SunPos = new Vector3(-x, y, 0);

            return m_World.SunPos;
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
                Input.Update(gameTime);
                m_PlayerRenderer.Update(gameTime);
                m_SkyRenderer.Update(gameTime);
                m_Renderer.Update(gameTime);
                m_Fps.Update(gameTime);
            }
            WeltGame.Instance.IsMouseVisible = m_Player.IsPaused;
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
            m_SkyRenderer.Draw(gameTime);
            m_Renderer.Draw(gameTime);
            m_PlayerRenderer.Draw(gameTime);
            m_Hud.Draw(gameTime);
            m_Fps.Draw(gameTime);
        }

        #endregion

        public override void Dispose()
        {
            m_PlayerRenderer = null;
            m_SkyRenderer = null;
            m_Renderer = null;
            m_Player = null;
            m_Fps.Dispose();
            GC.WaitForPendingFinalizers();
            //GC.Collect();
            GC.AddMemoryPressure(1024*1024);
            base.Dispose();
        }

        #region Private methods

        private void SwitchToSettings()
        {
            UI = SeUi;
        }

        private void SwitchToPause()
        {
            UI = PlUi;
        }

        #endregion
    }
}
