using Welt.API.Forge;
using Welt.Core;

namespace Welt.Forge.Processors
{
    public class LightingProcessor
    {
        private const byte ABSOLUTE_MAX_LIGHT = 15;
        private const byte ABSOLUTE_MIN_LIGHT = 0;

        private readonly byte _maxLightLevel;
        private readonly byte _minLightLevel;

        public LightingProcessor(IWorld world)
        {
            _maxLightLevel = (byte) (ABSOLUTE_MAX_LIGHT - world.SystemIndex);
            _minLightLevel = (byte) (ABSOLUTE_MAX_LIGHT/2 - world.SystemIndex);
            FastMath.Adjust(ABSOLUTE_MIN_LIGHT, ABSOLUTE_MAX_LIGHT, ref _minLightLevel);
            FastMath.Adjust(ABSOLUTE_MIN_LIGHT, ABSOLUTE_MAX_LIGHT, ref _maxLightLevel);
        }

        public void ClearLighting()
        {

        }
    }
}