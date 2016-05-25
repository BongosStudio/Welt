using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Models;
<<<<<<< HEAD
using Welt.Scenes;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

namespace Welt.UI
{
    public static class Effects
    {
<<<<<<< HEAD
        public static Texture2D CreateSolidColorTexture(GraphicsDevice graphics, int width, int height, Color color,
            BoundsBox border = new BoundsBox(), Color borderColor = default(Color))
=======
        public static Texture2D CreateSolidColorTexture(GraphicsDevice graphics, int width, int height, Color color)
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
        {
            var texture = new Texture2D(graphics, width, height);
            var colors = new Color[width*height];
            for (var i = 0; i < colors.Length; i++)
            {
<<<<<<< HEAD
                var isBorder =
                    i%width < border.Left ||
                    i%width >= width - border.Right ||
                    i <= width*border.Top ||
                    i >= colors.Length - width*border.Bottom;
                if (isBorder) colors[i] = borderColor;
                else colors[i] = color;
=======
                colors[i] = color;
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            }
            texture.SetData(colors);
            return texture;
        }

        public static KeyValuePair<Color, string>[] ProcessText(string input, Color defaultColor)
        {
<<<<<<< HEAD
            if (input == null) return new KeyValuePair<Color, string>[0];
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            var data = new List<KeyValuePair<Color, string>>();
            var builder = new StringBuilder();
            var color = defaultColor;
            for (var i = 0; i < input.Length; i++)
            {
                if (input[i] == '{')
                {
                    var si = i;
                    var isColor = false;
                    while (si < input.Length)
                    {
                        if (input[si] == ' ') break;
                        if (input[si] == '}')
                        {
                            isColor = true;
                            break;
                        }
                        si++;
                    }
                    if (!isColor)
                    {
                        builder.Append(input[i]);
                        continue;
                    }
                    var colorData = input.Substring(i, si - i);
                    var requestedColor = Maybe<string, InvalidOperationException>.Check(() => colorData.Split('=')[1]);
                    if (requestedColor.HasError)
                    {
                        builder.Append(input[i]);
                        continue;
                    }

                    data.Add(new KeyValuePair<Color, string>(color, builder.ToString()));
                    builder.Clear();

                    var finalValue = requestedColor.Value.Replace("}", "").Trim();
                    Console.WriteLine(finalValue);
                    if (finalValue.StartsWith("#"))
                    {
                        color = GetColorFromHex(finalValue);
                    }
                    else if (finalValue == "default")
                    {
                        color = defaultColor;
                    }
                    else if (finalValue.StartsWith("{{"))
                    {
                        color = GetColorFromString(finalValue);
                    }
                    else
                    {
                        color = GetColorFromName(finalValue);
                    }
                    
                    i += colorData.Length;
                }
                else
                {
                    builder.Append(input[i]);
                }
            }

            data.Add(new KeyValuePair<Color, string>(color, builder.ToString()));
            return data.ToArray();
        }

        public static Color GetColorFromHex(string hex)
        {
            return new Color(byte.Parse(hex.Substring(1, 2))/255,
                byte.Parse(hex.Substring(3, 2))/255, byte.Parse(hex.Substring(5, 2))/255);
        }

        public static Color GetColorFromSystemColor(System.Drawing.Color color)
        {
            return new Color(color.R/255, color.G/255, color.B/255, color.A/255);
        }

        public static Color GetColorFromName(string name)
        {
            if (name.Contains(".")) name = name.Split('.').Last();
            foreach (var colorObject in typeof (Color).GetProperties().Where(colorObject => colorObject.Name.ToLower() == name.ToLower()))
            {
                return (Color) colorObject.GetValue(colorObject, null);
            }
            return Color.Black;
        }

        public static Color GetColorFromString(string input)
        {
            byte r = 0x00;
            byte b = 0x00;
            byte g = 0x00;
            byte a = 0x00;

            for (var i = 0; i < input.Length; i++)
            {
<<<<<<< HEAD
                switch (char.ToUpper(input[i]))
=======
                switch (input[i])
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
                {
                    case 'R':
                        r = byte.Parse(input.Substring(i + 2, 2));
                        break;
                    case 'B':
                        b = byte.Parse(input.Substring(i + 2, 2));
                        break;
                    case 'G':
                        g = byte.Parse(input.Substring(i + 2, 2));
                        break;
                    case 'A':
                        a = byte.Parse(input.Substring(i + 2, 2));
                        break;
                    default:
                        continue;
                }
            }
            return Color.FromNonPremultiplied(r, g, b, a);
        }
<<<<<<< HEAD

        public static void FadeIn(this UIComponent component, Scene scene, TimeSpan length)
        {
            component.Opacity = 0;
            var stepSpan = length.TotalMilliseconds/50;
            for (var i = 0; i < 50; i++)
            {
                scene.Schedule(() => component.Opacity += 0.02f, TimeSpan.FromMilliseconds(stepSpan*i));
            }
        }

        public static void PopulateWith(this UIComponent c, UIComponent child, int count, Action selectfunc = null)
        {
            for (var i = 0; i < count; i++)
            {
                c.AddComponent(UIComponent.Clone(child, $"{child.Name}{i}"));
                selectfunc?.Invoke();
            }
        }

        public static void FadeSceneIn(this Scene scene, TimeSpan length)
        {
            // how to do this:
            // 1: draw a black rectangle over the screen OR clear screen of this specific color
            // 2: watch for .Initialize to be finished then start fading the rectangle. 
            // simple, yeah?
            
        }
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
    }
}