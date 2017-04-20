using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct EntityLookAndRelativeMovePacket : IPacket
    {
        public byte Id => 0x21;

        public int EntityID;
        public sbyte DeltaX, DeltaY, DeltaZ;
        public sbyte Yaw, Pitch;

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            DeltaX = stream.ReadSByte();
            DeltaY = stream.ReadSByte();
            DeltaZ = stream.ReadSByte();
            Yaw = stream.ReadSByte();
            Pitch = stream.ReadSByte();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(DeltaX);
            stream.Write(DeltaY);
            stream.Write(DeltaZ);
            stream.Write(Yaw);
            stream.Write(Pitch);
        }
    }
}