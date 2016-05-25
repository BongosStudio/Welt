#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Welt.Controllers;
using Welt.Forge;
<<<<<<< HEAD
using Welt.Forge.Renderers;
using Welt.UI;
using Welt.UI.Components;
=======
using Welt.UI;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt.Scenes
{
    public class MainMenuScene : Scene
    {
        protected override Color BackColor => Color.GhostWhite;

        public MainMenuScene(Game game) : base(game)
        {
<<<<<<< HEAD
            //game.Window.AllowUserResizing = true;
=======

>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            AddComponent(new ImageComponent("Images/welt", "background", GraphicsDevice)
            {
                Opacity = 0.8f
            });
            
            var button = new ButtonComponent("Singleplayer", "spbutton", 300, 100, GraphicsDevice)
            {
                TextHorizontalAlignment = HorizontalAlignment.Center,
                BorderWidth = new BoundsBox(2, 2, 2, 2),
                BackgroundColor = Color.White,
<<<<<<< HEAD
                BackgroundActiveColor = Color.MediumAquamarine,
=======
                BackgroundActiveColor = Color.CadetBlue,
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
                ForegroundColor = Color.Black,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            button.MouseLeftDown += (sender, args) =>
            {
<<<<<<< HEAD
                SceneController.Load(new LoadScene(game, new World("DEMO WORLD"))); // TODO: fetch world data
=======
                SceneController.Load(new LoadScene(game, new World())); // TODO: fetch world data
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            };

            AddComponent(button);
            
        }
    }
}
