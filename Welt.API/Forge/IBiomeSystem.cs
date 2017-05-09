using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IBiomeSystem
    {
        void RegisterBiome<TBiome>(ITerrainGenerator terrain, IChunkDecorationGenerator decorator) where TBiome : IBiome;

        IChunk GenerateChunk(Vector3I index);
    }
}
