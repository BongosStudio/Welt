using Welt.Core.Forge;
using Welt.Forge.Processors;

namespace Welt.Controllers
{
    // To be contained within an IRenderer
    public class WorldController
    {
        public VertexProcessor VertexProcessor;
        public LightingProcessor LightingProcessor;
        public World World;

    }
}