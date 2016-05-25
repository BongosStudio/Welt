#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Models;
using Welt.UI.Components;

namespace Welt.UI
{
    public class GuiRenderer
    {
        private GraphicsDevice _graphics;
        private Player _player;
        private UIComponent[][] _guiElements;
        private int _currentMenuIndex;
        private ColumnPanelComponent _hotbar;

        public GuiRenderer(GraphicsDevice device, Player player)
        {
            _graphics = device;
            _player = player;

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
            {
                ChildVerticalAlignment = VerticalAlignment.Bottom,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
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
        }

        public void Initialize()
        {
            foreach (var elem in _guiElements.Where(a => a != null).SelectMany(t => t))
            {
                elem.Initialize();
            }
            _hotbar.Initialize();
        }

        public void Update(GameTime time)
        {
            if (_player.IsPaused)
            {
                foreach (var element in _guiElements[_currentMenuIndex])
                {
                    element.Update(time);
                }
            }
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
                foreach (var element in _guiElements[_currentMenuIndex])
                {
                    element.Draw(time);
                }
            }
            else
            {
                _hotbar.Draw(time);
            }
        }
    }
}