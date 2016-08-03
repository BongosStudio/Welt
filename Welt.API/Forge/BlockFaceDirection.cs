#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;

namespace Welt.API.Forge
{
    [Flags]
    public enum BlockFaceDirection : byte
    {
        None,
        XIncreasing = 1 << 0,
        XDecreasing = 1 << 1,
        YIncreasing = 1 << 2,
        YDecreasing = 1 << 3,
        ZIncreasing = 1 << 4,
        ZDecreasing = 1 << 5
    }
}