using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Graphics;

namespace Welt.Forge.BlockProviders
{
    public class WoodBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.LOG;

        public override string BlockName => "log_oak";

        public override string BlockTitle { get; set; } = "Oak Log";

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
        {
            switch (faceDir)
            {
                case BlockFaceDirection.YIncreasing:
                case BlockFaceDirection.YDecreasing:
                    return TextureMap.GetTexture("log_oak_top", faceDir);
                default:
                    return TextureMap.GetTexture(BlockName, faceDir);
            }
        }
    }
}
