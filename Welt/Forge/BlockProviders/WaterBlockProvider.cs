
namespace Welt.Forge.BlockProviders
{
    public class WaterBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.WATER;

        public override string BlockName => "water_still";

        public override string BlockTitle { get; set; } = "Water";
        public override float Hardness { get; set; } = 0;
        public override bool IsOpaque { get; set; } = true;
        public override bool IsSolid { get; set; } = false;
        public override bool HasCollision { get; set; } = false;
    }
}
