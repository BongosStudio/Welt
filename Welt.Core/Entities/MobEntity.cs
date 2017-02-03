using System;
using Welt.API.Entities;
using Welt.API;
using Welt.API.Physics;
using Welt.API.Net;
using Welt.Core.Net.Packets;
using Microsoft.Xna.Framework;
using Welt.API.AI;
using Welt.Core.Extensions;
using Welt.Core.AI;

namespace Welt.Core.Entities
{
    public abstract class MobEntity : LivingEntity, IAABBEntity, IMobEntity
    {
        protected MobEntity()
        {
            Speed = 4;
            CurrentState = new WanderState();
        }

        public event EventHandler PathComplete;

        public override IPacket SpawnPacket => null;

        public abstract sbyte MobType { get; }

        public virtual bool Friendly { get { return true; } }

        public virtual void TerrainCollision(Vector3 collisionPoint, Vector3 collisionDirection)
        {
            // This space intentionally left blank
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position, Position + new Vector3(Size.Width, Size.Height, Size.Depth));
            }
        }

        public virtual bool BeginUpdate()
        {
            EnablePropertyChange = false;
            return true;
        }

        public virtual void EndUpdate(Vector3 newPosition)
        {
            EnablePropertyChange = true;
            Position = newPosition;
        }

        public float AccelerationDueToGravity
        {
            get
            {
                return 1.6f;
            }
        }

        public float Drag
        {
            get
            {
                return 0.40f;
            }
        }

        public float TerminalVelocity
        {
            get
            {
                return 78.4f;
            }
        }

        public PathResult CurrentPath { get; set; }

        /// <summary>
        /// Mob's current speed in m/s.
        /// </summary>
        public virtual double Speed { get; set; }

        public IMobState CurrentState { get; set; }

        public void ChangeState(IMobState state)
        {
            CurrentState = state;
        }

        public void Face(Vector3 target)
        {
            var diff = target - Position;
            Yaw = (float)FastMath.RadiansToDegrees(-(Math.Atan2(diff.X, diff.Z) - Math.PI) + Math.PI); // "Flip" over the 180 mark
        }

        public bool AdvancePath(TimeSpan time, bool faceRoute = true)
        {
            var modifier = (float)(time.TotalSeconds * Speed);
            if (CurrentPath != null)
            {
                // Advance along path
                var target = (Vector3)CurrentPath.Waypoints[CurrentPath.Index];
                target.Y = Position.Y; // TODO: Find better way of doing this
                if (faceRoute)
                    Face(target);
                var lookAt = Vector3.Forward.Transform(Matrix.CreateRotationY((float)FastMath.ToRadians(-(Yaw - 180) + 180)));
                lookAt *= modifier;
                Velocity = new Vector3(lookAt.X, Velocity.Y, lookAt.Z);
                if (Position.DistanceTo(target) < 0.1)
                {
                    CurrentPath.Index++;
                    if (CurrentPath.Index >= CurrentPath.Waypoints.Count)
                    {
                        CurrentPath = null;
                        PathComplete?.Invoke(this, null);
                        return true;
                    }
                }
            }
            return false;
        }

        public override void Update(IEntityManager entityManager)
        {
            if (CurrentState != null)
                CurrentState.Update(this, entityManager);
            else
                AdvancePath(entityManager.TimeSinceLastUpdate);
            base.Update(entityManager);
        }
    }
}

