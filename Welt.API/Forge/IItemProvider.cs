using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Welt.API.Entities;
using Welt.API.Net;

namespace Welt.API.Forge
{
    public interface IItemProvider
    {
        ushort Id { get; }
        byte MaximumStack { get; }
        string DisplayName { get; }
        void ItemUsedOnNothing(ItemStack item, IWorld world, IRemoteClient user);
        void ItemUsedOnEntity(ItemStack item, IEntity usedOn, IWorld world, IRemoteClient user);
        void ItemUsedOnBlock(Vector3I coordinates, ItemStack item, BlockFaceDirection face, IWorld world, IRemoteClient user);
        Vector2 GetIconTexture(byte metadata);
    }
}
