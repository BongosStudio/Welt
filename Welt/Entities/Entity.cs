#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;

namespace Welt.Entities
{
    public abstract class Entity
    {
        public virtual DataMap Data => new DataMap();
        public abstract EntityClass EntityClass { get; }

        public event EventHandler Spawn;
        public event EventHandler Despawn;

        public virtual void Update(GameTime time)
        {
            // insert logic here
        }
    }
}