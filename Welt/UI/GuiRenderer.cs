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
        private UIComponent[][] _guiElements;
        private ColumnPanelComponent _pauseMenu;

        public GuiRenderer(GraphicsDevice device, Player player)
        {
            _graphics = device;
            _player = player;
            _pauseMenu = new ColumnPanelComponent("pauseMenu", -1, -1, device,
                new ButtonComponent("Resume", "resumebtn", -2, 100, _pauseMenu, device),
                new ButtonComponent("Settings", "settingsbtn", -2, 100, _pauseMenu, device),
                new ButtonComponent("Quit", "quitbtn", -2, 100, _pauseMenu, device))
            {
                ChildVerticalAlignment = VerticalAlignment.Center
            };
            
            _pauseMenu.ApplyToChildren(ButtonComponent.BorderWidthProperty, new BoundsBox(1, 1, 5, 5));
            _pauseMenu.ApplyToChildren(ButtonComponent.BackgroundColorProperty, Color.DarkGray);
            _pauseMenu.ApplyToChildren(ButtonComponent.ForegroundColorProperty, Color.White);
            _pauseMenu.ApplyToChildren(ButtonComponent.TextHorizontalAlignmentProperty, HorizontalAlignment.Center);
        }

        public void Initialize()
        {
            _pauseMenu.Initialize();
        }

        public void Update(GameTime time)
        {
            if (_player.IsPaused) _pauseMenu.Update(time);
        }

        public void Draw(GameTime time)
        {
            if (_player.IsPaused)
            {
                _pauseMenu.Draw(time);
                WeltGame.SetCursor(Cursors.AppStarting);
            }
        }
    }
}