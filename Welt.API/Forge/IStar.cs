using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IStar
    {
        string Name { get; }
        int Temperature { get; }
        Color Color { get; }
        int Size { get; } // between 8,448 and 16,192. Base size is 14,080 to a planet that is 256.
    }
}
