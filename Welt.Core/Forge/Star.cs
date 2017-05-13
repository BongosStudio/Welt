using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class Star : IStar
    {
        public string Name { get; }

        public int Temperature { get; }

        public Color Color { get; }

        public int Size { get; }

        public Star(string name, int temp, Color color, int size)
        {
            Name = name;
            Temperature = temp;
            Color = color;
            Size = size;
        }
    }
}
