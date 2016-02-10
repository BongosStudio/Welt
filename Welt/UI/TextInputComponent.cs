#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.IO;
using Color = Microsoft.Xna.Framework.Color;
using Keys = Microsoft.Xna.Framework.Input.Keys;
using Point = System.Drawing.Point;

namespace Welt.UI
{
    public class TextInputComponent : UIComponent
    {
        private SpriteFont _spriteFont;
        private readonly List<string> _text;
        private TimeSpan _cursorFlash;
        private bool _isCursorFlashing;
        private KeyboardState _oKeyState;
        private Texture2D _backgroundTexture;

        public override Cursor Cursor => Cursors.IBeam;
        public bool IsSelected;
        public string Text => _text.Aggregate((s, s1) => $"{s} {s1}");
        public Color Foreground { get; set; }
        public Color Background { get; set; }
        public int LineIndex { get; set; }
        public int CharacterIndex { get; set; }
        public TextAlignment TextAlignment { get; set; }
        public string Font { get; set; } = "Fonts/console";

        public override float Opacity
        {
            get { return Foreground.A; }
            set { Foreground = Color.FromNonPremultiplied(Foreground.R, Foreground.G, Foreground.B, (int) (value*255)); }
        }

        public TextInputComponent(string text, string name, int width, int height, GraphicsDevice device) : this(text, name, width, height, null, device)
        {
        }

        public TextInputComponent(string text, string name, int width, int height, UIComponent parent, GraphicsDevice device) : base(name, width, height, parent, device)
        {
            _text = new List<string> {text};
            IsSelected = true; // TODO: this is for testing only
        }

        public override void Initialize()
        {
            _spriteFont = WeltGame.Instance.Content.Load<SpriteFont>(Font);           
            WeltGame.Instance.Window.TextInput += InputCharacter;
            MouseLeftDown += ClickCharacter;
            LineIndex = _text.Count - 1;
            CharacterIndex = _text.Last().Length;
            base.Initialize();
            _oKeyState = Keyboard.GetState();
            _backgroundTexture = Effects.CreateSolidColorTexture(Graphics, Width, Height, Background);
        }

        public override void Draw(GameTime time)
        {
            // first adjust indices
            if (LineIndex >= _text.Count) LineIndex = _text.Count - 1;
            if (_text[LineIndex].Length < CharacterIndex) CharacterIndex = _text[LineIndex].Length;

            var builder = new StringBuilder();
            var h = _spriteFont.LineSpacing*_text.Count;
            if (!IsSelected) _isCursorFlashing = false;
            for (var i = 0; i < _text.Count; i++)
            {
                builder.AppendLine(i == LineIndex ? _text[i].Insert(CharacterIndex, _isCursorFlashing ? "_" : "") : _text[i]);
            }

            if (h > Height) Height = h;
            if (!IsActive) return;
            var stateColor = IsActive ? Foreground : new Color(Foreground, 0.5f);
            if (_text.Count > 0)
            {
                Sprite.Begin();
                Sprite.Draw(_backgroundTexture, new Vector2(X, Y), Background);
                Sprite.DrawString(_spriteFont, builder, new Vector2(X, Y), stateColor);
                Sprite.End();
            }

            base.Draw(time);
        }

