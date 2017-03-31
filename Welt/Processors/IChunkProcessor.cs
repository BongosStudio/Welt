#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using Welt.Core.Forge;
using Welt.Forge;

namespace Welt.Processors
{
    public interface IChunkProcessor
    {
        void ProcessChunk(Chunk chunk);
    }
}
