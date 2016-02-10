#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class StackPanelComponent : UIComponent
    {
        public HorizontalAlignment ChildHorizontalAlignment { get; set; }
        public Color BackgroundColor;

        protected Texture2D BackgroundTexture;

        public StackPanelComponent(string name, int width, int height, GraphicsDevice device,
            params UIComponent[] components) : base(name, width, height, device)
        {
            Components = components.ToDictionary(component => component.Name, component => component);
        }

        public override void Initialize()
        {
            ProcessArea();
            //base.Initialize();
            BackgroundTexture = Effects.CreateSolidColorTexture(Graphics, Width, Height, BackgroundColor);
            var currentY = Y + (int) Padding.Top;

            foreach (var child in Components.Values)
            {
                child.Parent = this;
                if (child.Height == -2)
                {
                    child.Height = (int) (Height/Components.Count -
                                          Components.Values.Sum(component => component.Margin.Top + component.Margin.Bottom)/
                                          Components.Count);
                }
                var width = (float)child.Width;
                FastMath.Adjust(0f, Width - Padding.Left + Padding.Right, ref width);
                child.Width = (int) width;

                child.HorizontalAlignment = ChildHorizontalAlignment;
                child.VerticalAlignment = VerticalAlignment.Top; // stack panel components cannot have VA other than top.
                currentY += (int) child.Margin.Top; // scale the top margin first
                child.Y += currentY;
                currentY += child.Height + (int) child.Margin.Bottom; // then scale the bottom margin
                child.Initialize();
            }
        }

        public void ApplyToChildren(UIProperty property, object value)
        {
            foreach (var child in Components.Values)
            {
                child.SetPropertyValue(property, value);
            }
        }

        public override void Draw(GameTime time)
        {
            Sprite.Begin();
            Sprite.Draw(BackgroundTexture, new Vector2(X, Y), BackgroundColor);
            Sprite.End();

            base.Draw(time);
        }
    }
}