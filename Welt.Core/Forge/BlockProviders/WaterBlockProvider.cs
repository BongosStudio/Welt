
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class WaterBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.WATER;

        public override string Name => "water_still";

        public override string DisplayName => "Water";
        public override BlockEffect DisplayEffect => BlockEffect.LightLiquidEffect;
        public override float Hardness { get; set; } = -1;
        public override float Density => 0.3f;
        public override bool IsOpaque => false;
        public override bool IsSelectable => false;
        public override bool WillRenderSameNeighbor => false;
        public override bool IsSolid => false;
    }
}
