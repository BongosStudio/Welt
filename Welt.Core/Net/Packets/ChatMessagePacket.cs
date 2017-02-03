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
        public byte Id { get { return 0x03; } }

        public ChatMessagePacket(string message)
        {
            Message = message;
        }

        public string Message;

        public void ReadPacket(IWeltStream stream)
        {
            Message = stream.ReadString();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteString(Message);
        }
    }
}