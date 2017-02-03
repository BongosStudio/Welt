using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent from clients to begin a new connection.
    /// </summary>
    public struct HandshakePacket : IPacket
    {
        public byte Id { get { return 0x02; } }

        public HandshakePacket(string username)
        {
            Username = username;
        }

        public string Username;

        public void ReadPacket(IWeltStream stream)
        {
            Username = stream.ReadString();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteString(Username);
        }
    }
}