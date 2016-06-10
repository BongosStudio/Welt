#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Welt.Annotations;

namespace Welt.Entities
{
    public abstract class Entity : INotifyPropertyChanged
    {
        public virtual DataMap Data => new DataMap();
        public abstract EntityClass EntityClass { get; }

        public event EventHandler Spawn;
        public event EventHandler Despawn;

        public virtual void Update(GameTime time)
        {
            // insert logic here
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}