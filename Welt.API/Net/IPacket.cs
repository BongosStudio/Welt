using Lidgren.Network;
using System;

namespace Welt.API.Net
{
    public interface IPacket
    {
        byte Id { get; }
        void ReadPacket(NetIncomingMessage stream);
        void WritePacket(NetOutgoingMessage stream);
    }
}