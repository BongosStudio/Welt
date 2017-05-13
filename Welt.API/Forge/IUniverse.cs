using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IUniverse
    {
        IGalaxy GetGalaxy(Vector3B position);
        bool SetGalaxy(Vector3B position, IGalaxy galaxy);
    }
}
