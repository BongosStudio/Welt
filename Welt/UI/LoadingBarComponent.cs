#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.UI
{
    public class LoadingBarComponent : UIComponent
    {
        public static UIProperty CurrentValueProperty = new UIProperty("currentvalue");

        public int CurrentValue { get; private set; }
        public Color Foreground { get; set; }
        public Color Background { get; set; }

        private int _maxValue;
        private int _incr;
        private int _widthIncr;
        private Texture2D _texture;

        #region Background Texture

        private Rectangle _backgroundR;

        #endregion

        #region Loading Texture

        private Rectangle _foregroundR;

        #endregion

        public LoadingBarComponent(int maxValue, int increment, string name, int width, int height, GraphicsDevice device)
            : this(maxValue, increment, name, width, height, null, device)
        {
        }

        public LoadingBarComponent(int maxValue, int increment, string name, int width, int height, UIComponent parent, GraphicsDevice device)
            : base(name, width, height, parent, device)
        {
            _maxValue = maxValue;
            _incr = increment;
        }

        public override void Initialize()
        {
            base.Initialize();
            CurrentValue = _incr;
            _widthIncr = Width/_incr;
            _backgroundR = new Rectangle(X, Y, Width, Height);
            _foregroundR = new Rectangle(X, Y, CurrentValue, Height);
            _texture = new Texture2D(Graphics, Width, Height);
            var colors = new Color[Width*Height];
            for (var i = 0; i < colors.Length; i++)
            {
                colors[i] = Color.Black;
            }
            _texture.SetData(colors);
        }

        public override void Update(GameTime time)
        {
            base.Update(time);
        }

        public override void Draw(GameTime time)
        {
            Sprite.Begin();
            Sprite.Draw(_texture, new Vector2(X, Y), _backgroundR, Background);
            Sprite.Draw(_texture, new Vector2(X, Y), _foregroundR, Foreground);
            Sprite.End();

            base.Draw(time);
        }

        public void Increment()
        {
            CurrentValue += _incr;
            _foregroundR = new Rectangle(X, Y, _foregroundR.Width += _widthIncr, Height);
        }
    }
}