#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Welt.Cameras;
using Welt.Controllers;
using Welt.Forge.Renderers;
using Welt.API;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using EmptyKeys.UserInterface.Controls;
using MonoGame.Extended;
using EmptyKeys.UserInterface.Generated;
using GameUILibrary.Models;
using Welt.Extensions;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Welt.Core.Forge;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        private PlayUI m_PlayUi => new PlayUI
        {
            DataContext = m_PlayViewModel
        };
        private Pause m_PauseUi => new Pause
        {
            DataContext = new PauseViewModel
            {
                ResumeButtonCommand = 
                new Action(SwitchToPlayUi).CreateButtonCommand(),
                OptionsButtonCommand = 
                new Action(SwitchToSettingsUi).CreateButtonCommand(),
                QuitButtonCommand = 
                new Action(() => Next(new MainMenuScene(Game))).CreateButtonCommand()
            }
        };
        private SettingsMenu m_SettingsUi => new SettingsMenu
        {
            DataContext = new SettingsModel
            {
                ExitCommand =
                new Action(SwitchToPauseUi).CreateButtonCommand()
            }
        };

        private PlayViewModel m_PlayViewModel = new PlayViewModel();

        private World m_World;
        private IRenderer m_Renderer;
        private HudRenderer m_Hud;
        private MultiplayerClient m_Player;
        private PlayerRenderer m_PlayerRenderer;
        private SkyRenderer m_SkyRenderer;
        private Vector2 m_PreviousMousePosition;
        private FramesPerSecondCounterComponent m_Fps;

        private RenderTarget2D m_RenderTarget;
        private SpriteBatch m_SpriteBatch;

        internal override Color BackColor => Color.Black;
        internal override UIRoot UI { get; set; }

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
            
            
            UI = m_PlayUi;
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
            m_RenderTarget = new RenderTarget2D(
                GraphicsDevice, 
                GraphicsDevice.PresentationParameters.BackBufferWidth, 
                GraphicsDevice.PresentationParameters.BackBufferHeight, 
                false, 
                GraphicsDevice.PresentationParameters.BackBufferFormat, 
                GraphicsDevice.PresentationParameters.DepthStencilFormat);
            m_SpriteBatch = new SpriteBatch(GraphicsDevice);
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
            // Calculate the position of the sun based on the time of day.
            float x;
            float y;

            if (m_World.TimeOfDay <= 12)
            {
                y = m_World.TimeOfDay / 12;
                x = 12 - m_World.TimeOfDay;
            }
            else
            {
                y = (24 - m_World.TimeOfDay) / 12;
                x = 12 - m_World.TimeOfDay;
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
       
        private void SwitchToPlayUi()
        {
            UI = m_PlayUi;
            m_Player.IsPaused = false;
        }

        private void SwitchToPauseUi()
        {
            UI = m_PauseUi;
        }

        private void SwitchToSettingsUi()
        {
            UI = m_SettingsUi;
        }

        #endregion
    }
}
