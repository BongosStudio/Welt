using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class WoodBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.LOG;

        public override string Name => "log_oak";

        public override string DisplayName => "Oak Log";

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, byte metadata = 0, ushort blockAbove = 0)
        {
            switch (faceDir)
            {
                case BlockFaceDirection.YIncreasing:
                case BlockFaceDirection.YDecreasing:
                    return TextureMap.GetTexture("log_oak_top", faceDir);
                default:
                    return TextureMap.GetTexture(Name, faceDir);
            }
        }
    }
}
