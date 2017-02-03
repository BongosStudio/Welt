using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public byte Id { get { return 0x1F; } }

        public int EntityID;
        public sbyte DeltaX, DeltaY, DeltaZ;

        public void ReadPacket(IWeltStream stream)
        {
            EntityID = stream.ReadInt32();
            DeltaX = stream.ReadInt8();
            DeltaY = stream.ReadInt8();
            DeltaZ = stream.ReadInt8();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(EntityID);
            stream.WriteInt8(DeltaX);
            stream.WriteInt8(DeltaY);
            stream.WriteInt8(DeltaZ);
        }
    }
}