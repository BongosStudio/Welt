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
        private UIComponent[] _guiElements;
        private int _currentMenuIndex;
        
        private ColumnPanelComponent _hotbar;

        public GuiRenderer(GraphicsDevice device, Player player)
        {
            _graphics = device;
            _player = player;

            _hotbar = new ColumnPanelComponent("hotbar", (int) (WeltGame.Width*WeltGame.WidthViewRatio - 100), 40,
                device);

            var pauseMenu = new StackPanelComponent("pauseMenu", 100, -1, device,
                new ButtonComponent("Resume", "resumebtn", 100, 100, device),
                new ButtonComponent("Settings", "settingsbtn", 100, 100, device),
                new ButtonComponent("Quit", "quitbtn", 100, 100, device))
            {
                ChildHorizontalAlignment = HorizontalAlignment.Left
            };
            
            pauseMenu.ApplyToChildren(ButtonComponent.BorderWidthProperty, new BoundsBox(0, 4, 0, 0));
            pauseMenu.ApplyToChildren(ButtonComponent.BackgroundColorProperty, Color.DarkGray);
            pauseMenu.ApplyToChildren(ButtonComponent.ForegroundColorProperty, Color.White);
            pauseMenu.ApplyToChildren(ButtonComponent.TextHorizontalAlignmentProperty, HorizontalAlignment.Center);

            _guiElements = new UIComponent[]
            {
                pauseMenu
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
                BackgroundColor = Color.Gray,
            };
            
            _hotbar.ApplyToChildren(TextComponent.ForegroundProperty, Color.White);
        }

        public void Initialize()
        {
            foreach (var elem in _guiElements.Where(a => a != null))
            {
                elem.Initialize();
                _hotbar.Initialize();
            }
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