using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent from clients to begin a new connection.
    /// </summary>
    public struct HandshakePacket : IPacket
    {
        public byte Id => 0x02;

        public HandshakePacket(string username)
        {
            Username = username;
        }

        public string Username;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Username = stream.ReadString();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Username);
        }
    }
}