using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;
using System.IO;
using Welt.API;
using Welt.API.Net;
using Welt.Controllers;
using Welt.Core.Forge;
using Welt.Core.Net;
using Welt.Core.Net.Packets;
using Welt.Events;

namespace Welt.Handlers
{
    internal static class PacketHandlers
    {
        public static void RegisterHandlers(MultiplayerClient client)
        {
            client.RegisterPacketHandler(new HandshakeResponsePacket().Id, HandleHandshake);
            client.RegisterPacketHandler(new ChatMessagePacket().Id, HandleChatMessage);
            client.RegisterPacketHandler(new SpawnPositionPacket().Id, HandleSpawnPosition);
            client.RegisterPacketHandler(new SetPlayerPositionPacket().Id, HandleSetPlayerPosition);
            client.RegisterPacketHandler(new PlayerPositionPacket().Id, HandlePlayerPosition);
            client.RegisterPacketHandler(new PlayerLookPacket().Id, HandlePlayerLook);
            client.RegisterPacketHandler(new LoginResponsePacket().Id, HandleLoginResponse);
            client.RegisterPacketHandler(new TimeUpdatePacket().Id, HandleTimeUpdate);

            client.RegisterPacketHandler(new ChunkPreamblePacket().Id, ChunkHandlers.HandleChunkPreamble);
            client.RegisterPacketHandler(new ChunkDataPacket().Id, ChunkHandlers.HandleChunkData);

            client.RegisterPacketHandler(new ScreenshotRequestPacket().Id, HandleScreenshotRequest);

            client.RegisterPacketHandler(new KeepAlivePacket().Id, HandleKeepAlive);
            client.RegisterPacketHandler(new DisconnectPacket().Id, HandleDisconnect);
        }

        public static void HandleKeepAlive(IPacket _packet, MultiplayerClient client)
        {
            //client.QueuePacket(new KeepAlivePacket());
        }

        public static void HandleChatMessage(IPacket _packet, MultiplayerClient client)
        {
            var packet = (ChatMessagePacket)_packet;
            client.OnChatMessage(new Events.ChatMessageEventArgs(packet.Message));
        }

        public static void HandleHandshake(IPacket _packet, MultiplayerClient client)
        {
            var packet = (HandshakeResponsePacket)_packet;
            if (packet.ConnectionHash != "-")
            {
                Console.WriteLine("Online mode is not supported");
            }
            // TODO: Authentication
            client.OnServerDiscovered(new ServerDiscoveredEventArgs(packet.ServerName, null, packet.ServerMotd, packet.OnlineUsers, packet.MaxUsers));
        }

        public static void HandleDisconnect(IPacket _packet, MultiplayerClient client)
        {
            var packet = (DisconnectPacket)_packet;
            Console.WriteLine(packet.Reason);
            SceneController.ShowError(packet.Reason);
        }

        public static void HandleLoginResponse(IPacket _packet, MultiplayerClient client)
        {
            var packet = (LoginResponsePacket)_packet;
            client.EntityId = packet.EntityID;
            client.QueuePacket(new PlayerGroundedPacket());
            client.World.World = new World(packet.WorldName, packet.Seed);
        }

        public static void HandleSpawnPosition(IPacket _packet, MultiplayerClient client)
        {
            var packet = (SpawnPositionPacket)_packet;
            client.World.World.SpawnPoint = new Vector2(packet.X, packet.Z);
        }

        public static void HandleSetPlayerPosition(IPacket _packet, MultiplayerClient client)
        {
            var packet = (SetPlayerPositionPacket)_packet;
            client._Position = new Vector3(packet.X, packet.Y, packet.Z);
            Console.WriteLine($"Set player position to {client._Position}");
            //client.QueuePacket(packet);
            client.IsLoggedIn = true;
            // TODO: Pitch and yaw
        }

        public static void HandlePlayerPositionAndLook(IPacket _packet, MultiplayerClient client)
        {
            var packet = (PlayerPositionAndLookPacket)_packet;
            client._Position = new Vector3(packet.X, packet.Y, packet.Z);
            client.Yaw = packet.Yaw;
            client.Pitch = packet.Pitch;
        }

        public static void HandlePlayerPosition(IPacket _packet, MultiplayerClient client)
        {
            var packet = (PlayerPositionPacket)_packet;
            client._Position = new Vector3(packet.X, packet.Y, packet.Z);
        }

        public static void HandlePlayerLook(IPacket _packet, MultiplayerClient client)
        {
            var packet = (PlayerLookPacket)_packet;
            client.Yaw = packet.Yaw;
            client.Pitch = packet.Pitch;
        }

        public static void HandleUpdateHealth(IPacket _packet, MultiplayerClient client)
        {
            
        }

        public static void HandleTimeUpdate(IPacket _packet, MultiplayerClient client)
        {
            var packet = (TimeUpdatePacket)_packet;
            //client.World.World.TimeOfDay = packet.Time;
        }

        public static void HandleScreenshotRequest(IPacket _packet, MultiplayerClient client)
        {
            var data = WeltGame.Instance.GetScreen();
            client.QueuePacket(new ScreenshotResultPacket { Data = data });
        }
    }
}