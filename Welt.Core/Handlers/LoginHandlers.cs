using Microsoft.Xna.Framework;
using System;
using System.Linq;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Entities;
using Welt.Core.Net.Packets;
using Welt.Core.Server;

namespace Welt.Core.Handlers
{
    internal static class LoginHandlers
    {
        public static void HandleHandshakePacket(IPacket packet, IRemoteClient client, IMultiplayerServer server)
        {
            var handshakePacket = (HandshakePacket) packet;
            var remoteClient = (RemoteClient)client;
            remoteClient.Username = handshakePacket.Username;
            remoteClient.QueuePacket(new HandshakeResponsePacket("-")); // TODO: Implement some form of authentication
        }

        public static void HandleLoginRequestPacket(IPacket packet, IRemoteClient client, IMultiplayerServer server)
        {
            var loginRequestPacket = (LoginRequestPacket)packet;
            var remoteClient = (RemoteClient)client;
            if (loginRequestPacket.ProtocolVersion < server.PacketReader.ProtocolVersion)
                remoteClient.QueuePacket(new DisconnectPacket("Client outdated!"));
            else if (loginRequestPacket.ProtocolVersion > server.PacketReader.ProtocolVersion)
                remoteClient.QueuePacket(new DisconnectPacket("Server outdated!"));
            else if (server.Worlds.Count == 0)
                remoteClient.QueuePacket(new DisconnectPacket("Server has no worlds configured."));
            else if (!server.PlayerIsWhitelisted(remoteClient.Username) && server.PlayerIsBlacklisted(remoteClient.Username))
                remoteClient.QueuePacket(new DisconnectPacket("You're banned from this server"));
            else if (server.Clients.Count(c => c.Username == client.Username) > 1)
                remoteClient.QueuePacket(new DisconnectPacket("The player with this username is already logged in"));
            else
            {
                remoteClient.LoggedIn = true;
                remoteClient.Entity = new PlayerEntity(remoteClient.Username);
                remoteClient.World = server.Worlds[0];
                remoteClient.ChunkRadius = 2;

                if (!remoteClient.Load())
                    remoteClient.Entity.Position = remoteClient.World.SpawnPoint;
                // Make sure they don't spawn in the ground
                var collision = new Func<bool>(() =>
                {
                    var feet = client.World.GetBlock(client.Entity.Position);
                    var head = client.World.GetBlock(client.Entity.Position + Vector3.Up);
                    var feetBox = server.BlockRepository.GetBlockProvider(feet.Id).GetBoundingBox(feet.Metadata);
                    var headBox = server.BlockRepository.GetBlockProvider(head.Id).GetBoundingBox(head.Metadata);
                    return feetBox != null || headBox != null;
                });
                while (collision())
                    client.Entity.Position += Vector3.Up;

                var entityManager = server.GetEntityManagerForWorld(remoteClient.World);
                entityManager.SpawnEntity(remoteClient.Entity);

                // Send setup packets
                remoteClient.QueuePacket(new LoginResponsePacket(client.Entity.EntityID, 0));
                remoteClient.UpdateChunks();
                remoteClient.QueuePacket(new SpawnPositionPacket((int)remoteClient.Entity.Position.X,
                        (int)remoteClient.Entity.Position.Y, (int)remoteClient.Entity.Position.Z));
                remoteClient.QueuePacket(new SetPlayerPositionPacket(remoteClient.Entity.Position.X,
                        remoteClient.Entity.Position.Y + 1,
                        remoteClient.Entity.Position.Y + remoteClient.Entity.Size.Height + 1,
                        remoteClient.Entity.Position.Z, remoteClient.Entity.Yaw, remoteClient.Entity.Pitch, true));
                remoteClient.QueuePacket(new TimeUpdatePacket(remoteClient.World.TimeOfDay));

                // Start housekeeping for this client
                entityManager.SendEntitiesToClient(remoteClient);
                server.Scheduler.ScheduleEvent("remote.keepalive", remoteClient, TimeSpan.FromSeconds(10), remoteClient.SendKeepAlive);
                server.Scheduler.ScheduleEvent("remote.chunks", remoteClient, TimeSpan.FromSeconds(1), remoteClient.ExpandChunkRadius);

                if (!string.IsNullOrEmpty(server.ServerConfiguration.MessageOfTheDay))
                    remoteClient.SendMessage(server.ServerConfiguration.MessageOfTheDay);
                server.SendMessage($"{remoteClient.Username} joined the server.");
            }
        }
    }
}