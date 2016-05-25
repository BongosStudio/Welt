#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI.Components
{
    public class CheckboxComponent : UIComponent
    {
        public bool IsChecked { get; set; }
        public string Text { get; set; }

        public CheckboxComponent(string text, string name, int width, int height, GraphicsDevice device)
            : this(text, name, width, height, null, device)
        {
        }

        public CheckboxComponent(string text, string name, int width, int height, UIComponent parent, GraphicsDevice device)
            : base(name, width, height, parent, device)
        {
            Text = text;
            //AddComponent(new ImageComponent());
        }

        public event EventHandler Toggled;

        public void OnToggled(object sender, EventArgs args)
        {
            Toggled?.Invoke(sender, args);
        }
    }
}