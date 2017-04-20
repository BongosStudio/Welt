using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public byte Id => 0x1F;

        public int EntityID;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            DeltaX = stream.ReadSByte();
            DeltaY = stream.ReadSByte();
            DeltaZ = stream.ReadSByte();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(DeltaX);
            stream.Write(DeltaY);
            stream.Write(DeltaZ);
        }
    }
}