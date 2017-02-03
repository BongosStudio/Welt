using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Welt.API;
using Welt.API.Net;
using Welt.Core.Net.Packets;

namespace Welt.Core.Entities
{
    public class PlayerEntity : LivingEntity
    {
        public PlayerEntity(string username) : base()
        {
            Username = username;
        }

        public const float Width = 0.6f;
        public const float Height = 1.62f;
        public const float Depth = 0.6f;

        public override IPacket SpawnPacket
        {
            get
            {
                return new SpawnPlayerPacket(EntityID, Username,
                    FastMath.CreateAbsoluteInt(Position.X),
                    FastMath.CreateAbsoluteInt(Position.Y),
                    FastMath.CreateAbsoluteInt(Position.Z),
                    FastMath.CreateRotationByte(Yaw),
                    FastMath.CreateRotationByte(Pitch), 0 /* Note: current item is set through other means */);
            }
        }

        public override Size Size => new Size(Width, Height, Depth);

        public override short MaxHealth => 20;

        public string Username { get; set; }
        public bool IsSprinting { get; set; }
        public bool IsCrouching { get; set; }
        public double PositiveDeltaY { get; set; }

        private Vector3 _OldPosition;
        public Vector3 OldPosition
        {
            get
            {
                return _OldPosition;
            }
            private set
            {
                _OldPosition = value;
            }
        }

        public override Vector3 Position
        {
            get
            {
                return _Position;
            }
            set
            {
                _OldPosition = _Position;
                _Position = value;
                OnPropertyChanged();
            }
        }

        protected short _SelectedSlot;
        public short SelectedSlot
        {
            get { return _SelectedSlot; }
            set
            {
                _SelectedSlot = value;
                OnPropertyChanged();
            }
        }

        public ItemStack ItemInMouse { get; set; }

        protected Vector3 _SpawnPoint;
        public Vector3 SpawnPoint
        {
            get { return _SpawnPoint; }
            set
            {
                _SpawnPoint = value;
                OnPropertyChanged();
            }
        }

        public event EventHandler<EntityEventArgs> PickUpItem;
        public void OnPickUpItem(ItemEntity item)
        {
            PickUpItem?.Invoke(this, new EntityEventArgs(item));
        }
    }
}
