using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Forge;

namespace Welt.Core.Forge.Generation
{
    public class HillGenerator : ITerrainGenerator
    {
        public int SubLayersMax => 2;

        public int SubLayersMin => 1;

        public int SurfaceDepthMax => 5;

        public int SurfaceDepthMin => 3;

        public int SurfaceMax => 200;

        public int SurfaceMaxPeakCount => 2;

        public int SurfaceMin => 120;

        public int SurfaceMinPeakCount => 0;

        public int WaterLineSteep => 0;
    }
}
