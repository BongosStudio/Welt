#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Blocks;
using Welt.Forge;
using Welt.Types;
using Welt.Processors.MeshBuilders;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

// ReSharper disable PossibleLossOfFraction

#endregion

namespace Welt.Processors
{
    public delegate void VertexBuilder(ushort id, Chunk chunk, Vector3I relativePosition);

    public class VertexBuildChunkProcessor : IChunkProcessor
    {
        private readonly GraphicsDevice m_GraphicsDevice;
        private const int MAX_SUN_VALUE = 16;

        public VertexBuildChunkProcessor(GraphicsDevice graphicsDevice)
        {
            m_GraphicsDevice = graphicsDevice;
        }

        #region BuildVertexList

        private void BuildVertexList(Chunk chunk)
        {
            //lowestNoneBlock and highestNoneBlock come from the terrain gen (Eventually, if the terraingen did not set them you gain nothing)
            //and digging is handled correctly too 
            //TODO generalize highest/lowest None to non-solid

            var yLow = (chunk.LowestNoneBlock.Y == 0 ? 0 : chunk.LowestNoneBlock.Y - 1);
            var yHigh =
                (chunk.HighestSolidBlock.Y == Chunk.Size.Y ? Chunk.Size.Y : chunk.HighestSolidBlock.Y + 1);

            for (byte x = 0; x < Chunk.Size.X; x++)
            {
                for (byte z = 0; z < Chunk.Size.Z; z++)
                {
                    var offset = x*Chunk.FlattenOffset + z*Chunk.Size.Y;
                        // we don't want this x-z value to be calculated each in in y-loop!

                    #region ylow and yhigh on chunk borders

                    if (x == 0)
                    {
                        if (chunk.E == null)
                        {
                            yHigh = Chunk.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.E.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.E.LowestNoneBlock.Y);
                        }
                    }
                    else if (x == Chunk.Max.X)
                    {
                        if (chunk.W == null)
                        {
                            yHigh = Chunk.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.W.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.W.LowestNoneBlock.Y);
                        }
                    }

                    if (z == 0)
                    {
                        if (chunk.S == null)
                        {
                            yHigh = Chunk.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.S.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.S.LowestNoneBlock.Y);
                        }
                    }
                    else if (z == Chunk.Max.Z)
                    {
                        if (chunk.N == null)
                        {
                            yHigh = Chunk.Size.Y;
                            yLow = 0;
                        }
                        else
                        {
                            yHigh = Math.Max(yHigh, chunk.N.HighestSolidBlock.Y);
                            yLow = Math.Min(yLow, chunk.N.LowestNoneBlock.Y);
                        }
                    }

                    #endregion

                    for (byte y = (byte) yLow; y < yHigh; y++)
                    {
                        var id = chunk.Blocks[offset + y].Id;
                        if (id == BlockType.NONE) continue;
                        var builder = BlockMeshBuilder.GetVertexBuilder(id);
                        builder(id, chunk, new Vector3I(x, y, z));
                    }
                }
            }

            var start = DateTime.Now;

            var pvwr = new WeakReference<VertexPositionTextureLightEffect[]>(chunk.PrimaryVertexList.ToArray());
            var pi = chunk.PrimaryIndexList.ToArray();
            var svwr = new WeakReference<VertexPositionTextureLightEffect[]>(chunk.SecondaryVertexList.ToArray());
            var si = chunk.SecondaryIndexList.ToArray();
            if (pvwr.TryGetTarget(out var pv))
            {
                if (pv.Length > 0)
                {
                    chunk.PrimaryVertexBuffer = new VertexBuffer(m_GraphicsDevice, typeof(VertexPositionTextureLightEffect), pv.Length,
                        BufferUsage.WriteOnly);
                    chunk.PrimaryVertexBuffer.SetData(pv);
                    chunk.PrimaryIndexBuffer = new IndexBuffer(m_GraphicsDevice, IndexElementSize.SixteenBits, pi.Length,
                        BufferUsage.WriteOnly);
                    chunk.PrimaryIndexBuffer.SetData(pi);
                }
            }
            if (svwr.TryGetTarget(out var sv))
            {
                if (sv.Length > 0)
                {
                    chunk.SecondaryVertexBuffer = new VertexBuffer(m_GraphicsDevice, typeof(VertexPositionTextureLightEffect), sv.Length,
                        BufferUsage.WriteOnly);

                    chunk.SecondaryVertexBuffer.SetData(sv);
                    chunk.SecondaryIndexBuffer = new IndexBuffer(m_GraphicsDevice, IndexElementSize.SixteenBits, si.Length,
                        BufferUsage.WriteOnly);
                    chunk.SecondaryIndexBuffer.SetData(si);
                }
            }
            chunk.PrimaryVertexList.Clear();
            chunk.SecondaryVertexList.Clear();
            chunk.PrimaryIndexList.Clear();
            chunk.SecondaryIndexList.Clear();
            //GC.Collect();
        }

        #endregion

        
        public void ProcessChunk(Chunk chunk)
        {
            if (chunk == null) return;
            
            lock (chunk)
            {
                chunk.Clear();
            }
            BuildVertexList(chunk);
            chunk.State = Models.ChunkState.Ready;
        }
    }
}
