using System;
using MonoGame.Utilities;
using Welt.API.Net;

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
            // TODO: ensure world has a chunk object for this packet
        }

        public static void HandleChunkData(IPacket _packet, MultiplayerClient client)
        {
            // TODO: load chunk and chunk data
        }
    }
}