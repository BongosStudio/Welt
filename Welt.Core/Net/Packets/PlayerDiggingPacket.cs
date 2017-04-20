using System;
using Welt.API.Net;
using Welt.API;
using Lidgren.Network;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent by clients when they start or stop digging. Also used to throw items.
    /// Also sent with action set to StartDigging when clicking to open a door.
    /// </summary>
    public struct PlayerDiggingPacket : IPacket
    {
        public enum Action
        {
            StartDigging = 0,
            StopDigging = 2,
            DropItem = 4
        }

        public PlayerDiggingPacket(Action playerAction, int x, sbyte y, int z, BlockFaceDirection face)
        {
            PlayerAction = playerAction;
            X = x;
            Y = y;
            Z = z;
            Face = face;
        }

        public byte Id => 0x0E;

        public Action PlayerAction;
        public int X;
        public sbyte Y;
        public int Z;
        public BlockFaceDirection Face;

        public void ReadPacket(NetIncomingMessage stream)
        {
            PlayerAction = (Action)stream.ReadSByte();
            X = stream.ReadInt32();
            Y = stream.ReadSByte();
            Z = stream.ReadInt32();
            Face = (BlockFaceDirection)stream.ReadByte();
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write((sbyte)PlayerAction);
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write((byte)Face);
        }
    }
}