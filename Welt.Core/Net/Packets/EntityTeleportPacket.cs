using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Used to teleport entities to arbitrary locations.
    /// </summary>
    public struct EntityTeleportPacket : IPacket
    {
        public byte Id => 0x22;

        public int EntityID;
        public int X, Y, Z;
        public sbyte Yaw, Pitch;

        public EntityTeleportPacket(int entityID, int x, int y, int z, sbyte yaw, sbyte pitch)
        {
            EntityID = entityID;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
        }

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadSByte();
            Pitch = stream.ReadSByte();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
        }
    }
}

