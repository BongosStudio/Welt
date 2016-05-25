#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI.Components
{
    public class RectangleComponent : UIComponent
    {
        public BoundsBox BorderWidth { get; set; }
        public Color BackgroundColor { get; set; }
        public Color BorderColor { get; set; }

        public override float Opacity
        {
            get { return BackgroundColor.A; }
            set
            {
                BackgroundColor = Color.FromNonPremultiplied(BackgroundColor.R, BackgroundColor.G, BackgroundColor.B,
                    (int) (value*255));
            }
        }

        private Texture2D _backgroundImage;

        public RectangleComponent(string name, int width, int height, GraphicsDevice device) : this(name, width, height, null, device)
        {
        }

        public RectangleComponent(string name, int width, int height, UIComponent parent, GraphicsDevice device) : base(name, width, height, parent, device)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            _backgroundImage = Effects.CreateSolidColorTexture(Graphics, Width, Height, BackgroundColor, BorderWidth);

        }

        public override void Draw(GameTime time)
        {
            base.Draw(time);
            Sprite.Begin();
            Sprite.Draw(_backgroundImage, new Vector2(X, Y));
            Sprite.End();
        }
    }
}