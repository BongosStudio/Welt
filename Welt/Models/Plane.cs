using Welt.Blocks;

namespace Welt.Models
{
    public class Plane
    {
        public VertexPositionTextureLightEffect[] Vertices;
        public short[] Indices;
        public short IndexOffset;
        public int Id;
        
        public Plane(VertexPositionTextureLightEffect[] vertices, short[] indices, short indexOffset, int id)
        {
            Vertices = vertices;
            Indices = indices;
            IndexOffset = indexOffset;
            Id = id;
        } 
    }
}
