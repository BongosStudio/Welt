namespace Welt.Forge.BlockProviders
{
    public class StoneBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.STONE;

        public override string BlockName => "stone";

        public override string BlockTitle { get; set; } = "Stone";
    }
}
