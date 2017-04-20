using Lidgren.Network;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Welt.API.Net
{
    public interface IPacketReader
    {
        int ProtocolVersion { get; }

        ConcurrentDictionary<object, IPacketSegmentProcessor> Processors { get; }
        void RegisterPacketType<T>(bool clientbound = true, bool serverbound = true) where T : IPacket;
        IPacket ReadPacket(NetIncomingMessage message, bool serverbound = true);
        void WritePacket(NetOutgoingMessage message, IPacket packet);
    }
}