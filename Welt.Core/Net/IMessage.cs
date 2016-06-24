#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Lidgren.Network;

namespace Welt.Core.Net
{
    public interface IMessage
    {
        byte Id { get; }
    }
}