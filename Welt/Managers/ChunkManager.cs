#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.API.Forge;
using Welt.Forge;
using Welt.Persistence;
using Welt.Types;

#endregion

namespace Welt.Managers
{

    public class ChunkManager : Dictionary2<IChunk>
    {

        private readonly IChunkPersistence _mPersistence;

        public ChunkManager(IChunkPersistence persistence)
        {
            this._mPersistence = persistence;
        }


        public override void Remove(uint x, uint z)
        {
            var chunk = this[x, z];
            if (chunk == null) return;

            BeforeRemove(chunk);

            IChunk removed;
            TryRemove(KeyFromCoords(x, z), out removed);

        }

        private void BeforeRemove(IChunk chunk)
        {
            _mPersistence.Save(chunk);
        }

        public IChunk Get(Vector3I index)
        {
            return this[index.X, index.Z];
        }

        private IChunk WhenNull(Vector3I index)
        {
            //return persistence.load(index);
            return null;
        }


        public IChunk Load(Vector3I index)
        {
            return _mPersistence.Load(index);
        }


    }
}
