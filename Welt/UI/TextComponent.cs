#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class TextComponent : UIComponent
    {
        private SpriteFont _spriteFont;

        public string Text { get; set; }
        public string Font { get; set; } = "Fonts/console";
        public Color Foreground { get; set; }

        public override float Opacity
        {
            get { return Foreground.A; }
            set { Foreground = Color.FromNonPremultiplied(Foreground.R, Foreground.G, Foreground.B, (int) (value*255)); }
        }

        public TextComponent(string text, string name, int width, int height, GraphicsDevice device)
            : this(text, name, width, height, null, device)
        {
        }

        public TextComponent(string text, string name, int width, int height, UIComponent parent, GraphicsDevice device)
            : base(name, width, height, parent, device)
        {
            Text = text; // TODO: process text like escaping or regexes? 
        }

        public override void Initialize()
        {
            _spriteFont = WeltGame.Instance.Content.Load<SpriteFont>(Font);
            base.Initialize();
        }

        public override void Draw(GameTime time)
        {
            Sprite.Begin();
            Sprite.DrawString(_spriteFont, Text, new Vector2(X, Y), Foreground);
            Sprite.End();
            base.Draw(time);
        }

        public override void Update(GameTime time)
        {
            var v = _spriteFont.MeasureString(Text);
            Width = (int) v.X;
            Height = (int) v.Y;
            base.Update(time);
        }
    }
}