using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients after the handshake to request logging into the world.
    /// </summary>
    public struct LoginRequestPacket : IPacket
    {
        public byte Id => 0x01;

        public LoginRequestPacket(int protocolVersion, string username)
        {
            ProtocolVersion = protocolVersion;
            Username = username;
        }

        public int ProtocolVersion;
        public string Username;

        public void ReadPacket(NetIncomingMessage stream)
        {
            ProtocolVersion = stream.ReadInt32();
            Username = stream.ReadString();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(ProtocolVersion);
            stream.Write(Username);
        }
    }
}