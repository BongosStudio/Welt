using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    /// <summary>
    ///     An interface that provides randomized block position placement within an <see cref="IChunk"/>.
    /// </summary>
    public interface ITerrainGenerator
    {
        /// <summary>
        ///     The minimum height for surface generation.
        /// </summary>
        int SurfaceMin { get; }
        /// <summary>
        ///     The maximum height for surface generation.
        /// </summary>
        int SurfaceMax { get; }

        /// <summary>
        ///     The minimum depth between the surface and sublayers.
        /// </summary>
        int SurfaceDepthMin { get; }
        /// <summary>
        ///     The maximum depth between the surface and sublayers.
        /// </summary>
        int SurfaceDepthMax { get; }

        /// <summary>
        ///     The minimum amount of sublayers, typically containing caves and underground structures.
        /// </summary>
        int SubLayersMin { get; }
        /// <summary>
        ///     The maximum amount of sublayers, typically containing caves and underground structures.
        /// </summary>
        int SubLayersMax { get; }

        /// <summary>
        ///     The minimum amount of peaks on the surface.
        /// </summary>
        int SurfaceMinPeakCount { get; }
        /// <summary>
        ///     The maximum amount of peaks on the surface.
        /// </summary>
        int SurfaceMaxPeakCount { get; }

        /// <summary>
        ///     The steepness of height from where the waterline of the <see cref="IWorld"/> and the land meet. Negative values create a smoothing and positive values result
        ///     in a steeper drop. Minimum is -30 and maximum is 30.
        /// </summary>
        int WaterLineSteep { get; }
    }
}
