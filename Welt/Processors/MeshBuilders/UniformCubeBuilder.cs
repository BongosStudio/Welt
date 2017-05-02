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
            Vector3I chunkRelativePosition, BlockFaceDirection face, int vertexCount,
            ref List<VertexPositionNormalTextureEffect> vertices, ref List<short> indices)
        {
            if (provider.Id == 0) return;
            var blockPosition = chunk.GetPosition() + chunkRelativePosition;
            BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, face,
                    provider, vertexCount,
                    ref vertices, ref indices);
        }

        protected static void BuildFaceVertices(ReadOnlyChunk chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            BlockFaceDirection faceDir, IBlockProvider provider, int vertexCount,
            ref List<VertexPositionNormalTextureEffect> vertices, ref List<short> indices)
        {
            var uvList = provider.GetTexture(faceDir);
            switch (faceDir)
            {
                case BlockFaceDirection.XIncreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[7], m_AddVectors[6], m_AddVectors[5], m_AddVectors[3] },
                        Normals[(int)BlockFaceDirection.XIncreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.XDecreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[2], m_AddVectors[4], m_AddVectors[0], m_AddVectors[1] },
                        Normals[(int)BlockFaceDirection.XDecreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.YIncreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[7], m_AddVectors[4], m_AddVectors[6], m_AddVectors[2] },
                        Normals[(int)BlockFaceDirection.YIncreasing],
                        new Vector2[] { uvList[4], uvList[5], uvList[1], uvList[3] },
                        new short[] { 3, 2, 0, 3, 0, 1 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.YDecreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[5], m_AddVectors[1], m_AddVectors[3], m_AddVectors[0] },
                        Normals[(int)BlockFaceDirection.YDecreasing],
                        new Vector2[] { uvList[0], uvList[2], uvList[4], uvList[5] },
                        new short[] { 0, 2, 1, 1, 2, 3 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.ZIncreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[4], m_AddVectors[7], m_AddVectors[1], m_AddVectors[5] },
                        Normals[(int)BlockFaceDirection.ZIncreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.ZDecreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[6], m_AddVectors[2], m_AddVectors[3], m_AddVectors[0] },
                        Normals[(int)BlockFaceDirection.ZDecreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    break;
            }
        }
    }
}
