using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

namespace Welt.Graphics
{
    [DebuggerDisplay("Position: {Position}, Normal: {Normal}, Texture: {Texture}, Effect: {Effect}")]
    public struct VertexPositionNormalTextureEffect : IVertexType 
    {
        public Vector3 Position { get; set; }
        public Vector3 Normal { get; set; }
        public Vector2 Texture { get; set; }
        public float Effect { get; set; }

        public VertexPositionNormalTextureEffect(Vector3 position, Vector3 normal, Vector2 texture, float effect)
        {
            Position = position;
            Normal = normal;
            Texture = texture;
            Effect = effect;
        }

        public static readonly VertexElement[] VertexElements =
        {
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(sizeof (float)*3, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof (float)*6, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(sizeof (float)*8, VertexElementFormat.Single, VertexElementUsage.BlendWeight, 0)
        };

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration => VertexDeclaration;
    }
}
