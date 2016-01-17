#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework.Input;

namespace Welt.IO
{
    public class KeySystem
    {
        public static string ConvertToString(Keys key, KeyboardState state)
        {
            var c = "";

            switch (key)
            {
                case Keys.Back:
                    c = "\b";
                    break;
                case Keys.Enter:
                    c = Environment.NewLine;
                    break;
                case Keys.Space:
                    c = " ";
                    break;
                case Keys.OemPlus:
                    c = "=";
                    break;
                case Keys.OemMinus:
                    c = "-";
                    break;
                case Keys.OemPeriod:
                    c = ".";
                    break;
                case Keys.OemQuestion:
                    c = "?";
                    break;
                case Keys.OemBackslash:
                    c = "\\";
                    break;
                case Keys.OemCloseBrackets:
                    c = "]";
                    break;
                case Keys.OemOpenBrackets:
                    c = "[";
                    break;
                case Keys.OemComma:
                    c = ",";
                    break;
                case Keys.OemSemicolon:
                    c = ";";
                    break;
                case Keys.OemQuotes:
                    c = "'";
                    break;
                default:
                    
                    if (key >= Keys.D0 && key <= Keys.D9) c = ((int) key - 48).ToString();
                    else switch (key)
                        {
                            case Keys.Multiply:
                                c = "*";
                                break;
                            case Keys.Add:
                                c = "=";
                                break;
                            case Keys.Subtract:
                                c = "-";
                                break;
                            case Keys.Decimal:
                                c = ".";
                                break;
                            case Keys.Divide:
                                c = "/";
                                break;
                            default:
                                if ((int) key > 64 && (int) key < 91)
                                {
                                    c = state[Keys.CapsLock] == KeyState.Down ? key.ToString() : key.ToString().ToLower();                                  
                                }
                                break;
                        }
                    break;
            }
            return state[Keys.LeftShift] == KeyState.Down
                ? GetShiftValue(key)
                : c;
        }

        public static string GetShiftValue(Keys key)
        {
            if ((int) key > 64 && (int) key < 91) return key.ToString().ToUpper();
            switch (key)
            {
                case Keys.D1:
                    return "!";
                case Keys.D2:
                    return "@";
                case Keys.D3:
                    return "#";
                case Keys.D4:
                    return "$";
                case Keys.D5:
                    return "%";
                case Keys.D6:
                    return "^";
                case Keys.D7:
                    return "&";
                case Keys.D8:
                    return "*";
                case Keys.D9:
                    return "(";
                case Keys.D0:
                    return ")";
                case Keys.OemMinus:
                    return "_";
                case Keys.OemPlus:
                    return "+";
                case Keys.OemSemicolon:
                    return ":";
                case Keys.OemTilde:
                    return "~";
                case Keys.OemQuotes:
                    return "\"";
                case Keys.OemComma:
                    return "<";
                case Keys.OemPeriod:
                    return ">";
                case Keys.OemQuestion:
                    return "?";
                case Keys.OemBackslash:
                    return "|";
                case Keys.OemOpenBrackets:
                    return "{";
                case Keys.OemCloseBrackets:
                    return "}";
                default:

                    return string.Empty;
            }
        }
    }
}