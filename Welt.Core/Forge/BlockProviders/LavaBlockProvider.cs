using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class LavaBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.LAVA;

        public override string Name => "lava_still";

        public override string DisplayName => "Lava";

        public override float Density => 0.7f;
    }
}
