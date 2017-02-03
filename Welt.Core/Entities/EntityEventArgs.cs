using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Welt.API.Entities;

namespace Welt.Core.Entities
{
    public class EntityEventArgs : EventArgs
    {
        public IEntity Entity { get; set; }

        public EntityEventArgs(IEntity entity)
        {
            Entity = entity;
        }
    }
}
