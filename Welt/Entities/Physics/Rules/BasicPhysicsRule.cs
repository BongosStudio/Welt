using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Welt.Entities.Physics.Rules
{
    public class BasicPhysicsRule : IPhysicsRule
    {
        public bool CanMoveTo(Vector3 current, Vector3 target, Vector3 speed)
        {
            return true;
        }
    }
}
