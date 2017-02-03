#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using Welt.API;
using Welt.API.IO;
using Welt.Core.Forge;

namespace Welt.Core.Persistence
{
    public class ChunkPersistence : IChunkPersistence
    {
        /*

        Chunk persistence follows this format:
        FileName: c_x_z.w (ie: c_1_12.w)
        Directory: world_name/chunks/ (ie: myworld/chunks/)

        Data Format follows:
        BYTE highestpoint (this is to make sure we don't have to go through an entire 'for' loop)
        BYTE[] blockdata
            The byte array is formatted per-block as:
            BYTE[2] blocktype (ushort value of the block)
        BYTE[] entitydata
            The byte array is formatted per-entity as:
            BYTE[2] entitytype (ushort value of the entity-type)
            BYTE[2] entityID (ushort value of the entity ID)
            DATAMAP entity metadata
            BYTE[2] entity position (first byte: leading 4 bits are X, trailing 4 bits are Z | second byte: Y)

        */
        public void Save(Chunk chunk)
        {
            
        }

        public Chunk Load(Vector3I index)
        {
            return null;
        }
    }
}