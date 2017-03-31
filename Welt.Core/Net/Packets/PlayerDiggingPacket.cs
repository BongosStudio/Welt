using System;
using Welt.API.Net;
using Welt.API;

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

        public void ReadPacket(IWeltStream stream)
        {
            PlayerAction = (Action)stream.ReadInt8();
            X = stream.ReadInt32();
            Y = stream.ReadInt8();
            Z = stream.ReadInt32();
            Face = (BlockFaceDirection)stream.ReadUInt8();
        }

        public void WritePacket(IWeltStream stream)
        {
            stream.WriteInt8((sbyte)PlayerAction);
            stream.WriteInt32(X);
            stream.WriteInt8(Y);
            stream.WriteInt32(Z);
            stream.WriteUInt8((byte)Face);
        }
    }
}