#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI.Components
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
            var currentY = Y;

            foreach (var child in Components.Values)
            {
                child.Parent = this;
                if (child.Height == -1) child.Height = Height;
                if (child.Width == -1) child.Width = Width;
                child.Y = currentY + (int) child.Margin.Top;
                currentY = child.Y + (int) child.Margin.Bottom;
                switch (ChildHorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        child.X = X + (int) (Padding.Left + child.Margin.Left);
                        break;
                    case HorizontalAlignment.Center:
                        child.X = X + Width/2 + (int) (Padding.Left + child.Margin.Left);
                        break;
                    case HorizontalAlignment.Right:
                        child.X = X + Width - child.Width - (int) (Padding.Right + child.Margin.Right);
                        break;
                }
                child.IsSizeProcessed = true;
            }
            base.Initialize();
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