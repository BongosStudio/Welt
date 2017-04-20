using System;
using Welt.API.Net;
using Welt.API;
using Lidgren.Network;

namespace Welt.Core.Net.Packets
{
    /// <summary>
    /// Sent when the player interacts with a block (generally via right clicking).
    /// This is also used for items that don't interact with blocks (i.e. food) with the coordinates set to -1.
    /// </summary>
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public byte Id => 0x0F;

        public PlayerBlockPlacementPacket(int x, sbyte y, int z, BlockFaceDirection face, ushort itemID,
            sbyte? amount, byte? metadata)
        {
            X = x;
            Y = y;
            Z = z;
            Face = face;
            ItemID = itemID;
            Amount = amount;
            Metadata = metadata;
        }

        public int X;
        public sbyte Y;
        public int Z;
        public BlockFaceDirection Face;
        /// <summary>
        /// The block or item ID. You should probably ignore this and use a server-side inventory.
        /// </summary>
        public ushort ItemID;
        /// <summary>
        /// The amount in the player's hand. Who cares?
        /// </summary>
        public sbyte? Amount;
        /// <summary>
        /// The block metadata. You should probably ignore this and use a server-side inventory.
        /// </summary>
        public byte? Metadata;

        public void ReadPacket(NetIncomingMessage stream)
        {
            X = stream.ReadInt32();
            Y = stream.ReadSByte();
            Z = stream.ReadInt32();
            Face = (BlockFaceDirection)stream.ReadSByte();
            ItemID = stream.ReadUInt16();
            if (ItemID != 0)
            {
                Amount = stream.ReadSByte();
                Metadata = stream.ReadByte();
            }
        }

        public void WritePacket(NetOutgoingMessage stream)
        {
            stream.Write(X);
            stream.Write(Y);
            stream.Write(Z);
            stream.Write((sbyte)Face);
            stream.Write(ItemID);
            if (ItemID != 0)
            {
                stream.Write(Amount.Value);
                stream.Write(Metadata.Value);
            }
        }
    }
}