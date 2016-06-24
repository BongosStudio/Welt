#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;

namespace Welt.Core.Net
{
    [Flags]
    public enum MessageType : byte
    {
        Incoming = 0 << 0,
        Outgoing = 1 << 0,

    }
}