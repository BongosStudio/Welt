using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Welt.API;
using Welt.API.Entities;
using Welt.API.Forge;
using Welt.API.Net;

namespace Welt.Core.Forge
{
    public abstract class ItemProvider : IItemProvider
    {
        public abstract ushort Id { get; }

        public abstract Vector2 GetIconTexture(byte metadata);

        public virtual byte MaximumStack => 64;

        public virtual string DisplayName => string.Empty;

        public virtual void ItemUsedOnEntity(ItemStack item, IEntity usedOn, IWorld world, IRemoteClient user)
        {
            // This space intentionally left blank
        }

        public virtual void ItemUsedOnBlock(Vector3I coordinates, ItemStack item, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            // This space intentionally left blank
        }

        public virtual void ItemUsedOnNothing(ItemStack item, IWorld world, IRemoteClient user)
        {
            // This space intentionally left blank
        }
    }
}