using System;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Net.Packets;
using Welt.Core.Server;

namespace Welt.Core.Handlers
{
    public static class PacketHandlers
    {
        public static void RegisterHandlers(IMultiplayerServer server)
        {
            server.RegisterPacketHandler(new KeepAlivePacket().Id, HandleKeepAlive);
            server.RegisterPacketHandler(new ChatMessagePacket().Id, HandleChatMessage);
            server.RegisterPacketHandler(new DisconnectPacket().Id, HandleDisconnect);

            server.RegisterPacketHandler(new HandshakePacket().Id, LoginHandlers.HandleHandshakePacket);
            server.RegisterPacketHandler(new LoginRequestPacket().Id, LoginHandlers.HandleLoginRequestPacket);

            server.RegisterPacketHandler(new PlayerGroundedPacket().Id, (a, b, c) => { /* no-op */ });
            server.RegisterPacketHandler(new PlayerPositionPacket().Id, EntityHandlers.HandlePlayerPositionPacket);
            server.RegisterPacketHandler(new PlayerLookPacket().Id, EntityHandlers.HandlePlayerLookPacket);
            server.RegisterPacketHandler(new PlayerPositionAndLookPacket().Id, EntityHandlers.HandlePlayerPositionAndLookPacket);
            server.RegisterPacketHandler(new SetPlayerPositionPacket().Id, EntityHandlers.HandleSetPlayerPositionPacket);

            server.RegisterPacketHandler(new PlayerDiggingPacket().Id, InteractionHandlers.HandlePlayerDiggingPacket);
            server.RegisterPacketHandler(new PlayerBlockPlacementPacket().Id, InteractionHandlers.HandlePlayerBlockPlacementPacket);
            
        }

        internal static void HandleKeepAlive(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            // TODO
        }

        internal static void HandleChatMessage(IPacket _packet, IRemoteClient _client, IMultiplayerServer _server)
        {
            // TODO: Abstract this to support things like commands
            // TODO: Sanitize messages
            var packet = (ChatMessagePacket)_packet;
            var server = (MultiplayerServer)_server;
            var args = new ChatMessageEventArgs(_client, packet.Message);
            server.OnChatMessageReceived(args);
        }

        internal static void HandleDisconnect(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (DisconnectPacket)_packet;
            Console.WriteLine(packet.Reason);
        }
    }
}