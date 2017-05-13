using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class Galaxy : IGalaxy
    {
        public string Name { get; }

        public Vector3B Position { get; }

        public Galaxy(IUniverse universe, Vector3B position)
        {

        }

        public Galaxy(IUniverse universe, string name, Vector3B position)
        {

        }

        public ISolarSystem GetSystem(Vector3B system)
        {
            throw new NotImplementedException();
        }

        public bool SetSystem(Vector3B position, ISolarSystem value)
        {
            throw new NotImplementedException();
        }
    }
}
