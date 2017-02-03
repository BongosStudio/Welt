using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.API;
using Welt.Core.Forge;
using System.Collections.Generic;
using Welt.Graphics;
using Welt.API.Forge;
using Welt.Forge.Renderers;

namespace Welt.Processors.MeshBuilders
{
    public class UniformCubeBuilder : BlockMeshBuilder
    {
        private static Vector3[] m_AddVectors = new[]
        {
            new Vector3(0, 0, 0), //0
            new Vector3(0, 0, 1), //1
            new Vector3(0, 1, 0), //2
            new Vector3(1, 0, 0), //3
            new Vector3(0, 1, 1), //4
            new Vector3(1, 0, 1), //5
            new Vector3(1, 1, 0), //6
            new Vector3(1, 1, 1), //7
        };

        public static void BuildBlockVertexList(IBlockProvider provider, ReadOnlyChunk chunk, 
            Vector3I chunkRelativePosition, ChunkRenderer.VisibleFaces faces, int vertexCount, 
            ref List<VertexPositionTextureLightEffect> vertices, ref List<short> indices)
        {
            if (provider.Id == 0) return;
            var blockPosition = chunk.GetPosition() + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte)chunkRelativePosition.X;
            var y = (sbyte)chunkRelativePosition.Y;
            var z = (sbyte)chunkRelativePosition.Z;

            var blockTopNw = chunk.GetBlock(x - 1, y + 1, z + 1);
            var blockTopN = chunk.GetBlock(x, y + 1, z + 1);
            var blockTopNe = chunk.GetBlock(x + 1, y + 1, z + 1);
            var blockTopW = chunk.GetBlock(x - 1, y + 1, z);
            var blockTopM = chunk.GetBlock(x, y + 1, z);
            var blockTopE = chunk.GetBlock(x + 1, y + 1, z);
            var blockTopSw = chunk.GetBlock(x - 1, y + 1, z - 1);
            var blockTopS = chunk.GetBlock(x, y + 1, z - 1);
            var blockTopSe = chunk.GetBlock(x + 1, y + 1, z - 1);

            var blockMidNw = chunk.GetBlock(x - 1, y, z + 1);
            var blockMidN = chunk.GetBlock(x, y, z + 1);
            var blockMidNe = chunk.GetBlock(x + 1, y, z + 1);
            var blockMidW = chunk.GetBlock(x - 1, y, z);
            var blockMidM = chunk.GetBlock(x, y, z);
            var blockMidE = chunk.GetBlock(x + 1, y, z);
            var blockMidSw = chunk.GetBlock(x - 1, y, z - 1);
            var blockMidS = chunk.GetBlock(x, y, z - 1);
            var blockMidSe = chunk.GetBlock(x + 1, y, z - 1);

            var blockBotNw = chunk.GetBlock(x - 1, y - 1, z + 1);
            var blockBotN = chunk.GetBlock(x, y - 1, z + 1);
            var blockBotNe = chunk.GetBlock(x + 1, y - 1, z + 1);
            var blockBotW = chunk.GetBlock(x - 1, y - 1, z);
            var blockBotM = chunk.GetBlock(x, y - 1, z);
            var blockBotE = chunk.GetBlock(x + 1, y - 1, z);
            var blockBotSw = chunk.GetBlock(x - 1, y - 1, z - 1);
            var blockBotS = chunk.GetBlock(x, y - 1, z - 1);
            var blockBotSe = chunk.GetBlock(x + 1, y - 1, z - 1);

            float sunTr, sunTl, sunBr, sunBl;
            float redTr, redTl, redBr, redBl;
            float grnTr, grnTl, grnBr, grnBl;
            float bluTr, bluTl, bluBr, bluBl;
            Color localTr, localTl, localBr, localBl;


            // XDecreasing
            if ((faces & ChunkRenderer.VisibleFaces.West) != 0)
            {
                sunTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.Sun + blockTopW.Sun + blockMidNw.Sun + blockMidW.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockTopSw.Sun + blockTopW.Sun + blockMidSw.Sun + blockMidW.Sun) / 4);
                sunBl = (1f / MAX_SUN_VALUE) * ((blockBotNw.Sun + blockBotW.Sun + blockMidNw.Sun + blockMidW.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockBotSw.Sun + blockBotW.Sun + blockMidSw.Sun + blockMidW.Sun) / 4);

                redTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.R + blockTopW.R + blockMidNw.R + blockMidW.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockTopSw.R + blockTopW.R + blockMidSw.R + blockMidW.R) / 4);
                redBl = (1f / MAX_SUN_VALUE) * ((blockBotNw.R + blockBotW.R + blockMidNw.R + blockMidW.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockBotSw.R + blockBotW.R + blockMidSw.R + blockMidW.R) / 4);

                grnTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.G + blockTopW.G + blockMidNw.G + blockMidW.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockTopSw.G + blockTopW.G + blockMidSw.G + blockMidW.G) / 4);
                grnBl = (1f / MAX_SUN_VALUE) * ((blockBotNw.G + blockBotW.G + blockMidNw.G + blockMidW.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockBotSw.G + blockBotW.G + blockMidSw.G + blockMidW.G) / 4);

                bluTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.B + blockTopW.B + blockMidNw.B + blockMidW.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockTopSw.B + blockTopW.B + blockMidSw.B + blockMidW.B) / 4);
                bluBl = (1f / MAX_SUN_VALUE) * ((blockBotNw.B + blockBotW.B + blockMidNw.B + blockMidW.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockBotSw.B + blockBotW.B + blockMidSw.B + blockMidW.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.East) != 0)
            {
                sunTl = (1f / MAX_SUN_VALUE) * ((blockTopSe.Sun + blockTopE.Sun + blockMidSe.Sun + blockMidE.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.Sun + blockTopE.Sun + blockMidNe.Sun + blockMidE.Sun) / 4);
                sunBl = (1f / MAX_SUN_VALUE) * ((blockBotSe.Sun + blockBotE.Sun + blockMidSe.Sun + blockMidE.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockBotNe.Sun + blockBotE.Sun + blockMidNe.Sun + blockMidE.Sun) / 4);

                redTl = (1f / MAX_SUN_VALUE) * ((blockTopSe.R + blockTopE.R + blockMidSe.R + blockMidE.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.R + blockTopE.R + blockMidNe.R + blockMidE.R) / 4);
                redBl = (1f / MAX_SUN_VALUE) * ((blockBotSe.R + blockBotE.R + blockMidSe.R + blockMidE.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockBotNe.R + blockBotE.R + blockMidNe.R + blockMidE.R) / 4);

                grnTl = (1f / MAX_SUN_VALUE) * ((blockTopSe.G + blockTopE.G + blockMidSe.G + blockMidE.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.G + blockTopE.G + blockMidNe.G + blockMidE.G) / 4);
                grnBl = (1f / MAX_SUN_VALUE) * ((blockBotSe.G + blockBotE.G + blockMidSe.G + blockMidE.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockBotNe.G + blockBotE.G + blockMidNe.G + blockMidE.G) / 4);

                bluTl = (1f / MAX_SUN_VALUE) * ((blockTopSe.B + blockTopE.B + blockMidSe.B + blockMidE.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.B + blockTopE.B + blockMidNe.B + blockMidE.B) / 4);
                bluBl = (1f / MAX_SUN_VALUE) * ((blockBotSe.B + blockBotE.B + blockMidSe.B + blockMidE.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockBotNe.B + blockBotE.B + blockMidNe.B + blockMidE.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.Bottom) != 0)
            {
                sunBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.Sun + blockBotS.Sun + blockBotM.Sun + blockTopW.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.Sun + blockBotS.Sun + blockBotM.Sun + blockTopE.Sun) / 4);
                sunTl = (1f / MAX_SUN_VALUE) * ((blockBotNw.Sun + blockBotN.Sun + blockBotM.Sun + blockTopW.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockBotNe.Sun + blockBotN.Sun + blockBotM.Sun + blockTopE.Sun) / 4);

                redBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.R + blockBotS.R + blockBotM.R + blockTopW.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.R + blockBotS.R + blockBotM.R + blockTopE.R) / 4);
                redTl = (1f / MAX_SUN_VALUE) * ((blockBotNw.R + blockBotN.R + blockBotM.R + blockTopW.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockBotNe.R + blockBotN.R + blockBotM.R + blockTopE.R) / 4);

                grnBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.G + blockBotS.G + blockBotM.G + blockTopW.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.G + blockBotS.G + blockBotM.G + blockTopE.G) / 4);
                grnTl = (1f / MAX_SUN_VALUE) * ((blockBotNw.G + blockBotN.G + blockBotM.G + blockTopW.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockBotNe.G + blockBotN.G + blockBotM.G + blockTopE.G) / 4);

                bluBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.B + blockBotS.B + blockBotM.B + blockTopW.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.B + blockBotS.B + blockBotM.B + blockTopE.B) / 4);
                bluTl = (1f / MAX_SUN_VALUE) * ((blockBotNw.B + blockBotN.B + blockBotM.B + blockTopW.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockBotNe.B + blockBotN.B + blockBotM.B + blockTopE.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YDecreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.Top) != 0)
            {
                sunTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.Sun + blockTopN.Sun + blockTopW.Sun + blockTopM.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.Sun + blockTopN.Sun + blockTopE.Sun + blockTopM.Sun) / 4);
                sunBl = (1f / MAX_SUN_VALUE) * ((blockTopSw.Sun + blockTopS.Sun + blockTopW.Sun + blockTopM.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockTopSe.Sun + blockTopS.Sun + blockTopE.Sun + blockTopM.Sun) / 4);

                redTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.R + blockTopN.R + blockTopW.R + blockTopM.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.R + blockTopN.R + blockTopE.R + blockTopM.R) / 4);
                redBl = (1f / MAX_SUN_VALUE) * ((blockTopSw.R + blockTopS.R + blockTopW.R + blockTopM.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockTopSe.R + blockTopS.R + blockTopE.R + blockTopM.R) / 4);

                grnTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.G + blockTopN.G + blockTopW.G + blockTopM.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.G + blockTopN.G + blockTopE.G + blockTopM.G) / 4);
                grnBl = (1f / MAX_SUN_VALUE) * ((blockTopSw.G + blockTopS.G + blockTopW.G + blockTopM.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockTopSe.G + blockTopS.G + blockTopE.G + blockTopM.G) / 4);

                bluTl = (1f / MAX_SUN_VALUE) * ((blockTopNw.B + blockTopN.B + blockTopW.B + blockTopM.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockTopNe.B + blockTopN.B + blockTopE.B + blockTopM.B) / 4);
                bluBl = (1f / MAX_SUN_VALUE) * ((blockTopSw.B + blockTopS.B + blockTopW.B + blockTopM.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockTopSe.B + blockTopS.B + blockTopE.B + blockTopM.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YIncreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.South) != 0)
            {
                sunTl = (1f / MAX_SUN_VALUE) * ((blockTopSw.Sun + blockTopS.Sun + blockMidSw.Sun + blockMidS.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockTopSe.Sun + blockTopS.Sun + blockMidSe.Sun + blockMidS.Sun) / 4);
                sunBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.Sun + blockBotS.Sun + blockMidSw.Sun + blockMidS.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.Sun + blockBotS.Sun + blockMidSe.Sun + blockMidS.Sun) / 4);

                redTl = (1f / MAX_SUN_VALUE) * ((blockTopSw.R + blockTopS.R + blockMidSw.R + blockMidS.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockTopSe.R + blockTopS.R + blockMidSe.R + blockMidS.R) / 4);
                redBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.R + blockBotS.R + blockMidSw.R + blockMidS.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.R + blockBotS.R + blockMidSe.R + blockMidS.R) / 4);

                grnTl = (1f / MAX_SUN_VALUE) * ((blockTopSw.G + blockTopS.G + blockMidSw.G + blockMidS.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockTopSe.G + blockTopS.G + blockMidSe.G + blockMidS.G) / 4);
                grnBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.G + blockBotS.G + blockMidSw.G + blockMidS.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.G + blockBotS.G + blockMidSe.G + blockMidS.G) / 4);

                bluTl = (1f / MAX_SUN_VALUE) * ((blockTopSw.B + blockTopS.B + blockMidSw.B + blockMidS.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockTopSe.B + blockTopS.B + blockMidSe.B + blockMidS.B) / 4);
                bluBl = (1f / MAX_SUN_VALUE) * ((blockBotSw.B + blockBotS.B + blockMidSw.B + blockMidS.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockBotSe.B + blockBotS.B + blockMidSe.B + blockMidS.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.North) != 0)
            {
                sunTl = (1f / MAX_SUN_VALUE) * ((blockTopNe.Sun + blockTopN.Sun + blockMidNe.Sun + blockMidN.Sun) / 4);
                sunTr = (1f / MAX_SUN_VALUE) * ((blockTopNw.Sun + blockTopN.Sun + blockMidNw.Sun + blockMidN.Sun) / 4);
                sunBl = (1f / MAX_SUN_VALUE) * ((blockBotNe.Sun + blockBotN.Sun + blockMidNe.Sun + blockMidN.Sun) / 4);
                sunBr = (1f / MAX_SUN_VALUE) * ((blockBotNw.Sun + blockBotN.Sun + blockMidNw.Sun + blockMidN.Sun) / 4);

                redTl = (1f / MAX_SUN_VALUE) * ((blockTopNe.R + blockTopN.R + blockMidNe.R + blockMidN.R) / 4);
                redTr = (1f / MAX_SUN_VALUE) * ((blockTopNw.R + blockTopN.R + blockMidNw.R + blockMidN.R) / 4);
                redBl = (1f / MAX_SUN_VALUE) * ((blockBotNe.R + blockBotN.R + blockMidNe.R + blockMidN.R) / 4);
                redBr = (1f / MAX_SUN_VALUE) * ((blockBotNw.R + blockBotN.R + blockMidNw.R + blockMidN.R) / 4);

                grnTl = (1f / MAX_SUN_VALUE) * ((blockTopNe.G + blockTopN.G + blockMidNe.G + blockMidN.G) / 4);
                grnTr = (1f / MAX_SUN_VALUE) * ((blockTopNw.G + blockTopN.G + blockMidNw.G + blockMidN.G) / 4);
                grnBl = (1f / MAX_SUN_VALUE) * ((blockBotNe.G + blockBotN.G + blockMidNe.G + blockMidN.G) / 4);
                grnBr = (1f / MAX_SUN_VALUE) * ((blockBotNw.G + blockBotN.G + blockMidNw.G + blockMidN.G) / 4);

                bluTl = (1f / MAX_SUN_VALUE) * ((blockTopNe.B + blockTopN.B + blockMidNe.B + blockMidN.B) / 4);
                bluTr = (1f / MAX_SUN_VALUE) * ((blockTopNw.B + blockTopN.B + blockMidNw.B + blockMidN.B) / 4);
                bluBl = (1f / MAX_SUN_VALUE) * ((blockBotNe.B + blockBotN.B + blockMidNe.B + blockMidN.B) / 4);
                bluBr = (1f / MAX_SUN_VALUE) * ((blockBotNw.B + blockBotN.B + blockMidNw.B + blockMidN.B) / 4);

                localTl = new Color(redTl, grnTl, bluTl);
                localTr = new Color(redTr, grnTr, bluTr);
                localBl = new Color(redBl, grnBl, bluBl);
                localBr = new Color(redBr, grnBr, bluBr);

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                    provider, sunTl, sunTr, sunBl, sunBr, localTl, localTr, localBl, localBr, vertexCount,
                    ref vertices, ref indices);
            }
        }

        protected static void BuildFaceVertices(ReadOnlyChunk chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            BlockFaceDirection faceDir, IBlockProvider provider, float sunLightTl, float sunLightTr, float sunLightBl,
            float sunLightBr, Color localLightTl, Color localLightTr, Color localLightBl, Color localLightBr, int vertexCount,
            ref List<VertexPositionTextureLightEffect> vertices, ref List<short> indices)
        {
            var uvList = provider.GetTexture(faceDir);
            var suns = new float[] { sunLightTr, sunLightTl, sunLightBr, sunLightBl };
            var locals = new Color[] { localLightTr, localLightTl, localLightBr, localLightBl };
            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[7], m_AddVectors[6], m_AddVectors[5], m_AddVectors[3] },
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.XDecreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[2], m_AddVectors[4], m_AddVectors[0], m_AddVectors[1] },
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.YIncreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[7], m_AddVectors[4], m_AddVectors[6], m_AddVectors[2] },
                        new Vector2[] { uvList[4], uvList[5], uvList[1], uvList[3] },
                        new short[] { 3, 2, 0, 3, 0, 1 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.YDecreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[5], m_AddVectors[1], m_AddVectors[3], m_AddVectors[0] },
                        new Vector2[] { uvList[0], uvList[2], uvList[4], uvList[5] },
                        new short[] { 0, 2, 1, 1, 2, 3 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.ZIncreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[4], m_AddVectors[7], m_AddVectors[1], m_AddVectors[5] },
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.ZDecreasing:
                    RenderMesh(provider, blockPosition, suns, locals,
                        new Vector3[] { m_AddVectors[6], m_AddVectors[2], m_AddVectors[3], m_AddVectors[0] },
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    break;
            }
        }
    }
}
