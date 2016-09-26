using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Welt.API.Forge;
using System;

namespace Welt.Rendering
{
    public sealed class Mesh
    {
        public static int VertexCount { get; private set; }
        public static int IndexCount { get; private set; }

        public VertexPositionTextureLight[] Vertices { get; private set; }
        public byte[] Indices { get; private set; }
        public Vector2 TextureCoords { get; private set; }
        
        /// <summary>
        ///     Creates a child mesh with assigned vertices and indices.
        /// </summary>
        /// <param name="v"></param>
        /// <param name="face"></param>
        /// <param name="i"></param>
        /// <param name="texture"></param>
        public Mesh(IEnumerable<VertexPositionTextureLight> v, IEnumerable<byte> i, Vector2 texCoords)
        {
            Vertices = v.ToArray();
            Indices = i.ToArray();
            TextureCoords = texCoords;
            VertexCount += Vertices.Length;
            IndexCount += Indices.Length;
        }
        
        ~Mesh()
        {
            VertexCount -= Vertices.Length;
            IndexCount -= Indices.Length;
            Vertices = null;
            Indices = null;
        }
    }
}