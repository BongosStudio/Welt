using Microsoft.Xna.Framework;
using System;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Extensions;
using Welt.Core.Net.Packets;

namespace Welt.Core.Handlers
{
    internal static class EntityHandlers
    {
        public static void HandleSetPlayerPositionPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (SetPlayerPositionPacket)_packet;
            _client.Entity.Position = new Vector3(packet.X, packet.Y, packet.Z);
            _client.Entity.Pitch = packet.Pitch;
            _client.Entity.Yaw = packet.Yaw;
        }

        public static void HandlePlayerPositionPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (PlayerPositionPacket)_packet;
            HandlePlayerMovement(_client, new Vector3(packet.X, packet.Y, packet.Z), _client.Entity.Yaw, _client.Entity.Pitch);

        }

        public static void HandlePlayerLookPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (PlayerLookPacket)_packet;
            HandlePlayerMovement(_client, _client.Entity.Position, packet.Yaw, packet.Pitch);
        }

        public static void HandlePlayerPositionAndLookPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (PlayerPositionAndLookPacket)_packet;
            HandlePlayerMovement(_client, new Vector3(packet.X, packet.Y, packet.Z), packet.Yaw, packet.Pitch);
        }

        public static void HandlePlayerMovement(IRemoteClient client, Vector3 position, float yaw, float pitch)
        {
            //if (client.Entity.Position.DistanceTo(position) > 10)
            //{
            //    //client.QueuePacket(new DisconnectPacket("The server determined you moved faster than allowed."));
            //    client.Entity.Position = position;
            //    client.Entity.Yaw = yaw;
            //    client.Entity.Pitch = pitch;
            //}
            //else
            {
                client.Entity.Position = position;
                client.Entity.Yaw = yaw;
                client.Entity.Pitch = pitch;
            }
        }
    }
}