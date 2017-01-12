#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Blocks
{
    [Serializable]
    public struct VertexPositionTextureLightEffect : IVertexType
    {
        private Vector3 m_Position;
        private Vector2 m_TexCoords1;
        private float m_SunLight;
        private Vector3 m_LocalLight;
        private float m_BlockEffect;

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*5, VertexElementFormat.Single, VertexElementUsage.Color, 0),
            new VertexElement(sizeof (float)*6, VertexElementFormat.Vector3, VertexElementUsage.Color, 1),
            new VertexElement(sizeof (float)*9, VertexElementFormat.Single, VertexElementUsage.BlendWeight, 1)
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public VertexPositionTextureLightEffect(Vector3 position, Vector2 textureCoordinate1, float sunLight,
            Vector3 localLight, BlockEffect effect)
        {
            m_Position = position;
            m_TexCoords1 = textureCoordinate1;
            m_SunLight = sunLight;
            m_LocalLight = localLight;
            m_BlockEffect = (float)effect;
        }


        public override string ToString()
        {
            return $"({m_Position}),({m_TexCoords1}),({m_TexCoords1})";
        }

        public Vector3 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Vector2 TextureCoordinate1
        {
            get { return m_TexCoords1; }
            set { m_TexCoords1 = value; }
        }

        public Vector3 LocalLight
        {
            get { return m_LocalLight; }
            set { m_LocalLight = value; }
        }

        public float SunLight
        {
            get { return m_SunLight; }
            set { m_SunLight = value; }
        }

        public float BlockEffect
        {
            get { return m_BlockEffect; }
            set { m_BlockEffect = value; }
        }
    }
}
