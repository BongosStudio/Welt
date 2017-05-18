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

        private OccupiedSpacialMap<ISolarSystem> m_Map;

        public Galaxy(IUniverse universe, Vector3B position)
        {
            Position = position;
            m_Map = new OccupiedSpacialMap<ISolarSystem>(256);
        }

        public Galaxy(IUniverse universe, string name, Vector3B position) : this(universe, position)
        {
            Name = name;

        }

        public ISolarSystem GetSystem(Vector3B system)
        {
            return m_Map.GetObject(system.X, system.Z, out var x, out var y);
        }

        public bool SetSystem(Vector3B position, ISolarSystem value)
        {
            return m_Map.TrySetPosition(position.X, position.Z, value, 4);
        }
    }
}
