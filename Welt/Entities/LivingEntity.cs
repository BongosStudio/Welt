#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Welt.Models;
// ReSharper disable CompareOfFloatsByEqualityOperator

namespace Welt.Entities
{
    public abstract class LivingEntity : Entity
    {
        private bool _isInWater;
        private bool _isOnFire;
        private bool _isRunning;
        private bool _isMoving;
        private bool _isCrouching;
        private float _health;
        private float _stamina;
        private InventoryContainer _inventory;

        public virtual bool IsInWater
        {
            get { return _isInWater; }
            set
            {
                if (_isInWater == value) return;
                _isInWater = value;
                OnPropertyChanged();
            }
        }

        public virtual bool IsOnFire
        {
            get { return _isOnFire; }
            set
            {
                if (_isOnFire == value) return;
                _isOnFire = value;
                OnPropertyChanged();
            }
        }

        public virtual bool IsRunning
        {
            get { return _isRunning; }
            set
            {
                if (_isRunning == value) return;
                _isRunning = value;
                OnPropertyChanged();
            }
        }

        public virtual bool IsMoving
        {
            get { return _isMoving; }
            set
            {
                if (_isMoving == value) return;
                _isMoving = value;
                OnPropertyChanged();
            }
        }

        public virtual bool IsCrouching
        {
            get { return _isCrouching; }
            set
            {
                if (_isCrouching == value) return;
                _isCrouching = value;
                OnPropertyChanged();
            }
        }

        public virtual float Health
        {
            get { return _health; }
            set
            {
                if (_health == value) return;
                _health = value;
                OnPropertyChanged();
            }
        }

        public virtual float Stamina
        {
            get { return _stamina; }
            set
            {
                if (_stamina == value) return;
                _stamina = value;
                OnPropertyChanged();
            }
        }

        public virtual InventoryContainer Inventory
        {
            get { return _inventory; }
            set
            {
                if (_inventory == value) return;
                _inventory = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler PositionChanged;
        public event EventHandler TargetPointChanged;
        public event EventHandler StaminaChanged;
        public event EventHandler DamageDealt;
        public event EventHandler DamageRecieved;
        public event EventHandler InventoryUpdated;
        
    }
}