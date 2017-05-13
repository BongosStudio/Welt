#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    // TODO: CHANGE TO USE SINGLE DIMENSION ARRAY. Gettin tired of this shit. Idk what even happened.
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

        private BlockDataWrapper[,,] m_Blocks;
        private int m_X;
        private int m_Y;
        private int m_Z;

        public BlockPalette(int width, int height, int depth)
        {
            m_Blocks = new BlockDataWrapper[width, height, depth];
            m_X = width;
            m_Y = height;
            m_Z = depth;
        }

        public Block this[int x, int y, int z]
        {
            get
            {
                return GetBlockData(x, y, z);
            }
            set
            {
                SetBlockData(x, y, z, value);
            }
        }

        public Block this[uint x, uint y, uint z]
        {
            get
            {
                return GetBlockData(x, y, z);
            }
            set
            {
                SetBlockData(x, y, z, value);
            }
        }

        public byte[] ToByteArray()
        {
            var data = new List<byte>(m_Blocks.Length*3);
            var length = m_Blocks.Length;
            data.AddRange(BitConverter.GetBytes(length)); // 4 bytes

            for (var x = 0; x < m_X; x++)
            {
                for (var z = 0; z < m_Z; z++)
                {
                    for (var y = 0; y < m_Y; y++)
                    {
                        var block = m_Blocks[x, y, z];
                        data.AddRange(BitConverter.GetBytes(block.Id));
                        data.Add(block.Metadata);
                    }
                }
            }
            
            return data.ToArray();
        }

        public static BlockPalette FromByteArray(int width, int height, int depth, byte[] data)
        {
            var queue = new Queue<byte>(data);
            var length = new[] { queue.Dequeue(), queue.Dequeue(), queue.Dequeue(), queue.Dequeue() };
            var palette = new BlockPalette(width, height, depth);
            var i = 0;
            for (var x = 0; x < palette.m_X; x++)
            {
                for (var z = 0; z < palette.m_Z; z++)
                {
                    for (var y = 0; y < palette.m_Y; y++)
                    {
                        var id = BitConverter.ToUInt16(new[] { queue.Dequeue(), queue.Dequeue() }, 0);
                        var meta = queue.Dequeue();
                        palette[x, y, z] = new Block(id, meta);
                    }
                }
            }
            
            return palette;
        }

        public ushort GetId(int x, int y, int z)
        {
            return m_Blocks[x, y, z].Id;
        }

        public byte GetBlockMetadata(int x, int y, int z)
        {
            return m_Blocks[x, y, z].Metadata;
        }

        public Vector3B GetBlockLight(int x, int y, int z)
        {
            return new Vector3B();
        }

        public byte GetBlockLightR(int x, int y, int z)
        {
            return 0;
        }

        public byte GetBlockLightG(int x, int y, int zex)
        {
            return 0;
        }

        public byte GetBlockLightB(int x, int y, int z)
        {
            return 0;
        }

        public byte GetSunLight(int x, int y, int z)
        {
            return 0;
        }

        public void SetId(int x, int y, int z, ushort value)
        {
            var block = GetBlockData(x, y, z);
            block.Id = value;
            block.Metadata = 0;
            SetBlockData(x, y, z, block);
        }

        public void SetBlockMeta(int x, int y, int z, byte value)
        {
            var block = GetBlockData(x, y, z);
            block.Metadata = value;
            SetBlockData(x, y, z, block);
        }
        
        private Block GetBlockData(int x, int y, int z)
        {
            var block = m_Blocks[x, y % m_Y, z];
            return new Block(block.Id, block.Metadata);
        }

        private Block GetBlockData(uint x, uint y, uint z)
        {
            var block = m_Blocks[x, y % m_Y, z];
            return new Block(block.Id, block.Metadata);
        }
        
        private void SetBlockData(int x, int y, int z, Block value)
        {
            m_Blocks[x, y % m_Y, z] = new BlockDataWrapper(value.Id, value.Metadata);
            
        }

        private void SetBlockData(uint x, uint y, uint z, Block value)
        {
            m_Blocks[x, y % m_Y, z] = new BlockDataWrapper(value.Id, value.Metadata);

        }

        private int GetIndex(int x, int y, int z)
        {
            return x + m_X * (y + m_Z * z);
        }
    }
}