using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class RoseBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.FLOWER_ROSE;

        public override string Name => "flower_rose";

        public override string DisplayName => "Rose";

        public override bool IsOpaque => false;
        public override bool WillRenderOpaque => false;
        public override bool WillRenderSameNeighbor => true;
        public override float Density => 0.1f;
        public override BlockEffect DisplayEffect => BlockEffect.VegetationWindEffect;

        public override BoundingBox? GetBoundingBox(byte metadata)
        {
            return new BoundingBox(new Vector3(0.4f, 0, 0.4f), new Vector3(0.6f));
        }
    }
}
