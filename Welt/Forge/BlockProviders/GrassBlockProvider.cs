using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Graphics;
using Welt.Types;

namespace Welt.Forge.BlockProviders
{
    public class GrassBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.GRASS;

        public override string BlockName => "grass";

        public override string BlockTitle { get; set; } = "Grass Block";

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
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
                    return base.GetTexture(faceDir, blockAbove);
            }
        }

        public override Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            switch (face)
            {
                case BlockFaceDirection.YIncreasing:
                    return new Vector4(0f, 0.4f, 0f, 0.9f);
                default:
                    return base.GetOverlay(face, blockAbove);
            }
        }
    }
}
