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
        private static List<Vector3B> _hashes = new List<Vector3B>(256); // this will contain ALL colors the game is rendering
        private byte[] _table;
        private const int OFFSET = Chunk.Width*Chunk.Depth;

        public LightTable(Chunk chunk)
        {
            _table = new byte[chunk.Height*Chunk.Depth*Chunk.Width];
        }

        public Vector3B this[int x, int y, int z]
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

        private void CreateTree(int x, int y, int z, Vector3B value)
        {
            // to fade evenly, we'll go in all 6 cardinal directions. If the light level already there is higher, no adjusting will
            // occur. The adjustment distance will be the highest value in `value`. 
            var previous = _hashes[_table[Vti(x, y, z)]];
            byte r = previous.X, g = previous.Y, b = previous.Z;
            // get the larger of the two values
            r = r > value.X ? r : value.X;
            g = g > value.Y ? g : value.Y;
            b = b > value.Z ? b : value.Z;
            // make sure they're within the limits
            FastMath.Adjust(0, 15, ref r);
            FastMath.Adjust(0, 15, ref g);
            FastMath.Adjust(0, 15, ref b);
            // check the list if it contains the light level already
            var v = new Vector3B(r, g, b);
            if (!_hashes.Contains(v))
                _hashes.Add(v);
            // set the index in the table to the hash index
            _table[Vti(x, y, z)] = (byte) _hashes.IndexOf(v);
            // branch off
            var newVal = new Vector3B(value.X - 1, value.Y - 1, value.Z - 1);
            CreateTree(x + 1, y, z, newVal);
            CreateTree(x - 1, y, z, newVal);
            CreateTree(x, y + 1, z, newVal);
            CreateTree(x, y - 1, z, newVal);
            CreateTree(x, y, z + 1, newVal);
            CreateTree(x, y, z - 1, newVal);
        }

        private int Vti(int x, int y, int z)
        {
            return x*OFFSET + z*Chunk.Depth + y;
        }
    }
}
