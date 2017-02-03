using System;
using System.Collections.Generic;
using System.Linq;
using Welt.API.Forge;

namespace Welt.Core.Forge
{
    public class ItemRepository : IItemRepository
    {
        public ItemRepository()
        {
            m_ItemProviders = new List<IItemProvider>();
        }

        private readonly List<IItemProvider> m_ItemProviders = new List<IItemProvider>();

        public IItemProvider GetItemProvider(ushort Id)
        {
            // TODO: Binary search
            for (int i = 0; i < m_ItemProviders.Count; i++)
            {
                if (m_ItemProviders[i].Id == Id)
                    return m_ItemProviders[i];
            }
            return null;
        }

        public void RegisterItemProvider(IItemProvider provIder)
        {
            int i;
            for (i = m_ItemProviders.Count - 1; i >= 0; i--)
            {
                if (provIder.Id == m_ItemProviders[i].Id)
                {
                    m_ItemProviders[i] = provIder; // OverrIde
                    return;
                }
                if (m_ItemProviders[i].Id < provIder.Id)
                    break;
            }
            m_ItemProviders.Insert(i + 1, provIder);
        }

        public void DiscoverItemProviders()
        {
            var provIderTypes = new List<Type>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes().Where(t =>
                    typeof(IItemProvider).IsAssignableFrom(t) && !t.IsAbstract))
                {
                    provIderTypes.Add(type);
                }
            }

            provIderTypes.ForEach(t =>
            {
                var instance = (IItemProvider)Activator.CreateInstance(t);
                RegisterItemProvider(instance);
            });
        }
    }
}