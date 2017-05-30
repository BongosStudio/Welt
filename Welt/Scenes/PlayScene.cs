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
using System.IO;
using System.Diagnostics;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Welt.Core.Extensions;
using Welt.API.Forge;
using Welt.Core;

namespace Welt.Scenes
{
    public class PlayScene : Scene
    {
        public static PlayScene Instance;

        private PlayUI PlayUI => new PlayUI
        {
            DataContext = m_PlayViewModel
        };
        private Pause PauseUI => new Pause
        {
            DataContext = m_PauseViewModel
        };
        private SettingsMenu SettingsUI => new SettingsMenu
        {
            DataContext = m_SettingsViewModel
        };

        private PlayViewModel m_PlayViewModel = new PlayViewModel();
        private PauseViewModel m_PauseViewModel = new PauseViewModel();
        private SettingsModel m_SettingsViewModel = new SettingsModel();
        
        private HudRenderer m_Hud;
        private PlayerRenderer m_PlayerRenderer;
        private ChunkComponent m_ChunkComp;
        private SkyComponent m_SkyComp;
        private FpsComponent m_Fps;

        private bool m_TooltipReady = true;
        private Queue<string> m_TooltipQueue;

        internal override Color BackColor => Color.Blue;
        internal override UIRoot UI { get; set; }

        public PlayScene(WeltGame game, ChunkComponent chunks, SkyComponent sky, PlayerRenderer player) : base(game)
        {   
            UI = PlayUI;
            m_ChunkComp = chunks;
            m_SkyComp = sky;
            m_PlayerRenderer = player;
            m_Hud = new HudRenderer(GraphicsDevice, game.Client.World, m_PlayerRenderer);
            m_Fps = new FpsComponent(game);
            m_TooltipQueue = new Queue<string>();
            Instance = this;
        }

        private async void ShowNextTooltip()
        {
            while (!m_TooltipReady)
            {
                await Task.Delay(500);
            }
            if (m_TooltipQueue.Count == 0) return;
            m_TooltipReady = false;
            var tooltip = m_TooltipQueue.Dequeue();
            m_PlayViewModel.TooltipOpacity = 0;
            m_PlayViewModel.TooltipText = tooltip;
            while (m_PlayViewModel.TooltipOpacity < 1)
            {
                m_PlayViewModel.TooltipOpacity += 0.05;
                await Task.Delay(20);
            }
            await Task.Delay(3000);
            while (m_PlayViewModel.TooltipOpacity > 0)
            {
                m_PlayViewModel.TooltipOpacity -= 0.05;
                await Task.Delay(100);
            }
            m_TooltipReady = true;
        }

