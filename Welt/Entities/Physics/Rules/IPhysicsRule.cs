using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Entities.Physics.Rules
{
    public interface IPhysicsRule
    {
        bool CanMoveTo(Vector3 current, Vector3 target, Vector3 speed);
    }
}
