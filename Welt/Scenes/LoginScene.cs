#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.UI;
using Welt.UI.Components;

namespace Welt.Scenes
{
    public class LoginScene : Scene
    {
        public LoginScene(Game game) : base(game)
        {
            AddComponent(new TextComponent("", "errortxt", GraphicsDevice)
            {
                Foreground = Color.Red
            });

            var tic = new TextInputComponent("username", "usernamebox", 500, 40, GraphicsDevice)
            {
                Background = Color.SteelBlue,
                Foreground = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(40, 0, 0, 100),
                BorderWidth = new BoundsBox(5, 5, 5, 5),
                BorderColor = Color.Gray
            };
            
            var pbc = new PasswordBoxComponent("passwordbox", 500, 40, GraphicsDevice)
            {
                Background = Color.SteelBlue,
                Foreground = Color.White,
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(40, 0, 0, 50),
                BorderWidth = new BoundsBox(5, 5, 5, 5),
                BorderColor = Color.Gray,
                CanReceiveInput = false
            };

            var loginButton = new ButtonComponent("Login", "loginbtn", 100, 40, GraphicsDevice)
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(550, 0, 0, 100),
                BorderWidth = new BoundsBox(5, 5, 5, 5),
                BorderColor = Color.Gray,
                BackgroundColor = Color.SteelBlue,
                ForegroundColor = Color.White,
                TextHorizontalAlignment = HorizontalAlignment.Center,
                IsAllowedInput = false
            };

            var loadingStatus = new TextComponent("", "statustxt", GraphicsDevice)
            {
                VerticalAlignment = VerticalAlignment.Bottom,
                Margin = new BoundsBox(550, 0, 0, 79),
                Foreground = Color.White,
                Cursor = Cursors.Default
            };
            AddComponent(loadingStatus);

            tic.TextChanged += (sender, args) =>
            {
                pbc.CanReceiveInput = tic.Text.Length >= 4;
            };
            AddComponent(tic);

            pbc.LengthAccepted += (sender, args) =>
            {
                loginButton.IsAllowedInput = true;
            };

            pbc.LengthDenied += (sender, args) =>
            {
                loginButton.IsAllowedInput = false;
            };
            AddComponent(pbc);

            loginButton.MouseLeftDown += (sender, args) =>
            {
                if (GetComponent<TextInputComponent>("usernamebox").Value.Text == "")
                {
                    SetError("Username field cannot be empty.");
                    return;
                }
                // TODO: hook verification services right here and attach player details

                SetStatus("Loading...");
                Schedule(() =>
                {
                    SceneController.Load(new MainMenuScene(game));
                }, TimeSpan.FromSeconds(3));
            };
            AddComponent(loginButton);
        }

        private void SetError(string error)
        {
            GetComponent<TextComponent>("errortxt").Value.Text = error;
        }

        private void SetStatus(string status)
        {
            GetComponent<TextComponent>("statustxt").Value.Text = status;
        }
    }
}