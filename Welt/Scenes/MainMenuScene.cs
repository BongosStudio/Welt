#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.UI;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        protected override Color BackColor => Color.GhostWhite;

        public MainMenuScene(Game game) : base(game)
        {
            
            AddComponent(new ImageComponent("Images/welt", "background", game.GraphicsDevice)
            {
                Opacity = 0.8f
            });
            var button = new ButtonComponent("Singleplayer", "spbutton", 300, 100, GraphicsDevice)
            {
                TextHorizontalAlignment = HorizontalAlignment.Center,
                BorderWidth = 2f,
                BackgroundColor = Color.Black,
                BackgroundActiveColor = Color.DarkGray,
                ForegroundColor = Color.White,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            button.ButtonPressed += (sender, args) =>
            {
                SceneController.Load(new PlayScene(game));
            };
            AddComponent(button);
        }
    }
}
