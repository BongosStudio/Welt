#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Diagnostics.CodeAnalysis;
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
        public BoundsBox BorderWidth { get; set; }
        public override Cursor Cursor => IsAllowedInput ? Cursors.Hand : Cursor.Current;
        public bool IsAllowedInput;

        public override float Opacity
        {
            get { return IsAllowedInput ? _opacity : _opacity/2; }
            set
            {
                _opacity = value;
                ForegroundColor = new Color(ForegroundColor, value);
                ForegroundActiveColor = new Color(ForegroundActiveColor, value);
                BackgroundColor = new Color(BackgroundColor, value);
                BackgroundActiveColor = new Color(BackgroundActiveColor, value);
                BorderColor = new Color(BorderColor, value);
            }
        }

        public Color ForegroundColor { get; set; } = Color.Black;
        public Color ForegroundActiveColor { get; set; } = Color.White;
        public Color BackgroundColor { get; set; } = Color.Blue;
        public Color BackgroundActiveColor { get; set; } = Color.CornflowerBlue;
        public Color BorderColor { get; set; } = Color.Black;
        public Texture2D BackgroundImage { get; set; }

        private float _opacity;
        private Color _inactiveColor => new Color(Color.Black, Opacity);
        private Texture2D _inactiveTexture { get; set; }

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
            IsAllowedInput = true;
            _textPosition = GetTextPosition();
            
            if (BackgroundImage != null) return;
            BackgroundImage = new Texture2D(Graphics, Width, Height);
            _inactiveTexture = new Texture2D(Graphics, Width, Height);
            var colors = new Color[Width*Height];
            var iColors = new Color[Width*Height];
            for (var i = 0; i < colors.Length; i++)
            {
                var isBorder =
                    i%Width < BorderWidth.Left ||
                    i%Width >= Width - BorderWidth.Right ||
                    i <= Width*BorderWidth.Top ||
                    i >= colors.Length - Width*BorderWidth.Bottom;
                if (isBorder) colors[i] = BorderColor;
                else colors[i] = Color.White;
                iColors[i] = _inactiveColor;
            }
            _inactiveTexture.SetData(iColors);
            BackgroundImage.SetData(colors);

            
        }

        public override void Draw(GameTime time)
        {
            Sprite.Begin();

            Sprite.Draw(BackgroundImage, new Vector2(X, Y),
                IsMouseOver && IsAllowedInput ? BackgroundActiveColor : BackgroundColor);
            Sprite.DrawString(_font, Text, _textPosition, ForegroundColor);
            if (!IsAllowedInput) Sprite.Draw(_inactiveTexture, new Vector2(X, Y), _inactiveColor);
            Sprite.End();

            base.Draw(time);
        }

        [SuppressMessage("ReSharper", "PossibleLossOfFraction")]
        private Vector2 GetTextPosition()
        {
            var y = Y + Height/2 - _font.LineSpacing/2;
            switch (TextHorizontalAlignment)
            {
                case HorizontalAlignment.Left:
                    return new Vector2(X + BorderWidth.Left, y);
                case HorizontalAlignment.Center:
                    return new Vector2(X + (Width - _font.MeasureString(Text).X)/2, y);
                case HorizontalAlignment.Right:
                    return new Vector2(X + Width - _font.MeasureString(Text).X/2 + BorderWidth.Right, y);
                default:
                    return new Vector2(X, y);
            }
        }

        public static UIProperty BorderWidthProperty = new UIProperty("borderwidth");
        public static UIProperty BorderColorProperty = new UIProperty("bordercolor");
        public static UIProperty BackgroundColorProperty = new UIProperty("backgroundcolor");
        public static UIProperty ForegroundColorProperty = new UIProperty("foregroundcolor");
        public static UIProperty TextHorizontalAlignmentProperty = new UIProperty("texthorizontalalignment");
    }
}