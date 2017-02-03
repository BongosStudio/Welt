using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Welt.API.Physics
{
    public interface IAABBEntity : IPhysicsEntity
    {
        BoundingBox BoundingBox { get; }
        Size Size { get; }

        void TerrainCollision(Vector3 collisionPoint, Vector3 collisionDirection);
    }
}
