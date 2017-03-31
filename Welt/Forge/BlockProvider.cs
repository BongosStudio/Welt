using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Welt.Blocks;

namespace Welt.Forge
{
    public abstract class BlockProvider
    {
        private static Dictionary<ushort, BlockProvider> m_RegisteredProviders = new Dictionary<ushort, BlockProvider>();

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

        public static void CreateProviders()
        {
            foreach (var p in Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsSubclassOf(typeof(BlockProvider))).Select(Activator.CreateInstance)
                .Cast<BlockProvider>())
            {
                m_RegisteredProviders.Add(p.BlockId, p);
            }
        }

        public virtual void PlaceBlock(World world, Vector3 position, Vector3 adjacent, Block block)
        {
            world.SetBlock(adjacent, block);
        }

        public abstract BlockTexture GetTexture(BlockFaceDirection faceDir, ushort blockAbove = 0);
    }
}
