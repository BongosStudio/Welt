using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Welt.API;
using Welt.API.Forge;
using ApiBlock = Welt.API.Forge.Block;
using Welt.API.Entities;
using Welt.API.Net;

namespace Welt.Core.Forge
{
    public abstract class BlockProvider : IBlockProvider
    {
        public static IBlockRepository BlockRepository { get; set; }
        public static IItemRepository ItemRepository { get; set; }

        public abstract ushort Id { get; }

        public virtual float BlastResistance { get; } = 1f;

        public virtual float Hardness { get; set; } = 1f;

        public virtual bool IsOpaque => true;
        public virtual bool WillRenderSameNeighbor => !IsOpaque;
        public virtual bool IsSolid => true;

        public virtual bool WillRenderOpaque => IsOpaque;

        public virtual byte LightOpacity => 0;

        public virtual bool WillDiffuseSkyLight => false;

        public virtual bool IsFlammable => false;

        public abstract string Name { get; }
        public abstract string DisplayName { get; }
        public virtual BlockEffect DisplayEffect => BlockEffect.None;

        ushort IItemProvider.Id => Id;

        public byte MaximumStack => 64;

        public virtual BoundingBox? GetBoundingBox(byte metadata)
        {
            return new BoundingBox(Vector3.Zero, Vector3.One);
        }

        public virtual Vector2[] GetTexture(BlockFaceDirection faceDir, byte metadata = 0, ushort blockAbove = 0)
        {
            return TextureMap.GetTexture(Name, faceDir);
        }

        public virtual Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            return new Vector4();
        }

        public virtual Vector3B GetLightLevel(byte metadata)
        {
            return new Vector3B();
        }

        public virtual void GenerateDropEntity(BlockDescriptor descriptor, IWorld world, IMultiplayerServer server, ItemStack heldItem)
        {
            throw new NotImplementedException();
        }

        public virtual void BlockHit(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            // put out fire? Idk
        }

        public virtual bool BlockInteractedWith(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            return false;
        }

        public virtual void BlockPlaced(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            
        }

        public virtual void BlockMined(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            
        }

        public virtual void BlockUpdate(BlockDescriptor descriptor, BlockDescriptor source, IMultiplayerServer server, IWorld world)
        {
            
        }

        public virtual void BlockLoadedFromChunk(Vector3I coords, IMultiplayerServer server, IWorld world)
        {
            
        }

        public virtual void ItemUsedOnNothing(ItemStack item, IWorld world, IRemoteClient user)
        {
            
        }

        public virtual void ItemUsedOnEntity(ItemStack item, IEntity usedOn, IWorld world, IRemoteClient user)
        {
            
        }

        public virtual void ItemUsedOnBlock(Vector3I coordinates, ItemStack item, BlockFaceDirection face, IWorld world, IRemoteClient user)
        {
            
        }

        public Vector2 GetIconTexture(byte metadata)
        {
            return new Vector2(-1);
        }

        /// <summary>
        /// Gets the time required to mine the given block with the given item.
        /// </summary>
        /// <returns>The harvest time in milliseconds.</returns>
        /// <param name="Id">Block identifier.</param>
        /// <param name="itemId">Item identifier.</param>
        /// <param name="damage">Damage sustained by the item.</param>
        public static int GetHarvestTime(ushort Id, ushort itemId, out short damage)
        {
            damage = 0;

            var block = BlockRepository.GetBlockProvider(Id);
            var item = ItemRepository.GetItemProvider(itemId);

            double hardness = block.Hardness;
            if (hardness == -1)
                return -1;

            double time = hardness * 1.5;

            // TODO: tools

            return (int)(time * 1000);
        }
    }
}
