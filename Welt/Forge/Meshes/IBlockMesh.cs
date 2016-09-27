using Microsoft.Xna.Framework;
using System;

namespace Welt.Game.Builders.Forge.Blocks
{
    public interface IBlockMesh
    {
        // so the only reason I'm implementing it like this is because it'll help speed up accessing each face
        // if we have it pointed to the data explicitly rather than index an array for it. This may also help
        // with persisting meshes because my goal is to only have one mesh per type persisted through the game.
        // the only varying will be textures and lights, hence why it's using vector3s instead of vertexcolor*s.
        // The ChunkMeshBuilder will have a static collection of IBlockMeshes the game is using. From there, it'll
        // create a static instance of each at runtime via reflection. We DO NOT want to make them as we go because 
        // it'll cause more jitter when we first place a block type down and there's no reason to not do it now. 
        // It'll also allow for small debugging. After that, the chunk will determine which faces of each block
        // to render and add the appropriate vertices & indices to the buffers. The buffers will be linked to a
        // local array so that we can modify the contents easier rather than call for the GPU to process the removal
        // of the vertices and indices. After the ChunkMeshBuilder has created the chunk, it'll assign it to an 
        // entity and add it to the scene. Or we can just draw the damn buffers to the graphics context (which I 
        // wouldn't mind at all because there's no sense for object collection).
        // TODO: maybe have normals? It's pretty easy to determine that .Top() normals are 0;1;0 and Left() are 1;0;0. 
        // Instead of having the data passed per the mesh, we could just calculate it on the fly?

        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Top();
        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Bottom();
        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Right();
        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Left();
        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Front();
        (Vector3[] Vertices, byte[] Indices, byte[] Uvs) Back();
    }
}
