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

        public void ReadPacket(IWeltStream stream)
        {
            EntityID = stream.ReadInt32();
            Yaw = stream.ReadInt8();
            Pitch = stream.ReadInt8();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt32(EntityID);
            stream.WriteInt8(Yaw);
            stream.WriteInt8(Pitch);
        }
    }
}