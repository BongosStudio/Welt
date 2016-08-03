using System;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Welt.Forge
{
    public static class WorldHelpers
    {
        public static uint GetIndexFromPosition(uint x, uint y, uint z, uint maxX, uint maxY, uint maxZ)
        {
            var flattenOffset = maxZ*maxY;

            return x*flattenOffset + z*maxY + y;
        }

        public static bool IsGroupingNear(this WorldObject world, Vector3 focalPoint, Vector3 distance, ushort block, float integrity)
        {
            var count = 0;
            var total = 0;
            for (var x = -distance.X; x < distance.X; x++)
            {
                for (var z = -distance.Z; z < distance.Z; z++)
                {
                    for (var y = -distance.Y; y < distance.Y; y++)
                    {
                        total++;
                        if (world.GetBlock(focalPoint + new Vector3(x, y, z)).Id == block) count++;
                    }
                }
            }
            return count/total >= integrity;
        }

        public static long GetSeedFromString(this string input)
        {
            return Encoding.ASCII.GetBytes(input)
                .Aggregate<byte, long>(0, (current, b) => (current + b)*input.GetHashCode());
        }
    }
}