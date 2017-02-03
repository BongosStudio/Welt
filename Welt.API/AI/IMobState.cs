using System;
using Welt.API.Entities;

namespace Welt.API.AI
{
    public interface IMobState
    {
        void Update(IMobEntity entity, IEntityManager manager);
    }
}