#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.IO;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Welt.UI
{
    public class TextInputComponent : UIComponent
    {
        private bool _hasBeenTouched;
        private TimeSpan _cursor;
        private bool _showCursor;
        private KeyboardState _previousKeyState;
        private SpriteFont _spriteFont;
        private TimeSpan _inputDelay;

        public override Cursor Cursor => Cursors.IBeam;
        public bool IsSelected;
        public List<string> Text { get; set; }
        public Color Foreground { get; set; }
        public int LineIndex { get; set; }
        public int CharacterIndex { get; set; }

        public override int Opacity
        {
            get { return Foreground.A; }
            set { Foreground = Color.FromNonPremultiplied(Foreground.R, Foreground.G, Foreground.B, value); }
        }


        public TextInputComponent(string text, string name, int width, int height, GraphicsDevice device) : this(text, name, width, height, null, device)
        {
        }

        public TextInputComponent(string text, string name, int width, int height, UIComponent parent, GraphicsDevice device) : base(name, width, height, parent, device)
        {
            Text = new List<string> {text};
            _hasBeenTouched = false;
            IsSelected = true; // TODO: this is for testing only
            _showCursor = true;
        }

        public override void Initialize(Game game, GameTime time)
        {
            _spriteFont = game.Content.Load<SpriteFont>("Fonts/console");
            LineIndex = Text.Count - 1;
            CharacterIndex = Text.Last().Length - 1;
            ProcessSpace();
            base.Initialize(game, time);
        }

        public override void Draw(GameTime time)
        {
            var l = 0;
            var cSpace = _showCursor ? "_" : "";
            var text = Text.Aggregate((s, s1) => $"{s}\r\n{s1}").Insert(CharacterIndex, cSpace);
            Graphics.Clear(Color.Black);
            if (Text.Count > 0)
            {
                Sprite.Begin();
                Sprite.Draw(new Texture2D(Graphics, Width, Height), new Vector2(X, Y), Color.Aqua);
                Sprite.DrawString(_spriteFont, text, new Vector2(X, Y), Foreground);
                Sprite.End();
            }
            base.Draw(time);
        }

        public override void Update(GameTime time)
        {
            if (_cursor.TotalMilliseconds > 0)
            {
                _cursor -= time.ElapsedGameTime;
            }
            else
            {
                _cursor = TimeSpan.FromSeconds(1);
                _showCursor = !_showCursor;
            }

            var keyState = Keyboard.GetState();
            KeyAdvanceMap.Update(keyState);

            if (keyState.GetPressedKeys().Length == 0) return;

            foreach (var key in keyState.GetPressedKeys())
            {
                if (keyState[Keys.Enter] == KeyState.Down && keyState[Keys.LeftShift] == KeyState.Up)
                {
                    EnterKeyPressed?.Invoke(this, null);
                    return;
                }

                if (keyState[Keys.Back] == KeyState.Down)
                {
                    if (!KeyAdvanceMap.Process(key)) return;

                    // find the last line and remove the last char in that line.
                    // if the line is empty, remove the line.
                    if (Text.Count > 1 && Text[LineIndex].Length == 0)
                    {
                        Text.RemoveAt(LineIndex);
                        LineIndex--;
                        CharacterIndex = Text[LineIndex].Length - 1;
                    }
                    else if (Text.Count > 0 && Text.Last().Length > 0)
                    {
                        Text[LineIndex] = Text[LineIndex].Substring(0, CharacterIndex - 1);
                        CharacterIndex--;
                    }
                }
                else
                {
                    if (!KeyAdvanceMap.Process(key)) return;
                    Text[Text.Count - 1] += KeySystem.ConvertToString(key, keyState);                 
                }
            }

            _previousKeyState = keyState;
            _hasBeenTouched = true;
            ProcessSpace(); // adjust text box width and height
            TextChanged?.Invoke(this, EventArgs.Empty);
            base.Update(time);
        }

        private void ProcessSpace()
        {
            if (LineIndex > Text.Count - 1) LineIndex = Text.Count - 1;
            if (CharacterIndex > Text[LineIndex].Length - 1) CharacterIndex = Text[LineIndex].Length - 1;

            var v = _spriteFont.MeasureString(Text.Last());
            if (v.X > Width)
            {
                Height += _spriteFont.LineSpacing;
                var i = Text.Last().Split(' ').Length - 1;
                if (i == 0)
                {
                    // this means the line is a single word and building
                    for (var ci = Text.Last().Length; ci > 0; ci--)
                    {
                        if (_spriteFont.MeasureString(Text.Last().Substring(0, ci)).X > Width) continue;
                        var firstLine = Text.Last().Substring(0, ci);
                        var secondLine = Text.Last().Substring(ci);
                        Text[Text.Count - 1] = firstLine;
                        Text.Add(secondLine);
                        LineIndex++;
                        CharacterIndex = secondLine.Length - 1;
                        return;
                    }
                }
                else
                {
                    var beg = Text.Last().Split(' ')[i];
                    Text[Text.Count - 1] = Text.Last().Split(' ').Take(i).Aggregate((s, s1) => $"{s} {s1}");
                    Text.Add(beg);
                    CharacterIndex += beg.Length - 1;
                    LineIndex++;
                }
            }
            else
            {
                CharacterIndex++;
                // TODO:
            }
            
        }

        public event EventHandler TextChanged;
        public event EventHandler EnterKeyPressed;

    }
}