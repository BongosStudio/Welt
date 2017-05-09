using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;
using Welt.API.Net;

namespace Welt.Core.Forge.BlockProviders
{
    public class DefaultBlockProvider : BlockProvider
    {
        public override ushort Id => 0;

        public override string Name => "air";

        public override string DisplayName => "Air";
        public override bool IsSolid => false;
        public override bool IsOpaque => false;
        public override bool WillRenderOpaque => IsOpaque;
        public override bool WillRenderSameNeighbor => true;
        public override float Hardness { get; set; } = 0;

        public override void BlockPlaced(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            return;
        }
    }
}
