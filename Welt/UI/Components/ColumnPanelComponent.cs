#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Linq;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI.Components
{
    public class ColumnPanelComponent : UIComponent
    {
        public VerticalAlignment ChildVerticalAlignment { get; set; }
        public Color BackgroundColor;

        protected Texture2D BackgroundTexture;

        public ColumnPanelComponent(string name, int width, int height, GraphicsDevice device,
            params UIComponent[] components) : base(name, width, height, device)
        {
            foreach (var child in components)
            {
                AddComponent(child);
            }
            MouseEnter += (sender, args) =>
            {
                Console.WriteLine("mouse entered");
            };
        }

        public override void Initialize()
        {
            ProcessArea();
            //base.Initialize();
            BackgroundTexture = Effects.CreateSolidColorTexture(Graphics, Width, Height, BackgroundColor);
            var currentX = X + (int) Padding.Left;

            foreach (var child in Components.Values)
            {
                child.Parent = this;
                if (child.Width == -2)
                {
                    child.Width = (int) (Width/Components.Count -
                                         Components.Values.Sum(component => component.Margin.Left + component.Margin.Right)/
                                         Components.Count);
                }
                var height = (float) child.Height;
                FastMath.Adjust(0f, Height - Padding.Top + Padding.Bottom, ref height);
                child.Height = (int) height;

                child.VerticalAlignment = ChildVerticalAlignment;

                currentX += (int) child.Margin.Left; // scale the left margin first
                child.X += currentX;
                currentX += child.Width + (int) child.Margin.Right; // then scale the right margin

                child.Y += Y;

                child.Initialize();
            }
        }

        public void ApplyToChildren<T>(UIProperty<T> property, T value)
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