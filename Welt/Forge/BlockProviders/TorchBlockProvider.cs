
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
        public override string BlockName => "torch";
        public override string BlockTitle
        {
            get
            {
                return m_BlockTitle;
            }

            set
            {
                m_BlockTitle = value;
            }
        }
        private string m_BlockTitle = "Torch";

        public static Vector3B GetLightLevel(byte meta)
        {
            return m_LightColor[NibbleArray.GetData(meta).Item1];
        }

        public static BlockFaceDirection GetPost(byte meta)
        {
            return (BlockFaceDirection)NibbleArray.GetData(meta).Item2;
        }

        public override BlockTexture GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
        {
            throw new NotImplementedException();
        }
    }
}
