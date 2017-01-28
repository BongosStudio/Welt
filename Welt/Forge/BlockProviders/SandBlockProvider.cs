
namespace Welt.Forge.BlockProviders
{
    public class SandBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.SAND;
        public override string BlockName => "sand";

        public override string BlockTitle { get; set; } = "Sand";
    }
}
