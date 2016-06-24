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
            //game.Window.AllowUserResizing = true;
            //AddComponent(new ImageComponent("Images/welt", "background", GraphicsDevice)
            //{
            //    Opacity = 0.8f
            //});

            //var button = new ButtonComponent("Singleplayer", "spbutton", 300, 100, GraphicsDevice)
            //{
            //    TextHorizontalAlignment = HorizontalAlignment.Center,
            //    BorderWidth = new BoundsBox(2, 2, 2, 2),
            //    BackgroundActiveColor = Color.CadetBlue,
            //    BackgroundColor = Color.LightSteelBlue,
            //    ForegroundColor = Color.Black,
            //    VerticalAlignment = VerticalAlignment.Center,
            //    HorizontalAlignment = HorizontalAlignment.Center
            //};

            //button.MouseLeftDown += (sender, args) =>
            //{
            //    SceneController.Load(new LoadScene(game, new World("DEMO WORLD"))); // TODO: fetch world data
            //};

            //AddComponent(button);
            var viewModel = new MainMenuViewModel
            {              
                ExitButtonCommand = new RelayCommand(o => { Game.Exit(); })
            };
            viewModel.SinglePlayerButtonCommand =
                new RelayCommand(o =>
                {
                    //SceneController.Load(new LoadScene(game, new World("DEMO WORLD")));
                    viewModel.SpMenuEnabled = true;
                });
            viewModel.MultiPlayerButtonCommand = 
                new RelayCommand(o =>
                {
                    viewModel.SpMenuEnabled = false;
                });
            viewModel.SettingsButtonCommand = 
                new RelayCommand(o =>
                {
                    viewModel.SpMenuEnabled = false;
                });
                
            DataContext = viewModel;
        }

        public override void Initialize()
        {
            
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            
            base.Update(gameTime);

        }

        public override void Draw(GameTime time)
        {
            base.Draw(time);
        }
        
    }
}
