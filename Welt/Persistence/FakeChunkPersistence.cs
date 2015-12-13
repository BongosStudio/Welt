#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Welt.Forge;
using Welt.Types;

#endregion

namespace Welt.Persistence
{
    public class MockChunkPersistence : IChunkPersistence
    {

        private readonly World _world;

        public MockChunkPersistence(World world)
        {
            this._world = world;
        }

        public void Save(Chunk chunk)
        {
            //Debug.WriteLine("would be saving " + GetFilename(chunk.Position));
        }

        public Chunk Load(Vector3I index)
        {
            var position = new Vector3I(index.X * Chunk.SIZE.X, index.Y * Chunk.SIZE.Y, index.Z * Chunk.SIZE.Z);

            var filename = GetFilename(position);
            //Debug.WriteLine("Would be loading " + filename);
            return null;
        }

        private string GetFilename(Vector3I position)
        {
            return $"{"LEVELFOLDER"}\\{position.X}-{position.Y}-{position.Z}";
        }
    }
}
