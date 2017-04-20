using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by servers to update the direction an entity is looking in.
    /// </summary>
    public struct EntityLookPacket : IPacket
    {
        public byte Id => 0x20;

        public int EntityID;
        public sbyte Yaw, Pitch;

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            Yaw = stream.ReadSByte();
            Pitch = stream.ReadSByte();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(Yaw);
            stream.Write(Pitch);
        }
    }
}