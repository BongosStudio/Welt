using System;
using System.Linq;
using Welt.Core.Forge;
using Welt.Forge.Builders;

namespace Welt.Processors
{
    public class WorldRenderingProcessor
    {
        // this class exists only to run parallel tasks of building the meshes, light, vertices,
        // and generation. Reflection has yet to be implemented.
        public readonly IChunkProcessor[] ChunkProcessors;

        public WorldRenderingProcessor(params IChunkProcessor[] processors)
        {
            ChunkProcessors = processors;
        }

        public void Process(ChunkBuilder builder)
        {
            foreach (var processor in ChunkProcessors)
                processor.ProcessChunk(builder);
        }

        public void Process<T>(ChunkBuilder builder) where T : IChunkProcessor
        {
            foreach (var p in ChunkProcessors.Where(p => typeof(T) == p.GetType()))
            {
                p.ProcessChunk(builder);
                return;
            }
            throw new TypeLoadException($"This instance does not have {typeof (T).Name} loaded");
        }
    }
}