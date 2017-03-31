using Microsoft.Xna.Framework;
using Welt.Forge;
using Welt.API;
using Welt.Core.Forge;
using Welt.Graphics;
using System.Collections.Generic;
using Welt.API.Forge;
using Welt.Forge.Renderers;

namespace Welt.Processors.MeshBuilders
{
    public class LadderBuilder : BlockMeshBuilder
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
            ref List<VertexPositionNormalTextureEffect> vertices, ref List<short> indices)
        {
            if (provider.Id == 0) return;
            var blockPosition = chunk.GetPosition() + chunkRelativePosition;


            // XDecreasing
            if ((faces & ChunkRenderer.VisibleFaces.West) != 0)
            {
                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.East) != 0)
            {
                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.Bottom) != 0)
            {
                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YDecreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.Top) != 0)
            {

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.YIncreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.South) != 0)
            {

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
            if ((faces & ChunkRenderer.VisibleFaces.North) != 0)
            {

                BuildFaceVertices(chunk, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                    provider, vertexCount,
                    ref vertices, ref indices);
            }
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
                        new Vector3[] { m_AddVectors[7] + Vector3.Left, m_AddVectors[6] + Vector3.Left, m_AddVectors[5] + Vector3.Left, m_AddVectors[3] + Vector3.Left },
                        Normals[(int)BlockFaceDirection.XIncreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.XDecreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[2] + Vector3.Right, m_AddVectors[4] + Vector3.Right, m_AddVectors[0] + Vector3.Right, m_AddVectors[1] + Vector3.Right },
                        Normals[(int)BlockFaceDirection.XDecreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    
                    break;
                case BlockFaceDirection.ZIncreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[4] + Vector3.Forward, m_AddVectors[7] + Vector3.Forward, m_AddVectors[1] + Vector3.Forward, m_AddVectors[5] + Vector3.Forward },
                        Normals[(int)BlockFaceDirection.ZIncreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                        new short[] { 0, 1, 3, 0, 3, 2 }, vertexCount, ref vertices, ref indices);
                    break;
                case BlockFaceDirection.ZDecreasing:
                    RenderMesh(provider, blockPosition,
                        new Vector3[] { m_AddVectors[6] + Vector3.Backward, m_AddVectors[2] + Vector3.Backward, m_AddVectors[3] + Vector3.Backward, m_AddVectors[0] + Vector3.Backward },
                        Normals[(int)BlockFaceDirection.ZDecreasing],
                        new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                        new short[] { 0, 1, 2, 2, 1, 3 }, vertexCount, ref vertices, ref indices);
                    
                    break;
            }
        }
    }
}
