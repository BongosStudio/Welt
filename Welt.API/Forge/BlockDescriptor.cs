
namespace Welt.API.Forge
{
    public struct BlockDescriptor
    {
        public ushort Id;
        public byte Metadata;
        public Vector3B BlockLight;
        public byte SkyLight;
        // Optional
        public Vector3I Position;
        // Optional
        public IChunk Chunk;

        public static BlockDescriptor FromBlock(Block block)
        {
            return new BlockDescriptor
            {
                Id = block.Id,
                Metadata = block.Metadata,
                BlockLight = new Vector3B(block.R, block.G, block.B),
                SkyLight = block.Sun
            };
        }
    }
}