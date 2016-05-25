#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
<<<<<<< HEAD
using System.Linq;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Models;
<<<<<<< HEAD
using Welt.UI.Components;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt.UI
{
    public class GuiRenderer
    {
        private GraphicsDevice _graphics;
        private Player _player;
<<<<<<< HEAD
        private UIComponent[][] _guiElements;
        private int _currentMenuIndex;
=======
        private UIComponent[] _guiElements;
        private int _currentMenuIndex;

>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        private ColumnPanelComponent _hotbar;

        public GuiRenderer(GraphicsDevice device, Player player)
        {
            _graphics = device;
            _player = player;
<<<<<<< HEAD

            _guiElements = new UIComponent[3][];
            _guiElements[0] = new UIComponent[]
            {
                new ButtonComponent("Resume", "resumebtn", 200, 80, _graphics)
                {
                    BackgroundColor = Color.Gray,
                    ForegroundColor = Color.White,
                    BorderWidth = new BoundsBox(0, 5, 0, 0),
                    BorderColor = Color.Aquamarine,
                    TextHorizontalAlignment = HorizontalAlignment.Right,
                    Padding = new BoundsBox(0, 30, 0, 0),
                    IsAllowedInput = true
                },
                new ButtonComponent("Options", "optionsbtn", 200, 80, _graphics)
                {
                    BackgroundColor = Color.Gray,
                    ForegroundColor = Color.White,
                    Y = 80,
                    BorderWidth = new BoundsBox(0, 5, 0, 0),
                    BorderColor = Color.Aquamarine,
                    TextHorizontalAlignment = HorizontalAlignment.Right,
                    Padding = new BoundsBox(0, 30, 0, 0)
                },
                new ButtonComponent("Quit", "quitbtn", 200, 80, _graphics)
                {
                    BackgroundColor = Color.Gray,
                    ForegroundColor = Color.White,
                    Y = 160,
                    BorderWidth = new BoundsBox(0, 5, 0, 0),
                    BorderColor = Color.Aquamarine,
                    TextHorizontalAlignment = HorizontalAlignment.Right,
                    Padding = new BoundsBox(0, 30, 0, 0)
                },
            };
            //_hotbar = new ColumnPanelComponent("hotbar", -1, 40, device,
            //    new TextComponent("1", "hb1", -2, 30, device),
            //    new TextComponent("2", "hb2", -2, 30, device),
            //    new TextComponent("3", "hb3", -2, 30, device),
            //    new TextComponent("4", "hb4", -2, 30, device),
            //    new TextComponent("5", "hb5", -2, 30, device),
            //    new TextComponent("6", "hb6", -2, 30, device),
            //    new TextComponent("7", "hb7", -2, 30, device),
            //    new TextComponent("8", "hb8", -2, 30, device),
            //    new TextComponent("9", "hb9", -2, 30, device),
            //    new TextComponent("0", "hb0", -2, 30, device)
            //    )
            //{
            //    ChildVerticalAlignment = VerticalAlignment.Bottom,
            //    VerticalAlignment = VerticalAlignment.Bottom,
            //    HorizontalAlignment = HorizontalAlignment.Center,
            //    BackgroundColor = Color.Gray
            //};

            _hotbar = new ColumnPanelComponent("hotbar", (int) (WeltGame.Width*WeltGame.WidthViewRatio -100), 40, device)
=======
            

            var _pauseMenu = new ColumnPanelComponent("pauseMenu", -1, -1, device,
                new ButtonComponent("Resume", "resumebtn", -2, 100, device),
                new ButtonComponent("Settings", "settingsbtn", -2, 100, device),
                new ButtonComponent("Quit", "quitbtn", -2, 100, device))
            {
                ChildVerticalAlignment = VerticalAlignment.Center,
                BackgroundColor = Color.Green,
                Padding = new BoundsBox(10, 10, 10, 10)
            };
            
            _pauseMenu.ApplyToChildren(ButtonComponent.BorderWidthProperty, new BoundsBox(1, 1, 5, 5));
            _pauseMenu.ApplyToChildren(ButtonComponent.BackgroundColorProperty, Color.DarkGray);
            _pauseMenu.ApplyToChildren(ButtonComponent.ForegroundColorProperty, Color.White);
            _pauseMenu.ApplyToChildren(ButtonComponent.TextHorizontalAlignmentProperty, HorizontalAlignment.Center);

            _guiElements = new UIComponent[]
            {
                _pauseMenu
            };

            _hotbar = new ColumnPanelComponent("hotbar", -1, 40, device,
                new TextComponent("1", "hb1", -2, 30, device),
                new TextComponent("2", "hb2", -2, 30, device),
                new TextComponent("3", "hb3", -2, 30, device),
                new TextComponent("4", "hb4", -2, 30, device),
                new TextComponent("5", "hb5", -2, 30, device),
                new TextComponent("6", "hb6", -2, 30, device),
                new TextComponent("7", "hb7", -2, 30, device),
                new TextComponent("8", "hb8", -2, 30, device),
                new TextComponent("9", "hb9", -2, 30, device),
                new TextComponent("0", "hb0", -2, 30, device)
                )
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            {
                ChildVerticalAlignment = VerticalAlignment.Bottom,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
<<<<<<< HEAD
                BackgroundColor = Color.Gray,
            };
            var i = 0;
            _hotbar.PopulateWith(new TextComponent(i.ToString(), $"hb{i}", -2, 30, _hotbar, device)
            {
                Foreground = Color.White,
                HorizontalAlignment = HorizontalAlignment.Center
            }, 10, () =>
            {
                i++;
            });
=======
                BackgroundColor = Color.BlueViolet
            };

            _hotbar.ApplyToChildren(TextComponent.ForegroundProperty, Color.White);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        }

        public void Initialize()
        {
<<<<<<< HEAD
            foreach (var elem in _guiElements.Where(a => a != null).SelectMany(t => t))
=======
            foreach (var elem in _guiElements)
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            {
                elem.Initialize();
            }
            _hotbar.Initialize();
        }

        public void Update(GameTime time)
        {
<<<<<<< HEAD
            if (_player.IsPaused)
            {
                foreach (var element in _guiElements[_currentMenuIndex])
                {
                    element.Update(time);
                }
            }
=======
            if (_player.IsPaused) _guiElements[_currentMenuIndex].Update(time);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            else
            {
                _currentMenuIndex = 0;
                _hotbar.Update(time);
            }
        }

        public void Draw(GameTime time)
        {
            if (_player.IsPaused)
            {
<<<<<<< HEAD
                foreach (var element in _guiElements[_currentMenuIndex])
                {
                    element.Draw(time);
                }
=======
                _guiElements[_currentMenuIndex].Draw(time);
                //WeltGame.SetCursor(Cursors.AppStarting);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            }
            else
            {
                _hotbar.Draw(time);
            }
        }
    }
}