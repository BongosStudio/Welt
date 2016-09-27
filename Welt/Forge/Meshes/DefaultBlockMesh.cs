using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Welt.Game.Builders.Forge.Blocks
{
    public sealed class DefaultBlockMesh : IBlockMesh
    {
        private static readonly Vector3[][] _vertices = new[]
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

        private static readonly byte[][] _cubeIndices =
        {
            new byte[] {0, 1, 2, 2, 1, 3}, // X+
            new byte[] {0, 1, 3, 0, 3, 2}, // X-
            new byte[] {3, 2, 0, 3, 0, 1}, // Y+
            new byte[] {0, 2, 1, 1, 2, 3}, // Y-
            new byte[] {0, 1, 3, 0, 3, 2}, // Z+
            new byte[] {0, 1, 2, 2, 1, 3}, // Z-
        };

        private static readonly byte[][] _uvs =
        {
            new byte[] { 0, 1, 2, 5 },
            new byte[] { 0, 1, 5, 2 },
            new byte[] { 4, 5, 1, 3 },
            new byte[] { 0, 2, 4, 5 },
            new byte[] { 0, 1, 5, 2 },
            new byte[] { 0, 1, 2, 5 },
        };

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Back()
        {
            return (_vertices[1], _cubeIndices[5], _uvs[5]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Bottom()
        {
            return (_vertices[5], _cubeIndices[3], _uvs[3]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Front()
        {
            return (_vertices[0], _cubeIndices[4], _uvs[4]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Left()
        {
            return (_vertices[3], _cubeIndices[1], _uvs[1]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Right()
        {
            return (_vertices[2], _cubeIndices[0], _uvs[0]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Top()
        {
            return (_vertices[4], _cubeIndices[2], _uvs[2]);
        }
    }
}
