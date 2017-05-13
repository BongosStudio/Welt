using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API;
using Welt.API.Forge;
using Welt.Core.Extensions;

namespace Welt.Core.Forge
{
    public class SolarSystem : ISolarSystem
    {
        public string Name { get; }

        public IGalaxy Galaxy { get; }

        public Vector3B Position { get; }

        public SolarSystem(IGalaxy galaxy, Vector3B position) : this(galaxy, galaxy.GenerateRandomSystemName(), position)
        {

        }

        public SolarSystem(IGalaxy galaxy, string name, Vector3B position)
        {
            Galaxy = galaxy;
            Name = name;
            Position = position;
        }

        private OccupiedSpacialMap<IWorld> m_Worlds;

        public IWorld GetWorld(Vector3B position)
        {
            return m_Worlds.GetObject(position.X, position.Z, out var centerX, out var centerY);
        }

        public bool SetWorld(Vector3B position, IWorld value)
        {
            return m_Worlds.TrySetPosition(position.X, position.Z, value, value.Size);
        }
    }
}
