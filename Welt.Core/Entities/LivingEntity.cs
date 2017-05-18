using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Welt.API;

namespace Welt.Core.Entities
{
    public abstract class LivingEntity : Entity
    {
        protected LivingEntity()
        {
            Health = MaxHealth;
        }

        protected short _Air;
        public short Air
        {
            get { return _Air; }
            set
            {
                _Air = value;
                OnPropertyChanged();
            }
        }

        protected short _Health;
        public short Health
        {
            get { return _Health; }
            set
            {
                _Health = value;
                OnPropertyChanged();
            }
        }

        protected float _HeadYaw;
        public float HeadYaw
        {
            get { return _HeadYaw; }
            set
            {
                _HeadYaw = value;
                OnPropertyChanged();
            }
        }

        protected bool _IsFlying;
        public bool IsFlying
        {
            get { return _IsFlying; }
            set
            {
                _IsFlying = value;
                OnPropertyChanged();
            }
        }

        public override bool SendMetadataToClients
        {
            get
            {
                return true;
            }
        }

        public abstract short MaxHealth { get; }
        public abstract AppendageReport[] AppendageReports { get; }
    }
}
