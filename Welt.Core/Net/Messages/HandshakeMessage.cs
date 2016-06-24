#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Lidgren.Network;

namespace Welt.Core.Net.Messages
{
    public class HandshakeMessage : IMessage
    {
        public byte Id => 0x0;

    }
}