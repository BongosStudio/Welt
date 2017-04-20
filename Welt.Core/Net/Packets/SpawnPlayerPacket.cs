using Lidgren.Network;
using System;
using Welt.API.Net;

namespace Welt.Core.Net.Packets
{
    public struct SpawnPlayerPacket : IPacket
    {
        public byte Id => 0x14;

        public int EntityID;
        public string PlayerName;
        public int X, Y, Z;
        public sbyte Yaw, Pitch;
        /// <summary>
        /// Note that this should be 0 for "no item".
        /// </summary>
        public short CurrentItem;

        public SpawnPlayerPacket(int entityID, string playerName, int x, int y, int z, sbyte yaw, sbyte pitch, short currentItem)
        {
            EntityID = entityID;
            PlayerName = playerName;
            X = x;
            Y = y;
            Z = z;
            Yaw = yaw;
            Pitch = pitch;
            CurrentItem = currentItem;
        }

        public void ReadPacket(NetIncomingMessage stream)
        {
            EntityID = stream.ReadInt32();
            PlayerName = stream.ReadString();
            X = stream.ReadInt32();
            Y = stream.ReadInt32();
            Z = stream.ReadInt32();
            Yaw = stream.ReadSByte();
            Pitch = stream.ReadSByte();
            CurrentItem = stream.ReadInt16();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(EntityID);
            stream.Write(PlayerName);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write(Yaw);
            stream.Write(Pitch);
            stream.Write(CurrentItem);
        }
    }
}