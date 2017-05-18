using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Physics;

namespace Welt.API.Forge
{
    public class PlayerCollidedEventArgs : EventArgs
    {
        public IAABBEntity PlayerEntity { get; }
        public Vector3I Position { get; }

        public PlayerCollidedEventArgs(IAABBEntity player, Vector3I position)
        {
            PlayerEntity = player;
            Position = position;
        }
    }
}
