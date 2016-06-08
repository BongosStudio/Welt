#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Welt.Models;

namespace Welt.Entities
{
    public abstract class LivingEntity : Entity
    {
        public virtual bool IsInWater { get; set; }
        public virtual bool IsOnFire { get; set; }
        public virtual bool IsRunning { get; set; }
        public virtual bool IsMoving { get; set; }

        //public virtual InventoryMap Inventory { get; set; }

        public virtual float Health { get; set; }
        public virtual float Stamina { get; set; }
        public virtual InventoryContainer Inventory { get; set; }

        public event EventHandler PositionChanged;
        public event EventHandler TargetPointChanged;
        public event EventHandler StaminaChanged;
        public event EventHandler DamageDealt;
        public event EventHandler DamageRecieved;
        public event EventHandler InventoryUpdated;
    }
}