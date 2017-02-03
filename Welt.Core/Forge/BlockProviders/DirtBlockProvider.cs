using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class DirtBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.DIRT;

        public override string Name => "dirt";

        public override string DisplayName => "Dirt";
    }
}
