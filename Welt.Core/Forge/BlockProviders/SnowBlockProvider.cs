
using Microsoft.Xna.Framework;
using Welt.API.Forge;

namespace Welt.Core.Forge.BlockProviders
{
    public class SnowBlockProvider : BlockProvider
    {
        public override ushort Id => BlockType.SNOW;

        public override string Name => "snow";

        public override string DisplayName => "Snow";
        public override bool IsSolid => false;

        public override BoundingBox? GetBoundingBox(byte metadata)
        {
            return new BoundingBox(Vector3.Zero, new Vector3(1, 0.2f, 1));
        }
    }
}
