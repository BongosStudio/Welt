#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.API.Forge;
using Welt.Forge;
using Welt.Types;

#endregion

namespace Welt.Persistence
{
    public interface IChunkPersistence
    {

        void Save(IChunk chunk);

        IChunk Load(Vector3I index);

    }
}
