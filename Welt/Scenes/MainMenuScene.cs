#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using EmptyKeys.UserInterface.Generated;
using GameUILibrary.Models;
using Welt.Extensions;
using Welt.Core.Forge;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        private MainMenu MainMenuUi => new MainMenu
        {
            DataContext = MainMenuVm
        };
        private SingleplayerMenu SpUi => new SingleplayerMenu
        {
            DataContext = SpVm
        };
        private SettingsMenu SeUi => new SettingsMenu
        {
            DataContext = SeVm
        };

        private MainMenuViewModel MainMenuVm => new MainMenuViewModel
        {
            SingleplayerButtonCommand =
                new Action(SwitchToSingleplayer).CreateButtonCommand(),
            SettingsButtonCommand = 
                new Action(SwitchToSettings).CreateButtonCommand(),
            ExitButtonCommand =
                new Action(() => Game.Exit()).CreateButtonCommand(),
            
        };
        private SingleplayerViewModel SpVm => new SingleplayerViewModel
        {
            WorldName = "New World",
            WorldSeed = "12345",
            CreateWorldCommand = 
                new Action(() => Next(new LoadScene(Game, new World(SpVm.WorldName)))).CreateButtonCommand(),
            ExitCommand = 
                new Action(SwitchToMainMenu).CreateButtonCommand()
        };

        private SettingsModel SeVm => new SettingsModel
        {
            ExitCommand = 
                new Action(SwitchToMainMenu).CreateButtonCommand()
        };

        internal override ViewModelBase DataContext { get; set; }
        internal override UIRoot UI { get; set; } = new MainMenu();
        internal override Color BackColor => Color.FromNonPremultiplied(15, 15, 15, 255);

        public MainMenuScene(WeltGame game) : base(game)
        {
            SwitchToMainMenu();
        }

        public override void Update(GameTime gameTime)
        {
            
        }

        public override void Draw(GameTime time)
        {
            
        }

        public override void Initialize()
        {
            
        }

        private void SwitchToMainMenu()
        {
            UI = MainMenuUi;
            //UI.DataContext = MainMenuVm;
        }

        private void SwitchToSingleplayer()
        {
            UI = SpUi;
            //UI.DataContext = SpVm;
        }

        private void SwitchToMultiplayer()
        {

        }

        private void SwitchToSettings()
        {
            UI = SeUi;
            //UI.DataContext = SeVm;
        }
    }
}
