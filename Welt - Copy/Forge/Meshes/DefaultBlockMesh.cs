using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;

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

        public (Vector3[] Vertices, byte[] Indices) Back()
        {
            return (_vertices[1], _cubeIndices[5]);
        }

        public (Vector3[] Vertices, byte[] Indices) Bottom()
        {
            return (_vertices[5], _cubeIndices[3]);
        }

        public (Vector3[] Vertices, byte[] Indices) Front()
        {
            return (_vertices[0], _cubeIndices[4]);
        }

        public (Vector3[] Vertices, byte[] Indices) Left()
        {
            return (_vertices[3], _cubeIndices[1]);
        }

        public (Vector3[] Vertices, byte[] Indices) Right()
        {
            return (_vertices[2], _cubeIndices[0]);
        }

        public (Vector3[] Vertices, byte[] Indices) Top()
        {
            return (_vertices[4], _cubeIndices[2]);
        }
    }
}
