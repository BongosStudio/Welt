using Microsoft.Xna.Framework;
using System;
using Welt.API;
using Welt.API.Forge;
using Welt.API.Net;
using Welt.Core.Entities;
using Welt.Core.Forge;
using Welt.Core.Net.Packets;
using Welt.Core.Server;

namespace Welt.Core.Handlers
{
    public static class InteractionHandlers
    {
        public static void HandlePlayerDiggingPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (PlayerDiggingPacket)_packet;
            var client = (RemoteClient)_client;
            var world = _client.World;
            var position = new Vector3I(packet.X, packet.Y, packet.Z);
            var descriptor = world.GetBlockData(position);
            var provider = server.BlockRepository.GetBlockProvider(descriptor.Id);
            short damage;
            int time;
            switch (packet.PlayerAction)
            {
                case PlayerDiggingPacket.Action.DropItem:
                    // Throwing item
                    if (client.SelectedItem.Count == 0)
                        break;
                    var spawned = client.SelectedItem;
                    spawned.Count = 1;
                    var inventory = client.SelectedItem;
                    inventory.Count--;
                    var item = new ItemEntity(client.Entity.Position + new Vector3(0, PlayerEntity.Height, 0), spawned)
                    {
                        Velocity = FastMath.RotateY(Vector3.Forward, FastMath.DegreesToRadians(client.Entity.Yaw)) * 0.5f
                    };
                    client.Inventory[client.SelectedSlot] = inventory;
                    server.GetEntityManagerForWorld(client.World).SpawnEntity(item);
                    break;
                case PlayerDiggingPacket.Action.StartDigging:
                    foreach (var nearbyClient in server.Clients) // TODO: Send this repeatedly during the course of the digging
                    {
                        var c = (RemoteClient)nearbyClient;
                        // send animation packet to set animation to digging
                    }
                    if (provider == null)
                        server.SendMessage($"WARNING: block provider for ID {descriptor.Id} is null (player digging)");
                    else
                        provider.BlockHit(descriptor, packet.Face, world, client);

                    time = BlockProvider.GetHarvestTime(descriptor.Id, client.SelectedItem.Block.Id, out damage);
                    if (time == 0)
                    {
                        provider.BlockMined(descriptor, packet.Face, world, client);
                    }
                    else
                    {
                        client.ExpectedDigComplete = DateTime.UtcNow.AddMilliseconds(time);
                    }
                    break;
                case PlayerDiggingPacket.Action.StopDigging:
                    foreach (var nearbyClient in server.Clients)
                    {
                        var c = (RemoteClient)nearbyClient;
                        // send animation update to stop digging animation to all clients
                    }
                    if (provider != null && descriptor.Id != 0)
                    {
                        time = BlockProvider.GetHarvestTime(descriptor.Id, client.SelectedItem.Block.Id, out damage);
                        if (time <= 20)
                            break; // Already handled earlier
                        var diff = (DateTime.UtcNow - client.ExpectedDigComplete).TotalMilliseconds;
                        if (diff > -100) // Allow a small tolerance
                        {
                            provider.BlockMined(descriptor, packet.Face, world, client);
                            // Damage the item
                            if (damage != 0)
                            {
                                //var tool = server.ItemRepository.GetItemProvider(client.SelectedItem.Block.Id) as ToolItem;
                                //if (tool != null && tool.Uses != -1)
                                //{
                                //    var slot = client.SelectedItem;
                                //    slot.Metadata += damage;
                                //    if (slot.Metadata >= tool.Uses)
                                //        slot.Count = 0; // Destroy item
                                //    client.Inventory[client.SelectedSlot] = slot;
                                //}
                            }
                        }
                    }
                    break;
            }
        }

        public static void HandlePlayerBlockPlacementPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            var packet = (PlayerBlockPlacementPacket)_packet;
            var client = (RemoteClient)_client;

            var slot = client.SelectedItem;
            var position = new Vector3I(packet.X, packet.Y, packet.Z);
            BlockDescriptor? block = null;
            if (position.DistanceTo(client.Entity.Position) > 10 /* TODO: Reach */)
                return;
            block = client.World.GetBlockData(position);
            bool use = true;
            if (block != null)
            {
                var provider = server.BlockRepository.GetBlockProvider(block.Value.Id);
                if (provider == null)
                {
                    server.SendMessage($"WARNING: block provider for ID {block.Value.Id} is null (player placing)");
                    server.SendMessage($"Error occured from client {client.Username} at coordinates {block.Value.Position}");
                    server.SendMessage($"Packet logged at {DateTime.UtcNow}, please report upstream");
                    return;
                }
                if (!provider.BlockInteractedWith(block.Value, packet.Face, client.World, client))
                {
                    position += FastMath.BlockFaceToCoordinates(packet.Face);
                    var oldBlock = client.World.GetBlock(position);
                    // send block change and inventory update packet
                    return;
                }
            }
            if (slot.Count != 0)
            {
                if (use)
                {
                    var itemProvider = server.ItemRepository.GetItemProvider(slot.Block.Id);
                    if (itemProvider == null)
                    {
                        server.SendMessage($"WARNING: item provider for ID {block.Value.Id} is null (player placing)");
                        server.SendMessage($"Error occured from client {client.Username} at coordinates {block.Value.Position}");
                        server.SendMessage($"Packet logged at {DateTime.UtcNow}, please report upstream");
                    }
                    if (block != null)
                    {
                        if (itemProvider != null)
                            itemProvider.ItemUsedOnBlock(position, slot, packet.Face, client.World, client);
                    }
                    else
                    {
                        // TODO: Use item
                    }
                }
            }
        }

        public static void HandleClickWindowPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {

        }

        public static void HandleCloseWindowPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            
        }

        public static void HandleChangeHeldItem(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            
        }

        public static void HandlePlayerAction(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            //var packet = (PlayerActionPacket)_packet;
            //var client = (RemoteClient)_client;
            //var entity = (PlayerEntity)client.Entity;
            //switch (packet.Action)
            //{
            //    case PlayerActionPacket.PlayerAction.Crouch:
            //        entity.EntityFlags |= EntityFlags.Crouched;
            //        break;
            //    case PlayerActionPacket.PlayerAction.Uncrouch:
            //        entity.EntityFlags &= ~EntityFlags.Crouched;
            //        break;
            //}
        }

        public static void HandleAnimation(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            //var packet = (AnimationPacket)_packet;
            //var client = (RemoteClient)_client;
            //if (packet.EntityID == client.Entity.EntityID)
            //{
            //    var nearby = server.GetEntityManagerForWorld(client.World)
            //        .ClientsForEntity(client.Entity);
            //    foreach (var player in nearby)
            //        player.QueuePacket(packet);
            //}
        }

        public static void HandleUpdateSignPacket(IPacket _packet, IRemoteClient _client, IMultiplayerServer server)
        {
            
        }
    }
}