using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Forge;

namespace Welt.Forge
{
    public class ReadOnlyWorld
    {
        internal World World { get; set; }

        public Vector3I GetSpawnPoint() => World.SpawnPoint;

        public Block GetBlock(Vector3I position) => World.GetBlock(position);
        public Block GetBlock(uint x, uint y, uint z) => World.GetBlock(x, y, z);

        public void SetBlock(Vector3I position, Block block)
        {
            World.SetBlock(position, block);
        }

        public ReadOnlyChunk GetChunk(Vector3I index) => new ReadOnlyChunk(World.GetChunk(index) as Chunk);
        public ReadOnlyChunk[] GetChunks() => World.ChunkManager.Chunks.Select(c => new ReadOnlyChunk(c)).ToArray();
        public void SetChunk(Vector3I index, ReadOnlyChunk chunk)
        {
            World.SetChunk(index, chunk.Chunk);
        }
    }
}
