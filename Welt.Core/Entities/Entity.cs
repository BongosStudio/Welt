using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Welt.API.Entities;
using Welt.API;
using Welt.API.Forge;
using Microsoft.Xna.Framework;
using System.Runtime.CompilerServices;
using Welt.API.Net;

namespace Welt.Core.Entities
{
    public abstract class Entity : IEntity
    {
        protected Entity()
        {
            EnablePropertyChange = true;
            EntityID = -1;
            SpawnTime = DateTime.UtcNow;
        }

        public DateTime SpawnTime { get; set; }

        public int EntityID { get; set; }
        public IEntityManager EntityManager { get; set; }
        public IWorld World { get; set; }

        public virtual MetadataDictionary Metadata => null;

        protected Vector3 _Position;
        public virtual Vector3 Position
        {
            get { return _Position; }
            set
            {
                _Position = value;
                OnPropertyChanged();
            }
        }

        protected Vector3 _Velocity;
        public virtual Vector3 Velocity
        {
            get { return _Velocity; }
            set
            {
                _Velocity = value;
                OnPropertyChanged();
            }
        }

        protected float _Yaw;
        public float Yaw
        {
            get { return _Yaw; }
            set
            {
                _Yaw = value;
                OnPropertyChanged();
            }
        }

        protected float _Pitch;
        public float Pitch
        {
            get { return _Pitch; }
            set
            {
                _Pitch = value;
                OnPropertyChanged();
            }
        }

        public bool Despawned { get; set; }

        public abstract Size Size { get; }

        public abstract IPacket SpawnPacket { get; }

        public virtual bool SendMetadataToClients { get { return false; } }

        protected EntityFlags _EntityFlags;
        public virtual EntityFlags EntityFlags
        {
            get { return _EntityFlags; }
            set
            {
                _EntityFlags = value;
                OnPropertyChanged();
            }
        }

        public virtual void Update(IEntityManager entityManager)
        {
            // TODO: Losing health and all that jazz
            if (Position.Y < -50)
                entityManager.DespawnEntity(this);
        }

        protected bool EnablePropertyChange { get; set; }

        

        public event PropertyChangedEventHandler PropertyChanged;
        protected internal virtual void OnPropertyChanged([CallerMemberName] string property = "")
        {
            if (!EnablePropertyChange) return;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}
