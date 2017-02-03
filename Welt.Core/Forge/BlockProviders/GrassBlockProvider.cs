using Microsoft.Xna.Framework;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class GrassBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.GRASS;

        public override string Name => "grass";

        public override string DisplayName => "Grass Block";

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, byte metadata = 0, ushort blockAbove = 0)
        {
            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                case BlockFaceDirection.XDecreasing:
                case BlockFaceDirection.ZIncreasing:
                case BlockFaceDirection.ZDecreasing:
                    return TextureMap.GetTexture("grass_side", faceDir);
                case BlockFaceDirection.YIncreasing:
                    return TextureMap.GetTexture("grass_top", faceDir);
                case BlockFaceDirection.YDecreasing:
                    return TextureMap.GetTexture("dirt", faceDir);
                default:
                    return base.GetTexture(faceDir, metadata, blockAbove);
            }
        }

        public override Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            switch (face)
            {
                case BlockFaceDirection.YIncreasing:
                    return new Vector4(-0.2f, 0.5f, -0.2f, 0.9f);
                default:
                    return base.GetOverlay(face, blockAbove);
            }
        }
    }
}
