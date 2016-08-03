using Microsoft.Xna.Framework;
using Welt.API.Forge;
using Welt.Blocks;

namespace Welt.Forge.Builders
{
    public interface IMeshBuilder
    {
        float MeshWidth { get; }
        float MeshHeight { get; }
        float MeshDepth { get; }

        Mesh CreateMesh(Vector3 position, ushort id, BlockFaceDirection face);
        bool CanBuild(ushort id, byte md);
    }
}