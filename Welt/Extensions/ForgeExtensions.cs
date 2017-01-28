using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Forge;

namespace Welt.Extensions
{
    public static class ForgeExtensions
    {
        public static bool DoesBlockExistIn(this World world, ushort id, Vector3 bl, Vector3 tr, out Vector3 position)
        {
            for (var x = bl.X; x <= tr.X; ++x)
            {
                for (var z = bl.Z; z <= tr.Z; ++z)
                {
                    for (var y = bl.Y; y <= tr.Y; ++y)
                    {
                        var p = new Vector3(x, y, z);
                        if (world.GetBlock(p).Id == id)
                        {
                            position = p;
                            return true;
                        }
                    }
                }
            }
            position = Vector3.Zero;
            return false;
        }

        public static bool IsBlockAround(this World world, ushort id, Vector3 center, int xr, int yr, int zr, out Vector3 position)
        {
            for (var x = 0; x < xr; ++x)
            {
                for (var z = 0; z < zr; ++z)
                {
                    for (var y = 0; y < yr; ++y)
                    {
                        if (world.GetBlock(center + new Vector3(x, y, z)).Id == id)
                        {
                            position = center + new Vector3(x, y, z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(-x, y, z)).Id == id)
                        {
                            position = center + new Vector3(-x, y, z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(x, -y, z)).Id == id)
                        {
                            position = center + new Vector3(x, -y, z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(x, y, -z)).Id == id)
                        {
                            position = center + new Vector3(x, y, -z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(-x, -y, z)).Id == id)
                        {
                            position = center + new Vector3(-x, -y, z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(-x, y, -z)).Id == id)
                        {
                            position = center + new Vector3(-x, y, -z);
                            return true;
                        }
                        if (world.GetBlock(center + new Vector3(x, -y, -z)).Id == id)
                        {
                            position = center + new Vector3(x, -y, -z);
                            return true;
                        }
                    }
                }
            }
            position = Vector3.Zero;
            return false;
        }

        public static ushort[] GetBlocksAround(this World world, Vector3 bl, Vector3 tr)
        {
            var blocks = new List<ushort>();
            for (var x = bl.X; x <= tr.X; ++x)
            {
                for (var z = bl.Z; z <= tr.Z; ++z)
                {
                    for (var y = bl.Y; y <= tr.Y; ++y)
                    {
                        blocks.Add(world.GetBlock(new Vector3(x, y, z)).Id);
                    }
                }
            }
            return blocks.ToArray();
        }

        public static void DrawPrimaryBuffer(this Chunk chunk, GraphicsDevice graphics)
        {
            if (chunk.PrimaryVertexCount == 0 || chunk.PrimaryVertexBuffer == null || chunk.PrimaryIndexBuffer == null)
                return;
            graphics.SetVertexBuffer(chunk.PrimaryVertexBuffer);
            graphics.Indices = chunk.PrimaryIndexBuffer;
            graphics?.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.PrimaryVertexCount);
        }

        public static void DrawSecondaryBuffer(this Chunk chunk, GraphicsDevice graphics)
        {
            if (chunk.SecondaryVertexCount == 0 || chunk.SecondaryVertexBuffer == null || chunk.SecondaryIndexBuffer == null)
                return;
            graphics.SetVertexBuffer(chunk.SecondaryVertexBuffer);
            graphics.Indices = chunk.SecondaryIndexBuffer;
            graphics?.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, chunk.SecondaryVertexCount);
        }
    }
}
