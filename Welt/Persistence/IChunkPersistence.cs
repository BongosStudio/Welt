#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge;
using Welt.Types;

#endregion

namespace Welt.Persistence
{
    public interface IChunkPersistence
    {

        void Save(Chunk chunk);

        Chunk Load(Vector3I index);

    }
}
