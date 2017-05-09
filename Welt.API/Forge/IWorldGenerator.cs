using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IWorldGenerator
    {
        /// <summary>
        ///     Registers the decorator to be used in the generator.
        /// </summary>
        /// <param name="decorator"></param>
        void RegisterDecorator(IWorldDecorationGenerator decorator);

        /// <summary>
        ///     Registers the biome system to be used in the generator.
        /// </summary>
        /// <param name="biomeSystem"></param>
        void RegisterBiomeSystem(IBiomeSystem biomeSystem);

        /// <summary>
        ///     Generates and returns the chunk at the specified index.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        IChunk GenerateChunk(Vector3I index);
    }
}
