using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Graphics;
using System.Diagnostics;
using Welt.Models;
using Welt.Cameras;

namespace Welt.Forge.BlockProviders
{
    public class LadderBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.LADDER;

        public override string BlockName => "ladder";

        public override string BlockTitle { get; set; } = "Ladder";

        public override bool IsOpaque { get; set; } = true;
        public override bool HasCollision { get; set; } = false;

        public override void PlaceBlock(World world, Vector3 position, Vector3 adjacent, Block block)
        {
            if (world.GetBlock(position).Id == BlockId) return;
            var dif = position - adjacent;
            if (dif == Vector3.Left)
                block.Metadata = 1;
            else if (dif == Vector3.Right)
                block.Metadata = 0;
            else if (dif == Vector3.Forward)
                block.Metadata = 3;
            else if (dif == Vector3.Backward)
                block.Metadata = 2;
            else return;
            base.PlaceBlock(world, position, adjacent, block);
        }

        public override BoundingBox GetBoundingBox(byte metadata)
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

        public override Vector2[] GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
        {
            return base.GetTexture(faceDir, blockAbove);
        }
    }
}
