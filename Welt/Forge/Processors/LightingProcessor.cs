using System;
using System.Threading.Tasks;
using Welt.API.Forge;
using Welt.Core;
using Welt.Core.Forge;
using Welt.Forge.Builders;

namespace Welt.Forge.Processors
{
    public class LightingProcessor : IChunkProcessor
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

        public ProcessorStatus Status { get; }

        public void ClearLighting()
        {

        }

        public async Task<ChunkBuilder> ProcessChunk(Chunk chunk)
        {
            throw new NotImplementedException();
        }
    }
}