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
            ProcessSpace();
            base.Initialize(game, time);
        }

        public override void Draw(GameTime time)
        {
            var l = 0;
            var cSpace = _showCursor ? "_" : "";
            var text = Text.Aggregate((s, s1) => $"{s}\r\n{s1}") + cSpace;
            Graphics.Clear(Color.Black);
            if (Text.Count > 0)
            {
                Sprite.Begin();
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

            if (keyState.GetPressedKeys().Length == 0) return;

            foreach (var key in keyState.GetPressedKeys())
            {              
                if (keyState[Keys.Back] == KeyState.Down)
                {
                    if (!_hasBeenTouched)
                    {
                        // if it hasn't been edited before, clear
                        Text = new List<string> {""};
                        LineIndex = 0;
                        CharacterIndex = 0;
                    }
                    else
                    {
                        // find the last line and remove the last char in that line.
                        // if the line is empty, remove the line.
                        if (Text.Count > 1 && Text.Last().Length == 0)
                        {
                            Text.RemoveAt(Text.Count - 1);
                            LineIndex--;
                            CharacterIndex = Text.Last().Length;
                        }
                        else if (Text.Count > 0 && Text.Last().Length > 0)
                        {
                            Text[Text.Count - 1] = Text.Last().Substring(0, Text.Last().Length - 1);
                            CharacterIndex--;
                        }
                    }            
                }
                else
                {
                    if (_previousKeyState[key] == KeyState.Down) continue;
                    var c = KeySystem.ConvertToString(key, keyState);
                    Text[Text.Count - 1] += c;
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
            if (Text.Count == 0)
            {
                LineIndex = 0;
                CharacterIndex = 0;
                return;
            }
            
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
                        break;
                    }
                }
                else
                {
                    var beg = Text.Last().Split(' ')[i];
                    Text[Text.Count - 1] = Text.Last().Split(' ').Take(i).Aggregate((s, s1) => $"{s} {s1}");
                    Text.Add(beg);
                }
                LineIndex++;
                CharacterIndex = 1;
            }
            else
            {
                CharacterIndex++;
            }
        }

        public event EventHandler TextChanged;
    }
}