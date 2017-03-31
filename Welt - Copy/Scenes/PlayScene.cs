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
using EmptyKeys.UserInterface.Generated;
using GameUILibrary.Models;
using Welt.Extensions;
using System.Windows.Forms;
using Microsoft.Xna.Framework.Graphics;
using Welt.Core.Forge;
using Welt.Forge;
using Microsoft.Xna.Framework.Content;
using Welt.Components;

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
        
        private HudRenderer m_Hud;
        private PlayerRenderer m_PlayerRenderer;
        private ChunkComponent m_ChunkComp;
        private SkyComponent m_SkyComp;

        internal override Color BackColor => Color.Black;
        internal override UIRoot UI { get; set; }

        public PlayScene(WeltGame game, ChunkComponent chunks, SkyComponent sky, PlayerRenderer player) : base(game)
        {   
            UI = m_PlayUi;
            m_ChunkComp = chunks;
            m_SkyComp = sky;
            m_PlayerRenderer = player;
            m_Hud = new HudRenderer(GraphicsDevice, game.Client.World, m_PlayerRenderer);

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
            //Input.Assign(() => m_PlayerRenderer.Player.IsPaused = !m_PlayerRenderer.Player.IsPaused, Keys.Escape);
        }

        #endregion

        public override void OnExiting(object sender, EventArgs args)
        {
            
            base.OnExiting(sender, args);
        }

        #region LoadContent

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent(ContentManager content)
        {
            m_Hud.LoadContent(content);   
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
                m_SkyComp.Update(gameTime);
                m_ChunkComp.Update(gameTime);
                m_PlayerRenderer.Update(gameTime);
            }
            WeltGame.Instance.IsMouseVisible = Game.Client.IsPaused;
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            //m_SkyComp.Draw(gameTime);
            m_ChunkComp.Draw(gameTime);
            m_PlayerRenderer.Draw(gameTime);
            m_Hud.Draw(gameTime);
        }

        #endregion

        public override void Dispose()
        {
            m_PlayerRenderer = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Dispose();
        }

        #region Private methods
       
        private void SwitchToPlayUi()
        {
            UI = m_PlayUi;
            Game.Client.IsPaused = false;
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
