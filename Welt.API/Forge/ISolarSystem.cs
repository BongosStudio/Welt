using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.API.Forge
{
    public interface ISolarSystem
    {
        string Name { get; }
        IGalaxy Galaxy { get; }
        Vector3B Position { get; }

        IWorld GetWorld(Vector3B position);
        bool SetWorld(Vector3B position, IWorld value);
    }
}
