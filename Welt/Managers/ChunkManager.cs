#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge;
using Welt.Persistence;
using Welt.Types;

#endregion

namespace Welt.Managers
{

    public class ChunkManager : Dictionary2<Chunk>
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

            Chunk removed;
            TryRemove(KeyFromCoords(x, z), out removed);

        }

        private void BeforeRemove(Chunk chunk)
        {
            _mPersistence.Save(chunk);
        }

        public Chunk Get(Vector3I index)
        {
            return this[index.X, index.Z];
        }

        /* public override Chunk this[uint x, uint z]
         {
             get
             {
                 Chunk chunk = base[x, z];
                 if (chunk == null)
                 {
                     Vector3i index = new Vector3i(x, 0, z);

                     chunk = whenNull(index);
                     base[x, z] = chunk; 
                 }
                 return chunk;
             }
             set
             {
                 base[x, z] = value;

             }
         }*/

        /*
 * The idea of loading directly whenever accessing a null chunk was cool but theres much more to do in the worldrenderer.generate method
 * 
 * Needs more thinking and surely some major refactoring. 
 * 
 */

        private Chunk WhenNull(Vector3I index)
        {
            //return persistence.load(index);
            return null;
        }


        public Chunk Load(Vector3I index)
        {
            return _mPersistence.Load(index);
        }


    }
}
