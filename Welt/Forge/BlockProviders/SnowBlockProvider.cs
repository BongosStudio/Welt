
using Microsoft.Xna.Framework;

namespace Welt.Forge.BlockProviders
{
    public class SnowBlockProvider : BlockProvider
    {
        public override ushort BlockId => BlockType.SNOW;

        public override string BlockName => "snow";

        public override string BlockTitle { get; set; } = "Snow";
        public override bool IsSolid { get; set; } = false;
        public override bool HasCollision { get; set; } = false;

        public override BoundingBox GetBoundingBox(byte metadata)
        {
            return new BoundingBox(Vector3.Zero, new Vector3(1, 0.2f, 1));
        }
    }
}
