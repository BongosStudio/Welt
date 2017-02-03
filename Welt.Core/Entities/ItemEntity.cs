using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Welt.API;
using Welt.API.Physics;
using Microsoft.Xna.Framework;
using Welt.API.Net;
using Welt.Core.Net.Packets;
using Welt.Core.Extensions;

namespace Welt.Core.Entities
{
    public class ItemEntity : ObjectEntity, IAABBEntity
    {
        public static float PickupRange = 2;

        public ItemEntity(Vector3 position, ItemStack item)
        {
            Position = position;
            Item = item;
            Velocity = new Vector3(
                (float)FastMath.NextRandomDouble() * 0.25f - 0.125f, 
                0.25f, 
                (float)FastMath.NextRandomDouble() * 0.25f - 0.125f);
            if (item.Count == 0)
                Despawned = true;
        }

        public ItemStack Item { get; set; }

        public override IPacket SpawnPacket => null;

        public override Size Size
        {
            get { return new Size(0.25f, 0.25f, 0.25f); }
        }

        public BoundingBox BoundingBox
        {
            get
            {
                return new BoundingBox(Position, Position + new Vector3(Size.Width, Size.Height, Size.Depth));
            }
        }

        public void TerrainCollision(Vector3 collisionPoint, Vector3 collisionDirection)
        {
            if (collisionDirection == Vector3.Down)
            {
                Velocity = Vector3.Zero;
            }
        }

        public override byte EntityType => 2;
        public override int Data => 1;

        public override MetadataDictionary Metadata
        {
            get
            {
                var metadata = base.Metadata;
                metadata[10] = Item;
                return metadata;
            }
        }

        public override bool SendMetadataToClients => false;

        public bool BeginUpdate()
        {
            EnablePropertyChange = false;
            return true;
        }

        public void EndUpdate(Vector3 newPosition)
        {
            EnablePropertyChange = true;
            Position = newPosition;
        }

        public override void Update(IEntityManager entityManager)
        {
            var nearbyEntities = entityManager.EntitiesInRange(Position, PickupRange);
            if ((DateTime.UtcNow - SpawnTime).TotalSeconds > 1)
            {
                var player = nearbyEntities.FirstOrDefault(e => e is PlayerEntity && (e as PlayerEntity).Health != 0
                    && e.Position.DistanceTo(Position) <= PickupRange);
                if (player != null)
                {
                    var playerEntity = player as PlayerEntity;
                    playerEntity.OnPickUpItem(this);
                    entityManager.DespawnEntity(this);
                }
            }
            if ((DateTime.UtcNow - SpawnTime).TotalMinutes > 5)
                entityManager.DespawnEntity(this);
            base.Update(entityManager);
        }

        public float AccelerationDueToGravity => 1.98f;

        public float Drag => 0.4f;

        public float TerminalVelocity => 39.2f;
    }
}
