using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.Core.Services;
using Welt.Entities;
using Welt.Game.Extensions;

namespace Welt.Controllers
{
    public class PhysicsController
    {
        public static bool TryMoveTo(Entity entity, Vector3 to)
        {
            
            return true;
        }

        public static void CreateContactWith(Entity entity, ushort id, byte metadata)
        {
            if (id == 0)
                return;
            // TODO: lava, suffocation, water, harmful blocks, etc.
        }
    }
}