        public override void Update(GameTime time)
        {
            
            // update the cursor flash
            if (_cursorFlash <= TimeSpan.Zero)
            {
                _cursorFlash = TimeSpan.FromMilliseconds(750);
                _isCursorFlashing = !_isCursorFlashing;
            }
            else
            {
                _cursorFlash -= time.ElapsedGameTime;
            }

            base.Update(time);         

            var keyState = Keyboard.GetState();
            if (keyState[Keys.Left] == KeyState.Down && _oKeyState[Keys.Left] == KeyState.Up) ShiftLeft();
            if (keyState[Keys.Right] == KeyState.Down && _oKeyState[Keys.Right] == KeyState.Up) ShiftRight();
            if (keyState[Keys.Down] == KeyState.Down && _oKeyState[Keys.Down] == KeyState.Up) ShiftDown();
            if (keyState[Keys.Up] == KeyState.Down && _oKeyState[Keys.Up] == KeyState.Up) ShiftUp();

            TextChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ClickCharacter(object sender, MouseEventArgs args)
        {
            if (IsSelected)
            {
                var positionWithinComponent = args.Location - new Size(new Point(X, Y));
                LineIndex = positionWithinComponent.Y/_spriteFont.LineSpacing;
                CharacterIndex = positionWithinComponent.X/_text[LineIndex].Length;
            }
            else
            {
                IsSelected = true;
            }
        }
        

        private void InputCharacter(object sender, TextInputEventArgs args)
        {
            if (!IsSelected) return;
            var keyState = Keyboard.GetState();
            if (keyState[Keys.Enter] == KeyState.Down)
            {
                if (keyState[Keys.LeftShift] == KeyState.Up)
                {
                    EnterKeyPressed?.Invoke(this, null);
                }
                else
                {
                    var firstLine = _text[LineIndex].Substring(0, CharacterIndex);
                    var secondLine = _text[LineIndex].Substring(CharacterIndex);

                    _text[LineIndex++] = firstLine;
                    _text.Insert(LineIndex, secondLine);
                    CharacterIndex = 0;
                }
            }

            else if (keyState[Keys.Back] == KeyState.Down)
            {
                // find the last line and remove the last char in that line.
                // if the line is empty, remove the line.
                if (_text.Count > 1 && _text[LineIndex].Length == 0)
                {
                    _text.RemoveAt(LineIndex);
                    LineIndex--;
                    CharacterIndex = _text[LineIndex].Length - 1;
                }
                else if (_text.Count > 0 && _text[LineIndex].Length > 0)
                {
                    _text[LineIndex] = _text[LineIndex].Remove(CharacterIndex - 1, 1);
                    CharacterIndex--;
                }
            }
            else if (keyState[Keys.Escape] == KeyState.Down)
            {
                IsSelected = false;
            }
            else
            {
                var toInsert = _text[LineIndex].Insert(CharacterIndex, args.Character.ToString());
                var m = _spriteFont.MeasureString(toInsert);
                if (m.X > Width)
                {
                    if (_text[LineIndex].Contains(" "))
                    {
                        var last = _text[LineIndex].Split(' ').Last();
                        _text[LineIndex] = _text[LineIndex].Substring(0, _text[LineIndex].LastIndexOf(' '));
                        _text.Insert(LineIndex + 1, last + args.Character);
                    }
                    else
                    {
                        _text.Insert(LineIndex + 1, args.Character.ToString());
                    }
                    LineIndex++;
                    CharacterIndex = _text[LineIndex].Length;
                }
                else
                {
                    _text[LineIndex] = _text[LineIndex].Insert(CharacterIndex, args.Character.ToString());
                    CharacterIndex++;
                }
            }
        }

        private void ShiftLeft()
        {
            if (CharacterIndex == 0)
            {
                // it's reached the farthest left it can go, so we either stop or going to
                // a lower line index
                if (LineIndex == 0) return;
                LineIndex--;
                CharacterIndex = _text[LineIndex].Length - 1;
            }
            else
            {
                CharacterIndex--;
            }
        }

        private void ShiftRight()
        {
            if (LineIndex == _text.Count - 1)
            {
                // this means there is a space at the end of the line to input a character
                if (CharacterIndex < _text[LineIndex].Length) CharacterIndex++;
            }
            else
            {
                // there's another line past the current index
                if (CharacterIndex == _text[LineIndex].Length - 1)
                {
                    LineIndex++;
                    CharacterIndex = 0;
                }
                else
                {
                    CharacterIndex++;
                }
            }
        }

        private void ShiftUp()
        {
            if (LineIndex == 0) return;
            LineIndex--;
            if (CharacterIndex > _text[LineIndex].Length) CharacterIndex = _text[LineIndex].Length;
        }

        private void ShiftDown()
        {
            if (LineIndex >= _text.Count - 1) return;
            LineIndex++;

            if (CharacterIndex > _text[LineIndex].Length) CharacterIndex = _text[LineIndex].Length;
        }

        public event EventHandler TextChanged;
        public event EventHandler EnterKeyPressed;

        public override void Dispose()
        {
            base.Dispose();
            TextChanged = null;
            EnterKeyPressed = null;
            WeltGame.Instance.Window.TextInput -= InputCharacter;
        }
    }
}