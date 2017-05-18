using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Welt.API.Entities;
using Welt.API.Forge;
using Welt.Core.Extensions;
using Welt.Core.Forge;
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

        public static bool IsEntityInWater(this IEntity entity)
        {
            return entity.World.GetBlock(entity.Position).Id == BlockType.WATER;
        }

        public static bool IsPlayerInWater(this MultiplayerClient player)
        {
            return player.World.GetBlock(player.Position).Id == BlockType.WATER ||
                player.World.GetBlock(player.Position + Vector3.UnitY).Id == BlockType.WATER;
        }

        public static float GetGravity(this ReadOnlyWorld world)
        {
            return world.World.GetGravity();
        }

        public static IBlockProvider GetBlockProvider(this ReadOnlyWorld world, Vector3 position)
        {
            return BlockProvider.BlockRepository.GetBlockProvider(world.GetBlock(position).Id);
        }
    }
}
