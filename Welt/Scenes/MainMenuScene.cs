#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Welt.Controllers;
using Welt.UI;

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        protected override Color BackColor => Color.GhostWhite;

        public MainMenuScene(Game game) : base(game)
        {

            AddComponent(new ImageComponent("Images/welt", "background", GraphicsDevice)
            {
                Opacity = 0.8f
            });
            
            var button = new ButtonComponent("Singleplayer", "spbutton", 300, 100, GraphicsDevice)
            {
                TextHorizontalAlignment = HorizontalAlignment.Center,
                BorderWidth = new BoundsBox(2, 2, 2, 2),
                BackgroundColor = Color.White,
                BackgroundActiveColor = Color.CadetBlue,
                ForegroundColor = Color.Black,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            button.MouseLeftDown += (sender, args) =>
            {
                SceneController.Load(new PlayScene(game));
            };

            AddComponent(button);
            
        }
    }
}
