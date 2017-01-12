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
        private Vector3 _mPosition;
        private float _mShade;
        private Vector2 _mTextureCoordinate;

        public VertexPositionTextureShade(Vector3 position, Vector2 textureCoordinate, float shade)
        {
            _mPosition = position;
            _mTextureCoordinate = textureCoordinate;
            _mShade = shade;
        }

        public VertexPositionTextureShade(Vector3 position, Vector3 normal, float shade, Vector2 textureCoordinate)
        {
            //normal is not in use currently
            _mPosition = position;
            _mTextureCoordinate = textureCoordinate;
            _mShade = shade;
        }

        public Vector3 Position
        {
            get { return _mPosition; }
            set { _mPosition = value; }
        }

        public Vector2 TextureCoordinate
        {
            get { return _mTextureCoordinate; }
            set { _mTextureCoordinate = value; }
        }

        public float Shade
        {
            get { return _mShade; }
            set { _mShade = value; }
        }

        public static int SizeInBytes => sizeof (float)*6;

        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;

        public override string ToString()
        {
            return "(" + _mPosition + "),(" + _mTextureCoordinate + ")," + _mShade;
        }
    }
}