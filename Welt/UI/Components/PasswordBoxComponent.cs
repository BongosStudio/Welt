#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Welt.UI.Components
{
    public class PasswordBoxComponent : UIComponent
    {
        public char ReplacementCharacter = '*';
        public bool IsSelected;
        public int MinimumLength = 6;
        public bool CanReceiveInput = true;
        public Color Background = Color.White;
        public Color Foreground = Color.Black;
        public Color InactiveColor = Color.Gray;
        public string Font = "Fonts/console";
        public BoundsBox BorderWidth = new BoundsBox();
        public Color BorderColor = Color.Black;
        public int Length => _text.Length;
        public override Cursor Cursor => Cursors.IBeam;

        private SpriteFont _spriteFont;
        private string _text = "";
        private string _fake = "";
        private bool _hasBeenTouched;
        private Texture2D _backgroundTexture;

        public PasswordBoxComponent(string name, int width, int height, GraphicsDevice device)
            : this(name, width, height, null, device)
        {

        }

        public PasswordBoxComponent(string name, int width, int height, UIComponent parent, GraphicsDevice device)
            : base(name, width, height, parent, device)
        {
        }

        public override void Initialize()
        {
            _spriteFont = WeltGame.Instance.Content.Load<SpriteFont>(Font);
            WeltGame.Instance.Window.TextInput += InputCharacter;
            _backgroundTexture = Effects.CreateSolidColorTexture(Graphics, Width, Height, Background, BorderWidth,
                BorderColor);
            MouseLeftDown += (sender, args) => IsSelected = true;
            base.Initialize();
        }

        public override void Draw(GameTime time)
        {
            if (!IsActive) return;
            Sprite.Begin();
            Sprite.Draw(_backgroundTexture, new Vector2(X, Y), Background);
            var color = CanReceiveInput ? Foreground : InactiveColor;
            if (_text.Length > 0)
            {
                Sprite.DrawString(_spriteFont, _fake, new Vector2(X + BorderWidth.Left, Y + BorderWidth.Top), color);
            }
            else if (!_hasBeenTouched)
            {
                Sprite.DrawString(_spriteFont, "password", new Vector2(X + BorderWidth.Left, Y + BorderWidth.Top), new Color(color, 0.5f));
            }
            Sprite.End();
            base.Draw(time);
        }

        private void InputCharacter(object sender, TextInputEventArgs args)
        {
            if (!IsSelected) return;
            var keyState = Keyboard.GetState();
            if (keyState[Keys.Enter] == KeyState.Down)
            {
                EnterKeyPressed?.Invoke(this, EventArgs.Empty);
                IsSelected = false;
                return;
            }
            if (keyState[Keys.Escape] == KeyState.Down)
            {
                IsSelected = false;
                return;
            }
            if (keyState[Keys.Back] == KeyState.Down)
            {
                if (!_hasBeenTouched)
                {
                    _hasBeenTouched = true;
                }
                switch (_text.Length)
                {
                    case 0:
                        return;
                    case 1:
                        _text = "";
                        _fake = "";
                        break;
                    default:
                        _text = _text.Remove(_text.Length - 1, 1);
                        _fake = _fake.Remove(_text.Length - 1, 1);
                        break;
                }
                return;
            }
            if (!_hasBeenTouched) _hasBeenTouched = true;
            _text += args.Character.ToString();
            _fake += ReplacementCharacter;
            if (Length >= MinimumLength) LengthAccepted?.Invoke(this, null);
            else LengthDenied?.Invoke(this, null);
        }

        public void ToggleSelected(bool value)
        {
            IsSelected = value;
        }

        public event EventHandler EnterKeyPressed;
        public event EventHandler LengthAccepted;
        public event EventHandler LengthDenied;

        public static UIProperty<bool> IsSelectedProperty = new UIProperty<bool>("isselected");
    }
}