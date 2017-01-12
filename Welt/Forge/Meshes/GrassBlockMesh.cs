using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SiliconStudio.Core.Mathematics;
using Welt.Game.Exceptions.Forge;

namespace Welt.Game.Builders.Forge.Blocks
{
    public class GrassBlockMesh : FacelessBlockMesh
    {
        private static (Vector3[], byte[]) _data;

        public GrassBlockMesh() : base()
        {
            // these are made locally only because we should only have a single instance of GrassBlockMesh. After
            // that, there is no use to keep the objects in memory if they're already in `_data`.
            
            var vt = new[]
            {
                new[] // X
                {
                    new Vector3(0.3f, 1, 1),
                    new Vector3(0.3f, 1, 0),
                    new Vector3(0.3f, 0, 1),
                    new Vector3(0.3f, 0, 0),
                    new Vector3(0.7f, 1, 1),
                    new Vector3(0.7f, 1, 0),
                    new Vector3(0.7f, 0, 1),
                    new Vector3(0.7f, 0, 0)
                },
                new[] // Z
                {
                    new Vector3(1, 1, 0.3f),
                    new Vector3(1, 0, 0.3f),
                    new Vector3(0, 1, 0.3f),
                    new Vector3(0, 0, 0.3f),
                    new Vector3(1, 1, 0.7f),
                    new Vector3(1, 0, 0.7f),
                    new Vector3(0, 1, 0.7f),
                    new Vector3(0, 0, 0.7f)
                }
            };

            var id = new[]
            {
                new byte[] {0, 1, 2, 2, 1, 3},
                new byte[] {0, 1, 3, 0, 3, 2},
            };

            var va = new Vector3[32];
            var ia = new byte[24];
            var size = 8*Vector3.SizeInBytes;
            Buffer.BlockCopy(vt[0], 0, va, 0, size);
            Buffer.BlockCopy(vt[0], 0, va, 8, size);
            Buffer.BlockCopy(vt[1], 0, va, 16, size);
            Buffer.BlockCopy(vt[1], 0, va, 24, size);

            Buffer.BlockCopy(id[0], 0, ia, 0, 6);
            Buffer.BlockCopy(id[1], 0, ia, 6, 6);
            Buffer.BlockCopy(id[0], 0, ia, 12, 6);
            Buffer.BlockCopy(id[0], 0, ia, 18, 6);

            _data = (va, ia);
        }

        public override (Vector3[] Vertices, byte[] Indices) All()
        {
            return _data;
        }
    }
}
