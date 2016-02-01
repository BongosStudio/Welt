#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Remoting.Channels;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class ButtonComponent : UIComponent
    {
        public string Text { get; set; }
        public HorizontalAlignment TextHorizontalAlignment { get; set; } = HorizontalAlignment.Left;
        public string Font { get; set; } = "Fonts/console";
        public float FontSize { get; set; } = 16f; // TODO
        public float BorderWidth { get; set; }
        public override Cursor Cursor => Cursors.Hand;

        public Color ForegroundColor { get; set; } = Color.Black;
        public Color ForegroundActiveColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.Blue;
        public Color BackgroundActiveColor { get; set; } = Color.CornflowerBlue;
        public Color BorderColor { get; set; } = Color.Gray;
        public Texture2D BackgroundImage { get; set; }

        private readonly SpriteFont _font;
        private Vector2 _textPosition;

        public ButtonComponent(string text, string name, int width, int height, GraphicsDevice device)
            : this(text, name, width, height, null, device)
        {
        }

        public ButtonComponent(string text, string name, int width, int height, UIComponent parent,
            GraphicsDevice device) : base(name, width, height, parent, device)
        {
            Sprite = new SpriteBatch(device);
            _font = WeltGame.Instance.Content.Load<SpriteFont>(Font);
            Text = text;
        }

        public override void Initialize()
        {
            base.Initialize();
            MouseLeftDown += (sender, args) => ButtonPressed?.Invoke(sender, null);
            _textPosition = GetTextPosition();
            
            if (BackgroundImage != null) return;
            var rect = new Texture2D(Graphics, Width, Height);
            var colors = new Color[Width*Height];
            for (var i = 0; i < colors.Length; i++)
            {
                // TODO borders? Maybe?
                var isBorder =
                    i%Width < BorderWidth*100 ||
                    i%Width >= Width - BorderWidth*100 ||
                    i <= Width*BorderWidth*100 ||
                    i >= colors.Length - Width*BorderWidth*100;
                if (isBorder) colors[i] = BorderColor;
                else colors[i] = Color.White;
            }
            
            rect.SetData(colors);
            BackgroundImage = rect;
        }

        public override void Draw(GameTime time)
        {
            Sprite.Begin();
            
            Sprite.Draw(BackgroundImage, new Vector2(X, Y), IsMouseOver ? BackgroundActiveColor : BackgroundColor);
            Sprite.DrawString(_font, Text, _textPosition, ForegroundColor);
            Sprite.End();

            base.Draw(time);
        }

        public event EventHandler ButtonPressed;

        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        private Vector2 GetTextPosition()
        {
            var y = Y + Height/2 - _font.LineSpacing/2;
            switch (TextHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return new Vector2(X, y);
                case HorizontalAlignment.Center:
                    return new Vector2(X + _font.MeasureString(Text).X/2, y);
                case HorizontalAlignment.Right:
                    return new Vector2(X + Width - _font.MeasureString(Text).X/2, y);
                default:
                    return new Vector2(X, y);
            }
        }
    }
}