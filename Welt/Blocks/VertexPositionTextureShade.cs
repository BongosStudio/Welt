#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Blocks
{
    [Serializable]
    public struct VertexPositionTextureShade : IVertexType
    {
        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*5, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 1)
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        private Vector3 m_position;
        private float m_shade;
        private Vector2 m_textureCoordinate;

        public VertexPositionTextureShade(Vector3 position, Vector2 textureCoordinate, float shade)
        {
            m_position = position;
            m_textureCoordinate = textureCoordinate;
            m_shade = shade;
        }

        public VertexPositionTextureShade(Vector3 position, Vector3 normal, float shade, Vector2 textureCoordinate)
        {
            //normal is not in use currently
            m_position = position;
            m_textureCoordinate = textureCoordinate;
            m_shade = shade;
        }

        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Vector2 TextureCoordinate
        {
            get { return m_textureCoordinate; }
            set { m_textureCoordinate = value; }
        }

        public float Shade
        {
            get { return m_shade; }
            set { m_shade = value; }
        }

        public static int SizeInBytes => sizeof (float)*6;

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public override string ToString()
        {
            return "(" + m_position + "),(" + m_textureCoordinate + ")," + m_shade;
        }
    }
}