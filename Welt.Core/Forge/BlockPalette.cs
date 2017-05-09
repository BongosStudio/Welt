#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
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

        private BlockDataWrapper[] m_Blocks;
        
        private byte[] m_R, m_G, m_B, m_S;

        public BlockPalette(int size)
        {
            m_Blocks = new BlockDataWrapper[size];
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

        public byte[] ToByteArray()
        {
            var data = new List<byte>(m_Blocks.Length*3);
            var length = m_Blocks.Length;
            data.AddRange(BitConverter.GetBytes(length)); // 4 bytes

            for (var i = 0; i < length; i++)
            {
                var block = m_Blocks[i];
                data.AddRange(BitConverter.GetBytes(block.Id));
                data.Add(block.Metadata);
            }

            return data.ToArray();
        }

        public static BlockPalette FromByteArray(int size, byte[] data)
        {
            var queue = new Queue<byte>(data);
            var length = new[] { queue.Dequeue(), queue.Dequeue(), queue.Dequeue(), queue.Dequeue() };
            var blocks = new BlockDataWrapper[BitConverter.ToInt32(length, 0)];
            var i = 0;
            while (queue.Count > 0)
            {
                var id = BitConverter.ToUInt16(new[] { queue.Dequeue(), queue.Dequeue() }, 0);
                var meta = queue.Dequeue();
                blocks[i] = new BlockDataWrapper(id, meta);
                i++;
            }
            return new BlockPalette(size) { m_Blocks = blocks };
        }

        public ushort GetId(int index)
        {
            return m_Blocks[index].Id;
        }

        public byte GetBlockMetadata(int index)
        {
            return m_Blocks[index].Metadata;
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
            var block = GetBlockData(index);
            block.Id = value;
            block.Metadata = 0;
            SetBlockData(index, block);
        }

        public void SetBlockMeta(int index, byte value)
        {
            var block = GetBlockData(index);
            block.Metadata = value;
            SetBlockData(index, block);
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
        
        private Block GetBlockData(int index)
        {
            var block = m_Blocks[index];
            return new Block(block.Id, block.Metadata);
        }

        private void SetBlockData(int index, Block value)
        {
            m_Blocks[index] = new BlockDataWrapper(value.Id, value.Metadata);
            m_R[index] = value.R;
            m_G[index] = value.G;
            m_B[index] = value.B;
            m_S[index] = value.Sun;
        }
    }
}