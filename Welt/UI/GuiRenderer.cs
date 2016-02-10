#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Models;

namespace Welt.UI
{
    public class GuiRenderer
    {
        private GraphicsDevice _graphics;
        private Player _player;
        private UIComponent[] _guiElements;
        private int _currentMenuIndex;

        private ColumnPanelComponent _hotbar;

        public GuiRenderer(GraphicsDevice device, Player player)
        {
            _graphics = device;
            _player = player;
            

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
            {
                ChildVerticalAlignment = VerticalAlignment.Bottom,
                VerticalAlignment = VerticalAlignment.Bottom,
                HorizontalAlignment = HorizontalAlignment.Center,
                BackgroundColor = Color.BlueViolet
            };

            _hotbar.ApplyToChildren(TextComponent.ForegroundProperty, Color.White);
        }

        public void Initialize()
        {
            foreach (var elem in _guiElements)
            {
                elem.Initialize();
            }
            _hotbar.Initialize();
        }

        public void Update(GameTime time)
        {
            if (_player.IsPaused) _guiElements[_currentMenuIndex].Update(time);
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
                _guiElements[_currentMenuIndex].Draw(time);
                //WeltGame.SetCursor(Cursors.AppStarting);
            }
            else
            {
                _hotbar.Draw(time);
            }
        }
    }
}