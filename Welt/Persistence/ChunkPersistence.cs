#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System.IO;
using Welt.API.Forge;
using Welt.Forge;
using Welt.IO;
using Welt.Types;

namespace Welt.Persistence
{
    public class ChunkPersistence : IChunkPersistence
    {
        /*

        ChunkObject persistence follows this format:
        FileName: c_x_z.w (ie: c_1_12.w)
        Directory: world_name/chunks/ (ie: myworld/chunks/)

        Data Format follows:
        BYTE highestpoint (this is to make sure we don't have to go through an entire 'for' loop)
        BYTE[] blockdata
            The byte array is formatted per-block as:
            BYTE[2] blocktype (ushort value of the block)
            BYTE    block metadata
        BYTE[] entitydata
            The byte array is formatted per-entity as:
            BYTE[2] entitytype (ushort value of the entity-type)
            BYTE[2] entityID (ushort value of the entity ID)
            DATAMAP entity metadata
            BYTE[2] entity position (first byte: leading 4 bits are X, trailing 4 bits are Z | second byte: Y)

        */
        public void Save(IChunk chunk)
        {
            File.WriteAllBytes("testchunk.w", new WireChunk(chunk).ToArray());
        }

        public IChunk Load(Vector3I index)
        {
            throw new System.NotImplementedException();
        }
    }
}