#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Blocks
{
    [Serializable]
    public struct VertexPositionDualTexture : IVertexType
    {
        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*5, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
            new VertexElement(sizeof (float)*7, VertexElementFormat.Single, VertexElementUsage.Color, 0)
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        private float m_aoWeight;
        private Vector3 m_position;
        private Vector2 m_textureCoordinate1;
        private Vector2 m_textureCoordinate2;

        public VertexPositionDualTexture(Vector3 position, Vector2 textureCoordinate1, Vector2 textureCoordinate2,
            float aoWeight)
        {
            m_position = position;
            m_textureCoordinate1 = textureCoordinate1;
            m_textureCoordinate2 = textureCoordinate2;
            m_aoWeight = aoWeight;
        }

        public Vector3 Position
        {
            get { return m_position; }
            set { m_position = value; }
        }

        public Vector2 TextureCoordinate1
        {
            get { return m_textureCoordinate1; }
            set { m_textureCoordinate1 = value; }
        }

        public Vector2 TextureCoordinate2
        {
            get { return m_textureCoordinate2; }
            set { m_textureCoordinate2 = value; }
        }

        public float AoWeight
        {
            get { return m_aoWeight; }
            set { m_aoWeight = value; }
        }

        public static int SizeInBytes
        {
            get { return sizeof (float)*8; }
        }

        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        public override string ToString()
        {
            return "(" + m_position + "),(" + m_textureCoordinate1 + "),(" + m_textureCoordinate1 + ")";
        }
    }
}