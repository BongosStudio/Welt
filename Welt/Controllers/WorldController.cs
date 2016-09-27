using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Forge.Processors;

namespace Welt.Controllers
{
    // To be contained within an IRenderer
    public class WorldController
    {
        public VertexProcessor VertexProcessor;
        public LightingProcessor  LightingProcessor;
    }
}
