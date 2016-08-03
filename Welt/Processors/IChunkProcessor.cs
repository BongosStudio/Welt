#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Forge;
using Welt.Forge.Builders;

namespace Welt.Processors
{
    public interface IChunkProcessor
    {
        void ProcessChunk(ChunkBuilder chunk);
    }
}
