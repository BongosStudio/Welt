using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Welt.API;
using Welt.API.Net;

namespace Welt.Core.Net
{
    public class PacketSegmentProcessor : IPacketSegmentProcessor
    {

        public List<byte> PacketBuffer { get; private set; }

        public PacketReader PacketReader { get; protected set; }

        public bool ServerBound { get; private set; }
        
        public IPacket CurrentPacket { get; protected set; }

        public PacketSegmentProcessor(PacketReader packetReader, bool serverBound)
        {
            PacketBuffer = new List<byte>();
            PacketReader = packetReader;
            ServerBound = serverBound;
        }

        public bool ProcessNextSegment(byte[] nextSegment, int offset, int len, out IPacket packet)
        {
            packet = null;
            CurrentPacket = null;

            if (nextSegment.Length > 0)
            {
                PacketBuffer.AddRange(new ByteArraySegment(nextSegment, offset, len));
            }

            if (PacketBuffer.Count == 0)
                return false;
            
            if (CurrentPacket == null)
            {
                byte packetId = PacketBuffer[0];

                Func<IPacket> createPacket;
                if (ServerBound)
                    createPacket = PacketReader.m_ServerboundPackets[packetId];
                else
                    createPacket = PacketReader.m_ClientboundPackets[packetId];

                if (createPacket == null)
                    throw new NotSupportedException("Unable to read packet type 0x" + packetId.ToString("X2"));

                CurrentPacket = createPacket();
            }
            
            using (ByteListMemoryStream listStream = new ByteListMemoryStream(PacketBuffer, 1))
            {
                using (WeltStream ms = new WeltStream(listStream))
                {
                    try
                    {
                        CurrentPacket.ReadPacket(ms);
                    }
                    catch (EndOfStreamException)
                    {
                        return false;
                    }
                }
                
                PacketBuffer.RemoveRange(0, (int)listStream.Position);
            }
            
            packet = CurrentPacket;
            CurrentPacket = null;

            return PacketBuffer.Count > 0;
        }

    }
}
