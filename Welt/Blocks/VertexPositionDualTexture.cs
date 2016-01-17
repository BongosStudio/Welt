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
        private float _mAoWeight;
        private Vector3 _mPosition;
        private Vector2 _mTextureCoordinate1;
        private Vector2 _mTextureCoordinate2;

        public VertexPositionDualTexture(Vector3 position, Vector2 textureCoordinate1, Vector2 textureCoordinate2,
            float aoWeight)
        {
            _mPosition = position;
            _mTextureCoordinate1 = textureCoordinate1;
            _mTextureCoordinate2 = textureCoordinate2;
            _mAoWeight = aoWeight;
        }

        public Vector3 Position
        {
            get { return _mPosition; }
            set { _mPosition = value; }
        }

        public Vector2 TextureCoordinate1
        {
            get { return _mTextureCoordinate1; }
            set { _mTextureCoordinate1 = value; }
        }

        public Vector2 TextureCoordinate2
        {
            get { return _mTextureCoordinate2; }
            set { _mTextureCoordinate2 = value; }
        }

        public float AoWeight
        {
            get { return _mAoWeight; }
            set { _mAoWeight = value; }
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
            return "(" + _mPosition + "),(" + _mTextureCoordinate1 + "),(" + _mTextureCoordinate1 + ")";
        }
    }
}