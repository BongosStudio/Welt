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
            var uvList = Block.GetTexture(blockType, BlockFaceDirection.XIncreasing);
            
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.3f, 1, 1), new Vector3(0.3f, 1, 0), new Vector3(0.3f, 0, 1), new Vector3(0.3f, 0, 0) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.XDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.3f, 1, 0), new Vector3(0.3f, 1, 1), new Vector3(0.3f, 0, 0), new Vector3(0.3f, 0, 1) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.XIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.7f, 1, 1), new Vector3(0.7f, 1, 0), new Vector3(0.7f, 0, 1), new Vector3(0.7f, 0, 0) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.XDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.7f, 1, 0), new Vector3(0.7f, 1, 1), new Vector3(0.7f, 0, 0), new Vector3(0.7f, 0, 1) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.ZIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0, 1, 0.3f), new Vector3(1, 1, 0.3f), new Vector3(0, 0, 0.3f), new Vector3(1, 0, 0.3f) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.ZDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(1, 1, 0.3f), new Vector3(0, 1, 0.3f), new Vector3(1, 0, 0.3f), new Vector3(0, 0, 0.3f) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.ZIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0, 1, 0.7f), new Vector3(1, 1, 0.7f), new Vector3(0, 0, 0.7f), new Vector3(1, 0, 0.7f) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = Block.GetTexture(blockType, BlockFaceDirection.ZDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(1, 1, 0.7f), new Vector3(0, 1, 0.7f), new Vector3(1, 0, 0.7f), new Vector3(0, 0, 0.7f) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });
        }
    }
}
