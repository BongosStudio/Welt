using Microsoft.Xna.Framework;
using Welt.Blocks;

namespace Welt.Forge.BlockProviders
{
    public class LeavesBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.LEAVES;

        public override string BlockName => "leaves_oak";

        public override string BlockTitle { get; set; } = "Leaves";
        public override bool IsOpaque { get; set; } = true;

        public override Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            return new Vector4(0.1f, 0.3f, 0f, 1f);
        }
    }
}
