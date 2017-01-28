using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Blocks;

namespace Welt.Forge.BlockProviders
{
    public class DefaultBlockProvider : BlockProvider
    {
        public override ushort BlockId => 0;

        public override string BlockName => "air";

        public override string BlockTitle { get; set; } = "Air";
        public override bool IsSolid { get; set; } = false;
        public override bool HasCollision { get; set; } = false;
        public override bool IsOpaque { get; set; } = true;
        public override float Hardness { get; set; } = 0;

        public override void PlaceBlock(World world, Vector3 position, Vector3 adjacent, Block block)
        {
            // do nothing
        }

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
        {
            return new Vector2[0];
        }
    }
}
