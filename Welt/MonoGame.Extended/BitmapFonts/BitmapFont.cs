using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Welt.MonoGame.Extended.BitmapFonts
{
    public class BitmapFont
    {
        internal BitmapFont(IEnumerable<BitmapFontRegion> regions, int lineHeight)
        {
            _characterMap = regions.ToDictionary(r => r.Character);
            LineHeight = lineHeight;
        }

        private readonly Dictionary<int, BitmapFontRegion> _characterMap;

        public int LineHeight { get; private set; }

        public BitmapFontRegion GetCharacterRegion(int character)
        {
            BitmapFontRegion region;
            return _characterMap.TryGetValue(character, out region) ? region : null;
        }

        internal static IEnumerable<int> GetUnicodeCodePoints(string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                for (int i = 0; i < s.Length; i += 1)
                {
                    if (char.IsLowSurrogate(s, i))
                        continue;

                    yield return char.ConvertToUtf32(s, i);
                }
            }
        }

        public Size GetSize(string text)
        {
            var width = 0;
            var height = 0;

            foreach (int c in GetUnicodeCodePoints(text))
            {
                BitmapFontRegion fontRegion;

                if (_characterMap.TryGetValue(c, out fontRegion))
                {
                    width += fontRegion.XAdvance;

                    if (fontRegion.Height + fontRegion.YOffset > height)
                        height = fontRegion.Height + fontRegion.YOffset;
                }
            }

            return new Size(width, height);
        }

        public Vector2 MeasureString(string text)
        {
            if (text == null) throw new ArgumentNullException(nameof(text));

            var size = GetSize(text);
            return new Vector2(size.Width, size.Height);
        }

        public Vector2 MeasureString(StringBuilder stringBuilder)
        {
            if (stringBuilder == null) throw new ArgumentNullException(nameof(stringBuilder));

            return MeasureString(stringBuilder.ToString());
        }

        public Rectangle GetStringRectangle(string text, Vector2 position)
        {
            var size = GetSize(text);
            var p = position.ToPoint();
            return new Rectangle(p.X, p.Y, size.Width, size.Height);
        }
    }
}
