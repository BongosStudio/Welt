
using Microsoft.Xna.Framework;
using System;
using Welt.Blocks;
using Welt.Types;

namespace Welt.Forge.BlockProviders
{
    public class TorchBlockProvider : BlockProvider
    {
        private static Vector3B[] m_LightColor = new[]
        {
            new Vector3B(14, 7, 3), // orange
            new Vector3B(3, 14, 3), // green? idk
            new Vector3B(3, 3, 14), // blueish
            new Vector3B(14, 3, 3), // redish
            new Vector3B(7, 3, 14), // purplish
        };

        public override ushort BlockId => BlockType.TORCH;
        public override string BlockName => "torch_on";
        public override string BlockTitle { get; set; } = "Torch";
        public override bool IsOpaque { get; set; } = true;
        public override bool HasCollision { get; set; } = false;

        public override Vector3B GetLightLevel(byte metadata)
        {
            return m_LightColor[NibbleArray.GetData(metadata).Item1];
        }

        public static BlockFaceDirection GetPost(byte meta)
        {
            return (BlockFaceDirection)NibbleArray.GetData(meta).Item2;
        }

        public override BoundingBox GetBoundingBox(byte metadata)
        {
            return new BoundingBox(new Vector3(0.4f, 0, 0.4f), new Vector3(0.6f));
        }
    }
}
