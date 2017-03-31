using Welt.API.Net;
using Welt.Core.Net.Packets;
using Welt.Core.Forge;
using Welt.API;
using Welt.Events.Forge;

namespace Welt.Handlers
{
    internal static class ChunkHandlers
    {
        public static void HandleBlockChange(IPacket _packet, MultiplayerClient client)
        {
            // TODO:
            
        }

        public static void HandleChunkPreamble(IPacket _packet, MultiplayerClient client)
        {
            var packet = (ChunkPreamblePacket)_packet;
            client.World.World.SetChunk(new Vector3I(packet.X, 0, packet.Z), new Chunk(client.World.World, new Vector3I(packet.X, 0, packet.Z)));
        }

        public static void HandleChunkData(IPacket _packet, MultiplayerClient client)
        {
            var packet = (ChunkDataPacket)_packet;
            client.World.World.GetChunk(new Vector3I(packet.X, 0, packet.Z)).Fill(packet.CompressedData);
            client.OnChunkLoaded(new ChunkEventArgs(client.World.GetChunk(new Vector3I(packet.X, 0, packet.Z))));
        }
    }
}