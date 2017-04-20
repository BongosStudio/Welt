using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Disconnects from a server or kicks a player. This is the last packet sent.
    /// </summary>
    public struct DisconnectPacket : IPacket
    {
        public byte Id => 0xFF;

        public DisconnectPacket(string reason)
        {
            Reason = reason;
        }

        public string Reason;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Reason = stream.ReadString();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Reason);
        }
    }
}