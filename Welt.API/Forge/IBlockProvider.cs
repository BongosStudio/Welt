using Microsoft.Xna.Framework;
using System;
using Welt.API.Net;
using Welt.API.Physics;

namespace Welt.API.Forge
{
    public interface IBlockProvider : IItemProvider
    {
        ushort Id { get; }
        float BlastResistance { get; }
        float Hardness { get; }
        float Density { get; }
        bool IsOpaque { get; }
        bool WillRenderSameNeighbor { get; }
        bool IsSolid { get; }
        bool WillRenderOpaque { get; }
        byte LightOpacity { get; }
        bool WillDiffuseSkyLight { get; }
        bool IsFlammable { get; }
        string Name { get; }
        string DisplayName { get; }
        BlockEffect DisplayEffect { get; }
        Vector2[] GetTexture(BlockFaceDirection face, byte metadata = 0, ushort blockAbove = 0);

        BoundingBox? GetBoundingBox(byte metadata);
        Vector3B GetLightLevel(byte metadata);
        Vector4 GetOverlay(BlockFaceDirection face, ushort blockAbove = 0);
        void GenerateDropEntity(BlockDescriptor descriptor, IWorld world, IMultiplayerServer server, ItemStack heldItem);
        void BlockHit(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user);
        bool BlockInteractedWith(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user);
        void BlockPlaced(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user);
        void BlockMined(BlockDescriptor descriptor, BlockFaceDirection face, IWorld world, IRemoteClient user);
        void BlockUpdate(BlockDescriptor descriptor, BlockDescriptor source, IMultiplayerServer server, IWorld world);
        void BlockLoadedFromChunk(Vector3I coords, IMultiplayerServer server, IWorld world);

        event EventHandler<PlayerCollidedEventArgs> PlayerCollide;
        void OnPlayerCollide(IAABBEntity entity, Vector3I collisionPoint);
    }
}