        internal void ShowTooltip(string tooltip)
        {
            if (tooltip == null) return;
            m_TooltipQueue.Enqueue(tooltip);
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
            RegisterViewModels();
            // all world/sky renderers will be initialized before this in the loading scene
            m_PlayerRenderer.Initialize();
            m_Hud.Initialize();
            //m_PlayerRenderer.Player._Position = m_PlayerRenderer.Player.World.World.GetSpawnPosition();
            m_Fps.Initialize();
            SwitchToPlayUi();

            m_PlayerRenderer.Player.Inventory = new InventoryContainer
            {
                new ItemStack(new Block(BlockType.DIRT)),
                new ItemStack(new Block(BlockType.STONE))
            };

            Input.RegisterHandler(ToggleFlight, InputController.InputAction.Flight);
            Input.RegisterHandler(TogglePause, InputController.InputAction.Escape);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 0); }, InputController.InputAction.Hotbar0);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 1); }, InputController.InputAction.Hotbar1);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 2); }, InputController.InputAction.Hotbar2);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 3); }, InputController.InputAction.Hotbar3);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 4); }, InputController.InputAction.Hotbar4);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 5); }, InputController.InputAction.Hotbar5);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 6); }, InputController.InputAction.Hotbar6);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 7); }, InputController.InputAction.Hotbar7);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 8); }, InputController.InputAction.Hotbar8);
            Input.RegisterHandler((client) => { return ToggleSelection(client, 9); }, InputController.InputAction.Hotbar9);
            Input.RegisterHandler(HitAction, InputController.InputAction.Hit);
            Input.RegisterHandler(PlaceAction, InputController.InputAction.Place);

            Game.ShowTooltip("Welcome to Welt!");
            Game.ShowTooltip("To move around, you can use the standard WSAD controls.");
            Game.ShowTooltip($"To leave flight-mode, press {Input.GetKeyFor(InputController.InputAction.Flight)}.");
            Game.ShowTooltip($"To run, press {Input.GetKeyFor(InputController.InputAction.Sprint)}. " +
                $"To jump, press {Input.GetKeyFor(InputController.InputAction.Jump)}. " +
                $"In flight-mode, these are your ascend and decend controls.");
        }

        #endregion

        private void UpdateViewModels()
        {
            m_PlayViewModel.LookingAt = $"{m_PlayerRenderer.Player.BlockRepository.GetBlockProvider(m_PlayerRenderer.LookingAt.Id).Name}";
            m_PlayViewModel.SelectedItemName = m_PlayerRenderer.Player.BlockRepository.GetBlockProvider(m_PlayerRenderer.Player.SelectedItem.Block.Id).Name;
            
            // TODO: copy settings over to viewmodel
        }

        private void RegisterViewModels()
        {
            m_PlayViewModel = new PlayViewModel
            {
                   
            };
            m_PauseViewModel = new PauseViewModel
            {
                ResumeButtonCommand =
                new Action(SwitchToPlayUi).CreateButtonCommand(),
                OptionsButtonCommand =
                new Action(SwitchToSettingsUi).CreateButtonCommand(),
                QuitButtonCommand =
                new Action(() => Next(new MainMenuScene(Game))).CreateButtonCommand()
            };
            m_SettingsViewModel = new SettingsModel
            {
                ExitCommand =
                new Action(SwitchToPauseUi).CreateButtonCommand()
            };
        }

        #region Input Handlers

        private static bool ToggleFlight(MultiplayerClient client)
        {
            //TODO: check permission
            client.IsFlying = !client.IsFlying;
            return true;
        }

        private bool TogglePause(MultiplayerClient client)
        {
            client.IsPaused = !client.IsPaused;
            if (client.IsPaused) SwitchToPauseUi();
            else SwitchToPlayUi();
            return true;
        }

        private static bool ToggleSelection(MultiplayerClient client, int index)
        {
            FastMath.Adjust(0, client.Inventory.Count - 1, ref index);
            client.HotbarSelection = index;
            return true;
        }

        private bool HitAction(MultiplayerClient client)
        {
            client.BreakBlock(m_PlayerRenderer.LookingAtPosition);
            return true;
        }

        private bool PlaceAction(MultiplayerClient client)
        {
            client.PlaceCurrentBlock(m_PlayerRenderer.LookingAtNeighbor);
            return true;
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
            m_Fps.LoadContent(content);
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
            WeltGame.Instance.IsMouseVisible = Game.Client.IsPaused;
            UpdateViewModels();
            if (Game.IsActive)
            {
                Input.Update(gameTime);
                m_SkyComp.Update(gameTime);
                m_ChunkComp.Update(gameTime);
                m_PlayerRenderer.Update(gameTime);
                m_Fps.Update(gameTime);
                ShowNextTooltip();
            }
        }

        #endregion

        #region Draw

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        public override void Draw(GameTime gameTime)
        {
            m_SkyComp.Draw(gameTime);
            m_ChunkComp.Draw(gameTime);
            m_PlayerRenderer.Draw(gameTime);
            m_Hud.Draw(gameTime);
            m_Fps.Draw(gameTime);
        }

        #endregion

        public override void Dispose()
        {
            Game.Client.Disconnect();
            m_PlayerRenderer = null;
            GC.WaitForPendingFinalizers();
            GC.Collect();
            base.Dispose();
        }

        #region Private methods
       
        private void SwitchToPlayUi()
        {
            UI = PlayUI;
            Game.Client.IsPaused = false;
        }

        private void SwitchToPauseUi()
        {
            UI = PauseUI;
        }

        private void SwitchToSettingsUi()
        {
            UI = SettingsUI;
        }

        #endregion
    }
}
