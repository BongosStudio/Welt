namespace Welt.Forge
{
    public static class WorldHelpers
    {
        public static uint GetIndexFromPosition(uint x, uint y, uint z, uint maxX, uint maxY, uint maxZ)
        {
            var flattenOffset = maxZ*maxY;

            return x*flattenOffset + z*maxY + y;
        }
    }
}