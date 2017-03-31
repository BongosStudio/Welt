using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Graphics
{
    public struct ParticleVertex : IVertexType 
    {
        public Vector2 Corner;
        public Vector3 Position;
        public Vector3 Velocity;
        public Vector4 Random;
        public float Time;

        public static readonly VertexElement[] Elements = new[]
        {
            new VertexElement(0, VertexElementFormat.Vector2, VertexElementUsage.Position, 0),
            new VertexElement(sizeof(float)*2, VertexElementFormat.Vector3, VertexElementUsage.Position, 1),
            new VertexElement(sizeof(float)*5, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(sizeof(float)*8, VertexElementFormat.Vector4, VertexElementUsage.Color, 0),
            new VertexElement(sizeof(float)*9, VertexElementFormat.Single, VertexElementUsage.TextureCoordinate, 0)
        };
        public VertexDeclaration VertexDeclaration => new VertexDeclaration(Elements);

        public ParticleVertex(Vector2 corner, Vector3 position, Vector3 velocity, Vector4 random, float time)
        {
            Corner = corner;
            Position = position;
            Velocity = velocity;
            Random = random;
            Time = time;
        }
    }
}
