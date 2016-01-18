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
            IsSelected = true; // TODO: this is for testing only
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
            var text = Text.Aggregate((s, s1) => $"{s}\r\n{s1}");
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

                switch (key)
                {
                    case Keys.Left:
                        if (CharacterIndex == 0 && LineIndex > 0)
                        {
                            LineIndex--;
                            CharacterIndex = Text[LineIndex].Length - 1;
                        }
                        else
                        {
                            CharacterIndex--;
                        }
                        break;
                    case Keys.Right:
                        if (CharacterIndex == Text[LineIndex].Length && LineIndex < Text.Count - 1)
                        {
                            LineIndex++;
                            CharacterIndex = 0;
                        }
                        else
                        {
                            CharacterIndex++;
                        }
                        break;
                    case Keys.Up:
                        if (LineIndex > 0)
                        {
                            LineIndex--;
                        }
                        break;
                    case Keys.Down:
                        if (LineIndex < Text.Count - 1)
                        {
                            LineIndex++;
                        }
                        break;

                }

                if (keyState[Keys.Back] == KeyState.Down)
                {
                    if (!KeyAdvanceMap.Process(key) && keyState[Keys.LeftShift] != KeyState.Down) return;

                    // find the last line and remove the last char in that line.
                    // if the line is empty, remove the line.
                    if (Text.Count > 1 && Text[LineIndex].Length == 0)
                    {
                        Text.RemoveAt(LineIndex);
                        LineIndex--;
                        CharacterIndex = Text[LineIndex].Length - 1;
                    }
                    else if (Text.Count > 0 && Text[LineIndex].Length > 0)
                    {
                        Text[LineIndex] = Text[LineIndex].Substring(0, CharacterIndex - 1);
                        // BUG: this throws an error if multiple lines are deleted. 
                        // cont of BUG: make sure that the line and character index are correct
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
            ProcessSpace(); // adjust text box width and height
            TextChanged?.Invoke(this, EventArgs.Empty);
            base.Update(time);
        }

        private void ProcessSpace()
        {
            if (LineIndex > Text.Count - 1) LineIndex = Text.Count - 1;
            if (CharacterIndex > Text[LineIndex].Length - 1) CharacterIndex = Text[LineIndex].Length - 1;

            var v = _spriteFont.MeasureString(Text[LineIndex]);
            if (v.X > Width)
            {
                Height += _spriteFont.LineSpacing;
                var i = Text.Last().Split(' ').Length - 1;
                if (i == 0)
                {
                    // this means the line is a single word and building
                    for (var ci = Text[LineIndex].Length; ci > 0; ci--)
                    {
                        if (_spriteFont.MeasureString(Text[LineIndex].Substring(0, ci)).X > Width) continue;
                        var firstLine = Text[LineIndex].Substring(0, ci);
                        var secondLine = Text[LineIndex].Substring(ci);
                        Text[LineIndex] = firstLine;
                        Text.Insert(LineIndex + 1, secondLine);
                        LineIndex++;
                        CharacterIndex = secondLine.Length - 1;
                        return;
                    }
                }
                else
                {
                    var beg = Text[LineIndex].Split(' ')[i];
                    Text[LineIndex] = Text[LineIndex].Split(' ').Take(i).Aggregate((s, s1) => $"{s} {s1}");
                    Text.Insert(LineIndex + 1, beg);
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