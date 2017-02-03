using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class StoneBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.STONE;

        public override string Name => "stone";

        public override string DisplayName => "Stone";
    }
}
