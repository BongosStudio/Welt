using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using Welt.Game.Exceptions.Forge;

namespace Welt.Game.Builders.Forge.Blocks
{
    public abstract class FacelessBlockMesh : IBlockMesh
    {

        protected FacelessBlockMesh()
        {
            
        }

        public (Vector3[] Vertices, byte[] Indices) Back()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public (Vector3[] Vertices, byte[] Indices) Bottom()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public (Vector3[] Vertices, byte[] Indices) Front()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public (Vector3[] Vertices, byte[] Indices) Left()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public (Vector3[] Vertices, byte[] Indices) Right()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public (Vector3[] Vertices, byte[] Indices) Top()
        {
            throw new NotImplementedException("Block is faceless. Use '.All()'");
        }

        public abstract (Vector3[] Vertices, byte[] Indices) All();

        public virtual void Dispose()
        {
            
        }
    }
}
