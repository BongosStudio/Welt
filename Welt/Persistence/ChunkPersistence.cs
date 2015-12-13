#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.IO;
using Welt.Forge;
using Welt.Models;
using Welt.Types;

#endregion

namespace Welt.Persistence
{
    public class ChunkPersistence : IChunkPersistence
    {
        private const String LEVELFOLDER = "c:\\techcraft";

        private readonly World _world;

        public ChunkPersistence(World world)
        {
            this._world = world;

            if (!Directory.Exists(LEVELFOLDER))
            {
                Directory.CreateDirectory(LEVELFOLDER);
            }
        }

        #region save
        public void Save(Chunk chunk)
        {
            //Debug.WriteLine("saving " + GetFilename(chunk.Position));
            var fs = File.Open(GetFilename(chunk.Position), FileMode.Create);
            var writer = new BinaryWriter(fs);
            Save(chunk, writer);
            writer.Flush();
            writer.Close();
            fs.Close();
        }
        #endregion

        #region load
        public Chunk Load(Vector3I index)
        {
            var position = new Vector3I(index.X * Chunk.SIZE.X, index.Y * Chunk.SIZE.Y, index.Z * Chunk.SIZE.Z);
            var filename = GetFilename(position);

            if (File.Exists(filename))
            {
                //Debug.WriteLine("Loading " + filename);
                var fs = File.Open(filename, FileMode.Open);

                var reader = new BinaryReader(fs);
                var chunk = Load(position, reader);
                reader.Close();
                fs.Close();
                //chunk.generated = true;
                chunk.State = ChunkState.AwaitingBuild;
                return chunk;
            }
            else
            {
                //Debug.WriteLine("New " + filename);
                return null;
            }
        }
        #endregion

        #region Private Save
        private void Save(Chunk chunk, BinaryWriter writer)
        {

            var array = new byte[chunk.Blocks.Length];

            for (var i = 0; i < chunk.Blocks.Length; i++)
            {
                array[i] = (byte)chunk.Blocks[i].Type;
            }
            writer.Write(array);
        }
        #endregion

        #region Private Load
        private Chunk Load(Vector3I worldPosition, BinaryReader reader)
        {
            //index from position
            var index = new Vector3I(worldPosition.X / Chunk.SIZE.X, worldPosition.Y / Chunk.SIZE.Y, worldPosition.Z / Chunk.SIZE.Z);

            var chunk = new Chunk(_world, index);

            var array = reader.ReadBytes(chunk.Blocks.Length);

            for (var i = 0; i < chunk.Blocks.Length; i++)
            {
                chunk.Blocks[i].Type = (BlockType)array[i];
            }

            return chunk;
        }
        #endregion

        private string GetFilename(Vector3I position)
        {
            return string.Format("{0}\\{1}-{2}-{3}", LEVELFOLDER, position.X, position.Y, position.Z);
        }
    }
}
