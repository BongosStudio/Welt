using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.API;
using Welt.Core.Forge;
using Welt.Core.Blocks;

namespace Welt.Processors.MeshBuilders
{
    public class PlantBuilder : BlockMeshBuilder
    {
        public const int VertexCount = 16;

        public static void BuildPlantVertexList(ushort id, ReadOnlyChunk chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte)chunkRelativePosition.X;
            var y = (sbyte)chunkRelativePosition.Y;
            var z = (sbyte)chunkRelativePosition.Z;

            var blockTopNw = chunk.GetBlockLight(x - 1, y + 1, z + 1);
            var blockTopN = chunk.GetBlockLight(x, y + 1, z + 1);
            var blockTopNe = chunk.GetBlockLight(x + 1, y + 1, z + 1);
            var blockTopW = chunk.GetBlockLight(x - 1, y + 1, z);
            var blockTopM = chunk.GetBlockLight(x, y + 1, z);
            var blockTopE = chunk.GetBlockLight(x + 1, y + 1, z);
            var blockTopSw = chunk.GetBlockLight(x - 1, y + 1, z - 1);
            var blockTopS = chunk.GetBlockLight(x, y + 1, z - 1);
            var blockTopSe = chunk.GetBlockLight(x + 1, y + 1, z - 1);

            var blockMidNw = chunk.GetBlockLight(x - 1, y, z + 1);
            var blockMidN = chunk.GetBlockLight(x, y, z + 1);
            var blockMidNe = chunk.GetBlockLight(x + 1, y, z + 1);
            var blockMidW = chunk.GetBlockLight(x - 1, y, z);
            var blockMidM = chunk.GetBlockLight(x, y, z);
            var blockMidE = chunk.GetBlockLight(x + 1, y, z);
            var blockMidSw = chunk.GetBlockLight(x - 1, y, z - 1);
            var blockMidS = chunk.GetBlockLight(x, y, z - 1);
            var blockMidSe = chunk.GetBlockLight(x + 1, y, z - 1);

            var blockBotNw = chunk.GetBlockLight(x - 1, y - 1, z + 1);
            var blockBotN = chunk.GetBlockLight(x, y - 1, z + 1);
            var blockBotNe = chunk.GetBlockLight(x + 1, y - 1, z + 1);
            var blockBotW = chunk.GetBlockLight(x - 1, y - 1, z);
            var blockBotM = chunk.GetBlockLight(x, y - 1, z);
            var blockBotE = chunk.GetBlockLight(x + 1, y - 1, z);
            var blockBotSw = chunk.GetBlockLight(x - 1, y - 1, z - 1);
            var blockBotS = chunk.GetBlockLight(x, y - 1, z - 1);
            var blockBotSe = chunk.GetBlockLight(x + 1, y - 1, z - 1);

            var local = Vector3B.Average(blockTopNw, blockTopN, blockTopNe, blockTopW, blockTopM, blockTopE,
                blockTopSw, blockTopS, blockTopSe, blockMidNw, blockMidN, blockMidNe, blockMidW, blockMidM,
                blockMidE, blockMidSw, blockMidS, blockMidSe, blockBotNw, blockBotN, blockBotNe, blockBotW,
                blockBotM, blockBotE, blockBotSw, blockBotS, blockBotSe);

            BuildPlantVertices(chunk, blockPosition, chunkRelativePosition, id, 0.6f, (new Color(0.5f + local.X/255, 0.5f + local.Y/255, 0.5f + local.Z/255)));
        }

        protected static void BuildPlantVertices(ReadOnlyChunk chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            ushort blockType, float sunLight, Color localLight)
        {
            var provider = BlockProvider.GetProvider(blockType);
            var uvList = provider.GetTexture(BlockFaceDirection.XIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.5f, 1, 1), new Vector3(0.5f, 1, 0), new Vector3(0.5f, 0, 1), new Vector3(0.5f, 0, 0) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = provider.GetTexture(BlockFaceDirection.XDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.5f, 1, 0), new Vector3(0.5f, 1, 1), new Vector3(0.5f, 0, 0), new Vector3(0.5f, 0, 1) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.ZIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0, 1, 0.5f), new Vector3(1, 1, 0.5f), new Vector3(0, 0, 0.5f), new Vector3(1, 0, 0.5f) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.ZDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(1, 1, 0.5f), new Vector3(0, 1, 0.5f), new Vector3(1, 0, 0.5f), new Vector3(0, 0, 0.5f) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });
        }
    }
}
