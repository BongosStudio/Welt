#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Blocks
{
    [Serializable]
    public struct VertexPositionTextureLight : IVertexType
    {
        private Vector3 m_position;
        private Vector2 m_textureCoordinate1;
        private float m_sunLight;
        private Vector3 m_localLight;

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*5, VertexElementFormat.Single, VertexElementUsage.Color, 0),
            new VertexElement(sizeof (float)*6, VertexElementFormat.Vector3, VertexElementUsage.Color, 1)
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexPositionTextureLight(Vector3 position, Vector2 textureCoordinate1, float sunLight,
            Vector3 localLight)
        {
            m_position = position;
            m_textureCoordinate1 = textureCoordinate1;
            m_sunLight = sunLight;
            m_localLight = localLight;
        }


        public override string ToString()
        {
            return $"({m_position}),({m_textureCoordinate1}),({m_textureCoordinate1})";
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

        public Vector3 LocalLight
        {
            get { return m_localLight; }
            set { m_localLight = value; }
        }

        public float SunLight
        {
            get { return m_sunLight; }
            set { m_sunLight = value; }
        }

        public static int SizeInBytes => sizeof (float)*8;
    }
}
