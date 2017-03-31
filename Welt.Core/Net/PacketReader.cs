using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Net.Packets;

namespace Welt.Core.Net
{
    public class PacketReader : IPacketReader
    {
        public static readonly int Version = 1;
        public int ProtocolVersion => Version;

        internal Func<IPacket>[] m_ClientboundPackets = new Func<IPacket>[0x100];
        internal Func<IPacket>[] m_ServerboundPackets = new Func<IPacket>[0x100];

        public ConcurrentDictionary<object, IPacketSegmentProcessor> Processors { get; private set; }

        private static readonly byte[] m_EmptyBuffer = new byte[0];

        public PacketReader()
        {
            Processors = new ConcurrentDictionary<object, IPacketSegmentProcessor>();
        }

        /// <summary>
        /// Registers Welt.Core vanilla packets.
        /// </summary>
        public void RegisterCorePackets()
        {
            RegisterPacketType<HandshakePacket>(clientbound: false, serverbound: true); // 0x02
            RegisterPacketType<HandshakeResponsePacket>(clientbound: true, serverbound: false); // 0x02
            RegisterPacketType<KeepAlivePacket>(clientbound: true, serverbound: true);
            RegisterPacketType<LoginRequestPacket>(clientbound: false, serverbound: true);
            RegisterPacketType<LoginResponsePacket>(clientbound: true, serverbound: false);
            RegisterPacketType<SpawnPositionPacket>(clientbound: true, serverbound: false);
            RegisterPacketType<SpawnPlayerPacket>(clientbound: true, serverbound: false);
            RegisterPacketType<PlayerGroundedPacket>(clientbound: false, serverbound: true);
            RegisterPacketType<PlayerPositionAndLookPacket>(clientbound: false, serverbound: true);
            RegisterPacketType<PlayerPositionPacket>(clientbound: true, serverbound: false);
            RegisterPacketType<PlayerLookPacket>(clientbound: true, serverbound: false);
            RegisterPacketType<SetPlayerPositionPacket>(clientbound: true, serverbound: true);
            RegisterPacketType<TimeUpdatePacket>(clientbound: true, serverbound: false);
            RegisterPacketType<ChatMessagePacket>(clientbound: true, serverbound: true);
            RegisterPacketType<ChunkPreamblePacket>(clientbound: true, serverbound: false);
            RegisterPacketType<ChunkDataPacket>(clientbound: true, serverbound: false);
            RegisterPacketType<DisconnectPacket>(clientbound: true, serverbound: true);
        }

        public void RegisterPacketType<T>(bool clientbound = true, bool serverbound = true) where T : IPacket
        {
            var func = Expression.Lambda<Func<IPacket>>(Expression.Convert(Expression.New(typeof(T)), typeof(IPacket))).Compile();
            var packet = func();

            if (clientbound)
                m_ClientboundPackets[packet.Id] = func;
            if (serverbound)
                m_ServerboundPackets[packet.Id] = func;
        }

        public IEnumerable<IPacket> ReadPackets(object key, byte[] buffer, int offset, int length, bool serverbound = true)
        {
            if (!Processors.ContainsKey(key))
                Processors[key] = new PacketSegmentProcessor(this, serverbound);

            IPacketSegmentProcessor processor = Processors[key];
            processor.ProcessNextSegment(buffer, offset, length, out IPacket packet);

            if (packet == null)
                yield break;

            while (true)
            {
                yield return packet;

                if (!processor.ProcessNextSegment(m_EmptyBuffer, 0, 0, out packet))
                {
                    if (packet != null)
                    {
                        yield return packet;
                    }

                    yield break;
                }
            }
        }

        public void WritePacket(IWeltStream stream, IPacket packet)
        {
            stream.WriteUInt8(packet.Id);
            packet.WritePacket(stream);
            stream.BaseStream.Flush();
        }
    }
}