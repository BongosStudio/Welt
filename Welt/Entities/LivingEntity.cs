#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion
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

    }
}