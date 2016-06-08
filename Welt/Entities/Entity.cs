#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;

namespace Welt.Entities
{
    public abstract class Entity
    {
        public virtual DataMap Data => new DataMap();
        public abstract EntityClass EntityClass { get; }

        public event EventHandler Spawn;
        public event EventHandler Despawn;

    }
}