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
        IEnumerable<IPacket> ReadPackets(object key, byte[] buffer, int offset, int length, bool serverbound = true);
        void WritePacket(IWeltStream stream, IPacket packet);
    }
}