#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    /// <summary>
    ///     Contains all block information within the assigned chunk.
    /// </summary>
    public class BlockPalette
    {
        private struct BlockDataWrapper
        {
            public ushort Id;
            public byte Metadata;

            public BlockDataWrapper(ushort id, byte metadata)
            {
                Id = id;
                Metadata = metadata;
            }
        }

        private byte m_NextAvailableSlot = 1;
        private BlockDataWrapper[] m_BlockInstances = new BlockDataWrapper[64];
        private byte[] m_Indices;
        private byte[] m_R, m_G, m_B, m_S;

        public BlockPalette(int size)
        {
            m_Indices = new byte[size];
            m_BlockInstances[0] = new BlockDataWrapper(0, 0);
            m_R = new byte[size];
            m_G = new byte[size];
            m_B = new byte[size];
            m_S = new byte[size];
        }

        public Block this[long index]
        {
            get
            {
                return GetBlockData((int)index);
            }
            set
            {
                SetBlockData((int)index, value);
            }
        }

        public ushort GetId(int index)
        {
            return m_BlockInstances[m_Indices[index]].Id;
        }

        public byte GetBlockMetadata(int index)
        {
            return m_BlockInstances[m_Indices[index]].Metadata;
        }

        public Vector3B GetBlockLight(int index)
        {
            return new Vector3B(m_R[index], m_G[index], m_B[index]);
        }

        public byte GetBlockLightR(int index)
        {
            return m_R[index];
        }

        public byte GetBlockLightG(int index)
        {
            return m_G[index];
        }

        public byte GetBlockLightB(int index)
        {
            return m_B[index];
        }

        public byte GetSunLight(int index)
        {
            return m_S[index];
        }

        public void SetId(int index, ushort value)
        {

        }

        public void SetBlockMeta(int index, byte value)
        {

        }

        public void SetBlockSun(int index, byte value)
        {
            m_S[index] = value;
        }

        public void SetBlockLight(int index, Vector3B value)
        {
            m_R[index] = value.X;
            m_G[index] = value.Y;
            m_B[index] = value.Z;
        }

        public void SetRLight(int index, byte value)
        {
            m_R[index] = value;
        }

        public void SetGLight(int index, byte value)
        {
            m_G[index] = value;
        }

        public void SetBLight(int index, byte value)
        {
            m_B[index] = value;
        }

        private bool TryGetIndex(BlockDataWrapper block, out byte index)
        {
            for (byte i = 0; i < m_NextAvailableSlot; ++i)
            {
                var c = m_BlockInstances[i];
                //Debug.WriteLine($"Comparing {block.Id};{block.Meta} to {c.Id};{c.Metadata}");
                if (m_BlockInstances[i].Id == block.Id && m_BlockInstances[i].Metadata == block.Metadata)
                {
                    //Debug.WriteLine($"Match found at {i}");
                    index = i;
                    return true;
                }
                //Thread.Sleep(250);
            }
            // no blocks were found. Create new one.
            //Debug.WriteLine($"No match found. Checking for space. Available index is {m_NextAvailableSlot}");
            index = m_NextAvailableSlot;
            if (index >= 64)
            {
                // no space. Abort.
                //Debug.WriteLine($"Index unacceptable: {index}");
                index = 0;
                return false;
            }
            m_BlockInstances[index] = block;
            m_NextAvailableSlot++;
            //Debug.WriteLine($"Index is acceptable.");
            return true;
        }

        private int GetIndex(int index)
        {
            return m_Indices[index];
        }

        private Block GetBlockData(int index)
        {
            var data = m_BlockInstances[GetIndex(index)];
            var result = new Block(data.Id, data.Metadata, m_R[index], m_G[index], m_B[index], m_S[index]);
            return result;
        }

        private void SetBlockData(int index, Block value)
        {
            if (TryGetIndex(new BlockDataWrapper(value.Id, value.Metadata), out var i))
            {
                // i is the index within the indices where we have our USHORT/BYTE instance.
                // first we set the index data
                m_Indices[index] = i;
                m_R[index] = value.R;
                m_G[index] = value.G;
                m_B[index] = value.B;
                m_S[index] = value.Sun;
            }
            else
            {
                //Debug.WriteLine("Could not set block data");
            }
        }
    }
}