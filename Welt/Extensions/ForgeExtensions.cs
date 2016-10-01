using Microsoft.Xna.Framework;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using Welt.API.Forge;
using Welt.Core;
using Welt.Core.Forge;
using Welt.Core.Services;
using Welt.Forge.Builders;

namespace Welt.Game.Extensions
{
    public static class ForgeExtensions
    {
        public static bool IsBlockOpen(ushort id)
        {
            return id == 0 || BlockService.GetOpaque(id, 0);
        }

        public static bool IsBlockOpen(this ChunkBuilder builder, int x, int y, int z)
        {
            return !builder.SolidBlockMap[x*Chunk.Width*Chunk.Depth + z*Chunk.Depth + y];
        }

        public static int GetHighestPointInChunk(this Chunk chunk)
        {
            return chunk.HeightMap.Max();
        }

        public static BitArray CreatSolidMapFromChunk(this Chunk chunk)
        {
            var bitArray = new BitArray(Chunk.Width*Chunk.Depth*chunk.Height);

            for (var x = 0; x < Chunk.Width; x++)
            {
                for (var z = 0; z < Chunk.Depth; z++)
                {
                    for (var y = 0; y < chunk.GetHeight(x, z); y++)
                    {
                        bitArray[ConvertToIndex(new Vector3(x, y, z), Chunk.Width, Chunk.Depth)] =
                            !IsBlockOpen(chunk.GetBlock(x, y, z).Id);
                    }
                }
            }

            return bitArray;
        }

        public static int ConvertToIndex(Vector3 position, int width, int depth)
        {
            return (int) (position.X*(depth*16) + position.Z*16 + position.Y);
        }

        public static int ConvertToIndex(int x, int y, int z, int width, int depth)
        {
            FastMath.Adjust(0, 15, ref x);
            FastMath.Adjust(0, 15, ref z);
            FastMath.Adjust(0, 512, ref y);
            return x*depth*16 + z*16 + y;
        }

        public static bool HasTopFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.YIncreasing)) != 0;
        }

        public static bool HasBottomFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.YDecreasing)) != 0;
        }

        public static bool HasLeftFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.XDecreasing)) != 0;
        }

        public static bool HasRightFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.XIncreasing)) != 0;
        }

        public static bool HasFrontFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.ZIncreasing)) != 0;
        }

        public static bool HasBackFace(this BlockFaceDirection direction)
        {
            return ((byte) direction & (1 << (byte) BlockFaceDirection.ZDecreasing)) != 0;
        }

        public static (ushort Id, byte Metadata) GetBlock(this IWorld world, Vector3 position)
        {
            return world.GetBlock((uint) position.X, (uint) position.Y, (uint) position.Z);
        }
    }
}