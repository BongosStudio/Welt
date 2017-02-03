using System;

namespace Welt.API.Forge
{
    /// <summary>
    /// Providers item providers for a server.
    /// </summary>
    public interface IItemRepository
    {
        /// <summary>
        /// Gets this repository's item provider for the specified item ID. This may return null
        /// if the item ID in question has no corresponding block provider.
        /// </summary>
        IItemProvider GetItemProvider(ushort id);
        /// <summary>
        /// Registers a new item provider. This overrides any existing item providers that use the
        /// same item ID.
        /// </summary>
        void RegisterItemProvider(IItemProvider provider);
    }
}