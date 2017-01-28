using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Welt.Blocks;
using Welt.Graphics;
using Welt.Models;
using Welt.Types;

namespace Welt.Forge
{
    public abstract class BlockProvider
    {
        private static Dictionary<ushort, BlockProvider> m_RegisteredProviders = new Dictionary<ushort, BlockProvider>();

        public static BlockProvider GetProvider(ushort id)
        {
            return m_RegisteredProviders[id];
        }

        /// <summary>
        ///     The ID the provider retrieves info of.
        /// </summary>
        public abstract ushort BlockId { get; }
        /// <summary>
        ///     The block name used for things like texture files and metadata files.
        /// </summary>
        public abstract string BlockName { get; }
        /// <summary>
        ///     The block name used for visual identification.
        /// </summary>
        public abstract string BlockTitle { get; set; }
        
        public virtual bool IsOpaque { get; set; } = false;
        public virtual bool IsSolid { get; set; } = true;
        public virtual bool IsPlantBlock { get; set; } = false;
        public virtual bool HasGravity { get; set; } = false;
        public virtual bool HasCollision { get; set; } = true;
        public virtual float Hardness { get; set; } = 1f;
        public virtual float Resistance { get; set; } = 0.5f;


        public static void CreateProviders()
        {

            foreach (var p in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BlockProvider))).Select(Activator.CreateInstance)
                .Cast<BlockProvider>())
            {
                m_RegisteredProviders.Add(p.BlockId, p);
            }
        }

        public virtual BoundingBox GetBoundingBox(byte metadata)
        {
            return new BoundingBox(Vector3.Zero, Vector3.One);
        }

        public virtual void UseBlock(World world, Vector3 position, Player player)
        {

        }

        public virtual void HitBlock(World world, Vector3 position, Player player)
        {
            if (Hardness > 0)
                world.SetBlock(position, new Block());
        }

        public virtual void PlaceBlock(World world, Vector3 position, Vector3 adjacent, Block block)
        {
            if (block.Id == 0) return;
            if (IsPlantBlock && !m_RegisteredProviders[world.GetBlock(position + Vector3.Down).Id].IsSolid) return;
            if (Block.IsSolidBlock(world.GetBlock(position).Id))
                world.SetBlock(adjacent, block);
            else
                world.SetBlock(position, block);
        }

        public virtual Vector2[] GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0)
        {
            return TextureMap.GetTexture(BlockName, faceDir);
        }

        public virtual Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0)
        {
            return new Vector4();
        }

        public virtual Vector3B GetLightLevel(byte metadata)
        {
            return new Vector3B();
        }
    }
}
