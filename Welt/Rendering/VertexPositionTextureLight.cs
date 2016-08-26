#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering
{
    [Serializable]
    public struct VertexPositionTextureLight : IVertexType
    {
        private Vector3 _mPosition;
        private Vector2 _mTextureCoordinate1;
        private float _mSunLight;
        private Vector3 _mLocalLight;

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
            _mPosition = position;
            _mTextureCoordinate1 = textureCoordinate1;
            _mSunLight = sunLight;
            _mLocalLight = localLight;
        }


        public override string ToString()
        {
            return $"({_mPosition}),({_mTextureCoordinate1}),({_mTextureCoordinate1})";
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

        public Vector3 LocalLight
        {
            get { return _mLocalLight; }
            set { _mLocalLight = value; }
        }

        public float SunLight
        {
            get { return _mSunLight; }
            set { _mSunLight = value; }
        }

        public static int SizeInBytes => sizeof (float)*8;
    }
}
