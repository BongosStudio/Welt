using Microsoft.Xna.Framework;
using System;
using System.ComponentModel;
using Welt.API.Forge;
using Welt.API.Net;

namespace Welt.API.Entities
{
    public interface IEntity : INotifyPropertyChanged
    {
        IPacket SpawnPacket { get; }
        int EntityID { get; set; }
        Vector3 Position { get; set; }
        float Yaw { get; set; }
        float Pitch { get; set; }
        bool Despawned { get; set; }
        DateTime SpawnTime { get; set; }
        Size Size { get; }
        MetadataDictionary Metadata { get; }
        IEntityManager EntityManager { get; set; }
        IWorld World { get; set; }
        bool SendMetadataToClients { get; }
        void Update(IEntityManager entityManager);
    }
}