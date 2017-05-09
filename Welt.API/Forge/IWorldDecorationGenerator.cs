using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    /// <summary>
    ///     The base interface for world decoration such as towns, villages, points of interest, etc.
    /// </summary>
    public interface IWorldDecorationGenerator
    {
        /// <summary>
        ///     A value between 0.00 and 1.00 of the chance to spawn within world.
        /// </summary>
        double SpawnChance { get; }
        /// <summary>
        ///     Before each attempt to generate, the base position will be passed into this function to test if the position is 
        ///     suitable for building.
        /// </summary>
        Func<Vector3B, bool> PositionRequirements { get; }
        /// <summary>
        ///     Determines whether or not a block will be removed if the decorator attempts to fill a spot already occupied in 
        ///     the world.
        /// </summary>
        bool WillRemoveCollisionPoints { get; }

        /// <summary>
        ///     Executes the generator on the world.
        /// </summary>
        /// <param name="world"></param>
        void Generate(IWorld world);
    }
}
