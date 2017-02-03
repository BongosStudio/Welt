using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients after the handshake to request logging into the world.
    /// </summary>
    public struct LoginRequestPacket : IPacket
    {
        public byte Id { get { return 0x01; } }

        public LoginRequestPacket(int protocolVersion, string username)
        {
            ProtocolVersion = protocolVersion;
            Username = username;
        }

        public int ProtocolVersion;
        public string Username;

        public void ReadPacket(IWeltStream stream)
        {
            ProtocolVersion = stream.ReadInt32();
            Username = stream.ReadString();
            stream.ReadInt64(); // Unused
            stream.ReadInt8();  // Unused
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(ProtocolVersion);
            stream.WriteString(Username);
            stream.WriteInt64(0); // Unused
            stream.WriteInt8(0);  // Unused
        }
    }
}