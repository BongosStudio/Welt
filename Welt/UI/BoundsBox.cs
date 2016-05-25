#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Runtime.InteropServices;

namespace Welt.UI
{
    /// <summary>
    ///     Basic bounds model for drawing UI components.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct BoundsBox
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public BoundsBox(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public void Transform(float left, float right, float top, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }

        public static bool operator ==(BoundsBox left, BoundsBox right)
        {
            return
                left.Left == right.Left &&
                left.Right == right.Right &&
                left.Top == right.Top &&
                left.Bottom == right.Bottom;
        }

        public static bool operator !=(BoundsBox left, BoundsBox right)
        {
            return !(left == right);
        }
    }
}