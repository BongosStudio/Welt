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

        private readonly World _mWorld;

        public MockChunkPersistence(World world)
        {
            this._mWorld = world;
        }

        public void Save(Chunk chunk)
        {
            //Debug.WriteLine("would be saving " + GetFilename(chunk.Position));
        }

        public Chunk Load(Vector3I index)
        {
            var position = new Vector3I(index.X * Chunk.Size.X, index.Y * Chunk.Size.Y, index.Z * Chunk.Size.Z);

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
