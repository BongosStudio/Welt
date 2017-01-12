using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Forge;
using Welt.Types;

namespace Welt.Processors.MeshBuilders
{
    public class GrassBuilder : BlockMeshBuilder
    {
        public static void BuildGrassVertexList(ushort id, Chunk chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte)chunkRelativePosition.X;
            var y = (sbyte)chunkRelativePosition.Y;
            var z = (sbyte)chunkRelativePosition.Z;

            BuildGrassVertices(chunk, blockPosition, chunkRelativePosition, id, 0.6f, Color.LightGray);
        }

        protected static void BuildGrassVertices(Chunk chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            ushort blockType, float sunLight, Color localLight)
        {
            var texture = Block.GetTexture(blockType);

            var uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.XIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 1),
                new Vector3(1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 0),
                new Vector3(1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 1),
                new Vector3(1, 0, 0), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 0),
                new Vector3(1, 0, 0), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.XDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 0),
                new Vector3(-1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 1, 1),
                new Vector3(-1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 0),
                new Vector3(-1, 0, 0), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.3f, 0, 1),
                new Vector3(-1, 0, 0), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.XIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 1),
                new Vector3(1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 0),
                new Vector3(1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 1),
                new Vector3(1, 0, 0), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 0),
                new Vector3(1, 0, 0), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.XDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 0),
                new Vector3(-1, 0, 0), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 1, 1),
                new Vector3(-1, 0, 0), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 0),
                new Vector3(-1, 0, 0), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0.7f, 0, 1),
                new Vector3(-1, 0, 0), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.ZIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.3f),
                new Vector3(0, 0, 1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.3f),
                new Vector3(0, 0, 1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.3f),
                new Vector3(0, 0, 1), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.3f),
                new Vector3(0, 0, 1), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.ZDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.3f),
                new Vector3(0, 0, -1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.3f),
                new Vector3(0, 0, -1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.3f),
                new Vector3(0, 0, -1), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.3f),
                new Vector3(0, 0, -1), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.ZIncreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.7f),
                new Vector3(0, 0, 1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.7f),
                new Vector3(0, 0, 1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.7f),
                new Vector3(0, 0, 1), uvList[5], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.7f),
                new Vector3(0, 0, 1), uvList[2], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 3, 0, 3, 2);

            uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.ZDecreasing];
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 1, 0.7f),
                new Vector3(0, 0, -1), uvList[0], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 1, 0.7f),
                new Vector3(0, 0, -1), uvList[1], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(1, 0, 0.7f),
                new Vector3(0, 0, -1), uvList[2], sunLight, localLight);
            AddVertex(chunk, blockType, blockPosition, chunkRelativePosition, new Vector3(0, 0, 0.7f),
                new Vector3(0, 0, -1), uvList[5], sunLight, localLight);
            AddIndex(chunk, blockType, 0, 1, 2, 2, 1, 3);
        }
    }
}
