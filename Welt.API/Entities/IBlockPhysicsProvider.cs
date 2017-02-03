using Microsoft.Xna.Framework;
using System;
using Welt.API;
using Welt.API.Forge;

namespace Welt.API.Entities
{
    public interface IBlockPhysicsProvider
    {
        BoundingBox? GetBoundingBox(IWorld world, Vector3I coordinates);
    }
}