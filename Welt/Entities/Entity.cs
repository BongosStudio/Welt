#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Welt.Annotations;
using Welt.API.Forge;

// this whole namespace should be moved to Welt.Core
namespace Welt.Entities
{
    public abstract class Entity : INotifyPropertyChanged
    {
        public virtual DataMap Data => new DataMap();
        public abstract EntityClass EntityClass { get; }
        public Vector3 Position;
        public Vector3 Speed;
        public float Pitch;
        public float Yaw;
        public IWorld World;

        public event EventHandler Spawn;
        public event EventHandler Despawn;
        public event PropertyChangedEventHandler PropertyChanged;

        public virtual void Update(GameTime time)
        {
            // insert logic here
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}