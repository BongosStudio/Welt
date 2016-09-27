using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Game.Builders.Forge.Blocks
{
    public class GrassBlockMesh : IBlockMesh
    {
        private static Vector3[][] _vertices = new[]
        {
            new Vector3[]
            {
                new Vector3(0.3f, 1, 1),
                new Vector3(0.3f, 1, 0),
                new Vector3(0.3f, 0, 1),
                new Vector3(0.3f, 0, 0),
                new Vector3(0.7f, 1, 1),
                new Vector3(0.7f, 1, 0),
                new Vector3(0.7f, 0, 1),
                new Vector3(0.7f, 0, 0)
            },
            new Vector3[]
            {
                new Vector3(1, 1, 0.3f),
                new Vector3(1, 0, 0.3f),
                new Vector3(0, 1, 0.3f),
                new Vector3(0, 0, 0.3f),
                new Vector3(1, 1, 0.7f),
                new Vector3(1, 0, 0.7f),
                new Vector3(0, 1, 0.7f),
                new Vector3(0, 0, 0.7f)
            }
        };

        private static byte[][] _indices = new[]
        {
            new byte[]
            {
                0, 1, 2, 2, 1, 3
            },
            new byte[]
            {
                0, 1, 3, 0, 3, 2,
            }
        };

        private static byte[][] _uvs = new[]
        {
            new byte[]
            {
                0, 1, 2, 5
            },
            new byte[]
            {
                0, 1, 5, 2
            }
        };

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Back()
        {
            return (_vertices[1], _indices[0], _uvs[0]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Bottom()
        {
            return (new Vector3[0], new byte[0], new byte[0]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Front()
        {
            return (_vertices[1], _indices[1], _uvs[1]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Left()
        {
            return (_vertices[0], _indices[0], _uvs[0]);   
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Right()
        {
            return (_vertices[0], _indices[1], _uvs[1]);
        }

        public (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Top()
        {
            return (new Vector3[0], new byte[0], new byte[0]);
        }
    }
}
