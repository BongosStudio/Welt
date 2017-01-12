#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Lidgren.Network;

namespace Welt.Core.Net
{
    public abstract class Packet
    {
        public abstract byte Id { get; }

        public abstract void ReadData(ref NetIncomingMessage message);
        public abstract void WriteData(ref NetOutgoingMessage message);
    }
}