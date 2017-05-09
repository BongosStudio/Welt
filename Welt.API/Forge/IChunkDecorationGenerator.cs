using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    /// <summary>
    ///     The base interface for chunk decoration such as trees, plants, etc.
    /// </summary>
    public interface IChunkDecorationGenerator
    {
        /// <summary>
        ///     A value between 0.00 and 1.00 of the chance to spawn within chunk.
        /// </summary>
        double SpawnChance { get; }
        /// <summary>
        ///     Before each attempt to generate, the base position will be passed into this function to test if the position is 
        ///     suitable for building.
        /// </summary>
        Func<Vector3B, bool> PositionRequirements { get; }
        /// <summary>
        ///     Determines whether or not a block will be removed if the decorator attempts to fill a spot already occupied in 
        ///     the chunk.
        /// </summary>
        bool WillRemoveCollisionPoints { get; }
        /// <summary>
        ///     A bounding box representing the 3 dimensional space required to generate.
        /// </summary>
        BoundingBox BoundingBox { get; }

        /// <summary>
        ///     Executes the generator on the chunk.
        /// </summary>
        /// <param name="chunk"></param>
        void Generate(IChunk chunk);
    }
}
