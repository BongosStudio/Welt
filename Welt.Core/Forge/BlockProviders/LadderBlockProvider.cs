using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;
using Welt.API.Net;

namespace Welt.Core.Forge.BlockProviders
{
    public class LadderBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.LADDER;

        public override string Name => "ladder";

        public override string DisplayName => "Ladder";

        public override bool IsOpaque => false;

        public override float Density => 0;

        public override BoundingBox? GetBoundingBox(byte metadata)
        {
            // lol this fukd
            switch (metadata)
            {
                case 0:
                    return new BoundingBox(new Vector3(0.9f, 0, 0.1f), new Vector3(1, 1, 0.9f));
                case 1:
                    return new BoundingBox(new Vector3(0, 0, 0), new Vector3(0.1f, 1, 1));
                case 2:
                    return new BoundingBox(new Vector3(0.1f, 0, 0.9f), new Vector3(0.9f, 1, 1));
                case 3:
                    return new BoundingBox(new Vector3(0.1f, 0, 0), new Vector3(0.9f, 1, 0.1f));
                default:
                    return base.GetBoundingBox(metadata);
            }
            

        }

        public override void BlockPlaced(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            if (world.GetBlock(FastMath.BlockFaceToCoordinates(face) + descriptor.Position).Id == Id) return;
            var dif = FastMath.BlockFaceToCoordinates(face);
            if (dif == Vector3I.Left)
                descriptor.Metadata = 1;
            else if (dif == Vector3I.Right)
                descriptor.Metadata = 0;
            else if (dif == Vector3I.Forward)
                descriptor.Metadata = 3;
            else if (dif == Vector3I.Backward)
                descriptor.Metadata = 2;
            else return;
            base.BlockPlaced(descriptor, face, world, user);
        }
    }
}
