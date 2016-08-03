using System;

namespace Welt.Console
{
    [Flags]
    public enum ThrowType : byte
    {
        Info = 0,
        Warning = 1 << 0,
        Error = 1 << 1,
        Severe = 1 << 2
    }
}