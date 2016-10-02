using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Core;
using Welt.Core.Forge;
using Welt.Types;

namespace Welt.Forge
{
    public class LightTable
    {
        private static List<(byte R, byte G, byte B)> _hashes = new List<(byte R, byte G, byte B)>(256)
        {
            (0, 0, 0)
        }; // this will contain ALL colors the game is rendering
        private byte[] _table;
        private const int OFFSET = Chunk.Width*Chunk.Depth;

        public LightTable(Chunk chunk)
        {
            _table = new byte[chunk.Height*Chunk.Depth*Chunk.Width];
        }

        public (byte R, byte G, byte B) this[int x, int y, int z]
        {
            get
            {
                return _hashes[Vti(x, y, z)];
            }
            set
            {
                CreateTree(x, y, z, value);
            }
        }

        public void ClearTable()
        {
            var l = _table.Length;
            _table = new byte[l];
        }

        private void CreateTree(int x, int y, int z, (byte R, byte G, byte B) value)
        {
            // create the light at this point and spread throughout to the rest. 
        }

        private int Vti(int x, int y, int z)
        {
            return x*OFFSET + z*Chunk.Depth + y;
        }
    }
}
