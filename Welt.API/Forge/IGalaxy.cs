using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface IGalaxy
    {
        string Name { get; }
        Vector3B Position { get; }

        ISolarSystem GetSystem(Vector3B system);
        bool SetSystem(Vector3B position, ISolarSystem value);
    }
}
