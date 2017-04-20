using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Used by clients to send messages and by servers to propegate messages to clients.
    /// Note that the server is expected to include the username, i.e. <User> message, but the
    /// client is not given the same expectation.
    /// </summary>
    public struct ChatMessagePacket : IPacket
    {
        public byte Id => 0x03;

        public ChatMessagePacket(string message)
        {
            Message = message;
        }

        public string Message;

        public void ReadPacket(NetIncomingMessage stream)
        {
            Message = stream.ReadString();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(Message);
        }
    }
}