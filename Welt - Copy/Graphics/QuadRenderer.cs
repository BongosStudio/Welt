using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Graphics
{
    public class QuadRenderer
    {
        public WeltGame Game { get; }
        public GraphicsDevice Graphics { get; }

        #region Private Members       
        
        private VertexPositionTexture[] m_Vertices = 
                        {
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(1,1)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(0,1)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(0,0)),
                            new VertexPositionTexture(
                                new Vector3(0,0,0),
                                new Vector2(1,0))
                        };
        private short[] m_Indices = { 0, 1, 2, 2, 3, 0 };

        #endregion

        public QuadRenderer(WeltGame game, GraphicsDevice graphics)
        {
            Game = game;
            Graphics = graphics;
        }

        public void Render(Vector2 v1, Vector2 v2)
        {
            m_Vertices[0].Position.X = v2.X;
            m_Vertices[0].Position.Y = v1.Y;

            m_Vertices[1].Position.X = v1.X;
            m_Vertices[1].Position.Y = v1.Y;

            m_Vertices[2].Position.X = v1.X;
            m_Vertices[2].Position.Y = v2.Y;

            m_Vertices[3].Position.X = v2.X;
            m_Vertices[3].Position.Y = v2.Y;

            Graphics.DrawUserIndexedPrimitives
                (PrimitiveType.TriangleList, m_Vertices, 0, 4, m_Indices, 0, 2);
        }
    }
}
