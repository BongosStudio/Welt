#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using EmptyKeys.UserInterface;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Input;
using EmptyKeys.UserInterface.Mvvm;
using GameUILibrary.Models;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.Forge;
using MainMenu = EmptyKeys.UserInterface.Generated.MainMenu;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        protected override Color BackColor => Color.GhostWhite;
        internal override UIRoot UI => new MainMenu();
        internal override ViewModelBase DataContext { get;  set; }

        public MainMenuScene(Game game) : base(game)
        {
            var viewModel = new MainMenuViewModel
            {              
                ExitButtonCommand = new RelayCommand(o => { Game.Exit(); }),
            };
            viewModel.SinglePlayerButtonCommand =
                new RelayCommand(delegate
                {
                    //SceneController.Load(new LoadScene(game, new WorldObject("DEMO WORLD")));
                    viewModel.SpMenuEnabled = true;
                });
            viewModel.MultiPlayerButtonCommand = 
                new RelayCommand(delegate
                {
                    viewModel.SpMenuEnabled = false;
                });
            viewModel.SettingsButtonCommand = 
                new RelayCommand(delegate
                {
                    viewModel.SpMenuEnabled = false; 
                    
                });
            viewModel.CreateNewWorldCommand = new RelayCommand(o =>
            {
                SceneController.Load(new LoadScene(game,
                    new WorldObject(viewModel.WorldName, 54321))); // TODO: fix the terrain generation. Maybe use Syhno's old style?
            });
                
            DataContext = viewModel;
        }
    }
}
