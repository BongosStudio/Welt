
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class SandBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.SAND;
        public override string Name => "sand";

        public override string DisplayName => "Sand";
    }
}
