using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Net;
using Welt.Core.Net.Packets;

namespace Welt.Handlers
{
    internal static class PacketHandlers
    {
        public static void RegisterHandlers(MultiplayerClient client)
        {
            client.RegisterPacketHandler(new HandshakeResponsePacket().Id, HandleHandshake);
            client.RegisterPacketHandler(new ChatMessagePacket().Id, HandleChatMessage);
            client.RegisterPacketHandler(new SetPlayerPositionPacket().Id, HandlePositionAndLook);
            client.RegisterPacketHandler(new LoginResponsePacket().Id, HandleLoginResponse);
            client.RegisterPacketHandler(new TimeUpdatePacket().Id, HandleTimeUpdate);

            client.RegisterPacketHandler(new ChunkPreamblePacket().Id, ChunkHandlers.HandleChunkPreamble);
            client.RegisterPacketHandler(new ChunkDataPacket().Id, ChunkHandlers.HandleChunkData);

            client.RegisterPacketHandler(new WindowItemsPacket().Id, InventoryHandlers.HandleWindowItems);
            client.RegisterPacketHandler(new SetSlotPacket().Id, InventoryHandlers.HandleSetSlot);
            client.RegisterPacketHandler(new CloseWindowPacket().Id, InventoryHandlers.HandleCloseWindowPacket);
            client.RegisterPacketHandler(new OpenWindowPacket().Id, InventoryHandlers.HandleOpenWindowPacket);
        }

        public static void HandleChatMessage(IPacket _packet, MultiplayerClient client)
        {
            var packet = (ChatMessagePacket)_packet;
            client.OnChatMessage(new ChatMessageEventArgs(packet.Message));
        }

        public static void HandleHandshake(IPacket _packet, MultiplayerClient client)
        {
            var packet = (HandshakeResponsePacket)_packet;
            if (packet.ConnectionHash != "-")
            {
                Console.WriteLine("Online mode is not supported");
            }
            // TODO: Authentication
            client.QueuePacket(new LoginRequestPacket(PacketReader.Version, client.User.Username));
        }

        public static void HandleLoginResponse(IPacket _packet, MultiplayerClient client)
        {
            var packet = (LoginResponsePacket)_packet;
            client.EntityID = packet.EntityID;
            client.QueuePacket(new PlayerGroundedPacket());
        }

        public static void HandlePositionAndLook(IPacket _packet, MultiplayerClient client)
        {
            var packet = (SetPlayerPositionPacket)_packet;
            client._Position = new Vector3(packet.X, packet.Y, packet.Z);
            client.QueuePacket(packet);
            client.IsLoggedIn = true;
            // TODO: Pitch and yaw
        }

        public static void HandleUpdateHealth(IPacket _packet, MultiplayerClient client)
        {
            
        }

        public static void HandleTimeUpdate(IPacket _packet, MultiplayerClient client)
        {
            var packet = (TimeUpdatePacket)_packet;
            client.World.World.TimeOfDay = packet.Time;
        }
    }
}