using System.Diagnostics;
using System.Linq;
using System.Threading;
using Welt.Types;

namespace Welt.Forge
{
    public class BlockPalette
    {
        private byte m_NextAvailableSlot = 1;
        private (ushort Id, byte Metadata)[] m_BlockInstances = new (ushort, byte)[64];
        private LightMap m_LightMap;
        private NibbleArray m_Indices;
        private NibbleArray m_R, m_G, m_B, m_S;

        public BlockPalette(int size)
        {
            m_Indices = new NibbleArray(size);
            m_BlockInstances[0] = (0, 0);
            m_R = new NibbleArray(size);
            m_G = new NibbleArray(size);
            m_B = new NibbleArray(size);
            m_S = new NibbleArray(size);
        }

        public Block this[long index]
        {
            get
            {
                return GetBlockData((int) index);
            }
            set
            {
                SetBlockData((int)index, value);
            }
        }

        public ushort GetBlockId(int index)
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

        public void SetBlockId(int index, ushort value)
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

        private bool TryGetIndex((ushort Id, byte Meta) block, out byte index)
        {
            for (byte i = 0; i < m_NextAvailableSlot; ++i)
            {
                var c = m_BlockInstances[i];
                //Debug.WriteLine($"Comparing {block.Id};{block.Meta} to {c.Id};{c.Metadata}");
                if (m_BlockInstances[i].Id == block.Id && m_BlockInstances[i].Metadata == block.Meta)
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

        private Block GetBlockData(int index)
        {
            var data = m_BlockInstances[m_Indices[index]];
            return new Block(data.Id, data.Metadata, m_R[index], m_G[index], m_B[index], m_S[index]);
        }

        private void SetBlockData(int index, Block value)
        {
            if (TryGetIndex((value.Id, value.Metadata), out var i))
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
