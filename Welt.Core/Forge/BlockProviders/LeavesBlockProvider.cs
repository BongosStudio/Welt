using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class LeavesBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.LEAVES;

        public override string Name => "leaves_oak";

        public override string DisplayName => "Leaves";
        public override bool IsOpaque => false;
        public override BlockEffect DisplayEffect => BlockEffect.VegetationWindEffect;

        public override Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            return new Vector4(0.1f, 0.3f, 0f, 1f);
        }
    }
}
