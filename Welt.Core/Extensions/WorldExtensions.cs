using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Forge;
using Welt.Core.Forge;

namespace Welt.Core.Extensions
{
    public static class WorldExtensions
    {
        const float BASE_GRAVITY_VAL = -0.05859375f; // magic

        public static float GetGravity(this World world)
        {
            return world.Size * BASE_GRAVITY_VAL;
        }

        public static float GetMaxSpeed(this World world)
        {
            return world.GetGravity() * -35;
        }

        public static string GenerateRandomPlanetName(this ISolarSystem system)
        {
            // random names will begin with the system's spacial identifier
            return $"{system.Name}-{FastMath.NextRandom(1000, 9999)}";
        }

        public static string GenerateRandomSystemName(this IGalaxy galaxy)
        {
            return $"{galaxy.Name[0]}{char.ConvertFromUtf32(FastMath.NextRandom(char.MaxValue))}{FastMath.NextRandom(10, 99)}";
        }
    }
}
