#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Lidgren.Network;

namespace Welt.Core.Net.Packets
{
    public class HandshakePacket : Packet
    {
        public string Username;

        public override byte Id => 0x00;

        public override void ReadData(ref NetIncomingMessage message)
        {
            if (!message.ReadString(out Username)) throw new NetException();
        }

        public override void WriteData(ref NetOutgoingMessage message)
        {
            message.Write(Username);
        }
    }
}