using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Welt.API;
using Welt.API.Entities;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class BlockRepository : IBlockRepository, IBlockPhysicsProvider
    {
        private readonly IBlockProvider[] m_BlockProviders = new IBlockProvider[ushort.MaxValue];

        public IBlockProvider GetBlockProvider(ushort id)
        {
            return m_BlockProviders[id];
        }

        public void RegisterBlockProvider(IBlockProvider provider)
        {
            m_BlockProviders[provider.Id] = provider;
        }

        public void DiscoverBlockProviders()
        {
            var providerTypes = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(t =>
                    typeof(IBlockProvider).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    providerTypes.Add(type);
                }
            }

            providerTypes.ForEach(t =>
            {
                var instance = (IBlockProvider)Activator.CreateInstance(t);
                RegisterBlockProvider(instance);
            });
        }
            
        public BoundingBox? GetBoundingBox(IWorld world, Vector3I coordinates)
        {
            // TODO: Block-specific bounding boxes
            var block = world.GetBlock(coordinates);
            if (block.Id == 0) return null;
            var provider = m_BlockProviders[block.Id];
            return provider.GetBoundingBox(block.Metadata);
        }
    }
}