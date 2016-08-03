using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Welt.API.Forge;
using Welt.Blocks;

namespace Welt.Forge.Builders
{
    public class DefaultMeshBuilder : IMeshBuilder
    {
        public float MeshWidth => 1;
        public float MeshHeight => 1;
        public float MeshDepth => 1;

        protected static readonly Vector3[][] CubeMesh =
        {
            new[] // Z+
            {
                new Vector3(1, 0, 1),
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
            },
            new[] // Z-
            {
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
            },
            new[] // X+
            {
                new Vector3(1, 0, 0),
                new Vector3(1, 0, 1),
                new Vector3(1, 1, 0),
                new Vector3(1, 1, 1),
            },
            new[] // X-
            {
                new Vector3(0, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(0, 1, 0),
                new Vector3(0, 1, 1),
            },
            new[] // Y+
            {
                new Vector3(0, 1, 0),
                new Vector3(1, 1, 0),
                new Vector3(0, 1, 1),
                new Vector3(1, 1, 1),
            },
            new[] // Y-
            {
                new Vector3(0, 0, 0),
                new Vector3(1, 0, 0),
                new Vector3(0, 0, 1),
                new Vector3(1, 0, 1),
            }
        };

        protected static readonly Vector3[] CubeNormals =
        {
            new Vector3(0, 0, 1),
            new Vector3(0, 0, -1),
            new Vector3(1, 0, 0),
            new Vector3(-1, 0, 0),
            new Vector3(0, 1, 0),
            new Vector3(0, -1, 0)
        };

        protected static readonly byte[][] CubeIndices =
        {
            new byte[] {0, 1, 2, 2, 1, 3}, // X+
            new byte[] {0, 1, 3, 0, 3, 2}, // X-
            new byte[] {3, 2, 0, 3, 0, 1}, // Y+
            new byte[] {0, 2, 1, 1, 2, 3}, // Y-
            new byte[] {0, 1, 3, 0, 3, 2}, // Z+
            new byte[] {0, 1, 2, 2, 1, 3}, // Z-
        };

        public Mesh CreateMesh(Vector3 position, ushort id, BlockFaceDirection face)
        {
            if (face == BlockFaceDirection.None)
                throw new ArgumentException("Block face must be provided in order to create mesh", nameof(face));
            var mesh = new Mesh();

            if ((face & BlockFaceDirection.XDecreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[3].Select(v => v += position), CubeIndices[1],
                    BlockFaceDirection.XDecreasing));
            }
            if ((face & BlockFaceDirection.XIncreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[2].Select(v => v += position), CubeIndices[0],
                    BlockFaceDirection.XIncreasing));
            }
            if ((face & BlockFaceDirection.ZDecreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[1].Select(v => v += position), CubeIndices[5],
                    BlockFaceDirection.ZDecreasing));
            }
            if ((face & BlockFaceDirection.ZIncreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[0].Select(v => v += position), CubeIndices[4],
                    BlockFaceDirection.ZIncreasing));
            }
            if ((face & BlockFaceDirection.YDecreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[5].Select(v => v += position), CubeIndices[3],
                    BlockFaceDirection.YDecreasing));
            }
            if ((face & BlockFaceDirection.YIncreasing) > 0)
            {
                mesh.AddSubmesh(new Mesh(CubeMesh[4].Select(v => v += position), CubeIndices[2],
                    BlockFaceDirection.YIncreasing));
            }

            return mesh;
        }

        public bool CanBuild(ushort id, byte md)
        {
            return true; // TODO: because this can't build everything.
        }
    }
}