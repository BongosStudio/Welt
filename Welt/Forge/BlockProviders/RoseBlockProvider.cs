using Microsoft.Xna.Framework;

namespace Welt.Forge.BlockProviders
{
    public class RoseBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.FLOWER_ROSE;

        public override string BlockName => "flower_rose";

        public override string BlockTitle { get; set; } = "Rose";

        public override bool HasCollision { get; set; } = false;
        public override bool IsOpaque { get; set; } = true;
        public override bool IsPlantBlock { get; set; } = true;
        public override BoundingBox GetBoundingBox(byte metadata)
        {
            return new BoundingBox(new Vector3(0.4f, 0, 0.4f), new Vector3(0.6f));
        }
    }
}
