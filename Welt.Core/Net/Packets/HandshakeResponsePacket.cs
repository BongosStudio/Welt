using System;
using Welt.API;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent from servers to continue with a connection. A kick is sent instead if the connection is refused.
    /// </summary>
    public struct HandshakeResponsePacket : IPacket
    {
        public byte Id => 0x02;

        public HandshakeResponsePacket(string connectionHash, string name, string motd, int online, int max)
        {
            ConnectionHash = connectionHash;
            ServerName = name;
            ServerMotd = motd;
            OnlineUsers = online;
            MaxUsers = max;
        }

        public HandshakeResponsePacket(string connectionHash, int online, IServerConfiguration config) 
            : this(connectionHash, config.Name, config.MessageOfTheDay, online, config.MaxPlayers)
        {

        }

        /// <summary>
        /// Set to "-" for offline mode servers.
        /// </summary>
        public string ConnectionHash;
        public string ServerName;
        public string ServerMotd;
        public int OnlineUsers;
        public int MaxUsers;

        public void ReadPacket(IWeltStream stream)
        {
            ConnectionHash = stream.ReadString();
            ServerName = stream.ReadString();
            ServerMotd = stream.ReadString();
            OnlineUsers = stream.ReadInt32();
            MaxUsers = stream.ReadInt32();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteString(ConnectionHash);
            stream.WriteString(ServerName);
            stream.WriteString(ServerMotd);
            stream.WriteInt32(OnlineUsers);
            stream.WriteInt32(MaxUsers);
        }
    }
}