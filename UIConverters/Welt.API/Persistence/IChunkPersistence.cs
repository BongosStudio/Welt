#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API.Forge;

namespace Welt.API.Persistence
{
    public interface IChunkPersistence
    {
        void Load(string directory);
        void Save(string directory);

        IChunk GetChunk(uint x, uint z);
        void SetChunk(uint x, uint z, IChunk value);

    }
}