using Microsoft.Xna.Framework;
using Welt.Blocks;
using Welt.Forge;
using Welt.Types;

namespace Welt.Processors.MeshBuilders
{
    public class TorchBuilder : BlockMeshBuilder
    {
        public static void BuildPostVertexList(ushort id, Chunk chunk, Vector3I chunkRelativePosition)
        {

            var blockPosition = chunk.Position + chunkRelativePosition;

            //get signed bytes from these to be able to remove 1 without further casts
            var x = (sbyte)chunkRelativePosition.X;
            var y = (sbyte)chunkRelativePosition.Y;
            var z = (sbyte)chunkRelativePosition.Z;

            BuildPostVertices(chunk, blockPosition, chunkRelativePosition, id, 0.6f, Color.LightGray);
        }

        public const int VertexCount = 32;

        private static void BuildPostVertices(Chunk chunk, Vector3I blockPosition, Vector3I chunkRelativePosition,
            ushort blockType, float sunLight, Color localLight)
        {
            var provider = BlockProvider.GetProvider(blockType);

            var uvList = provider.GetTexture(BlockFaceDirection.XIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.55f, 1, 1), new Vector3(0.55f, 1, 0), new Vector3(0.55f, 0, 1), new Vector3(0.55f, 0, 0) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = provider.GetTexture(BlockFaceDirection.XDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.55f, 1, 0), new Vector3(0.55f, 1, 1), new Vector3(0.55f, 0, 0), new Vector3(0.55f, 0, 1) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.XIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.45f, 1, 1), new Vector3(0.45f, 1, 0), new Vector3(0.45f, 0, 1), new Vector3(0.45f, 0, 0) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = provider.GetTexture(BlockFaceDirection.XDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.XDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0.45f, 1, 0), new Vector3(0.45f, 1, 1), new Vector3(0.45f, 0, 0), new Vector3(0.45f, 0, 1) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.ZIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0, 1, 0.55f), new Vector3(1, 1, 0.55f), new Vector3(0, 0, 0.55f), new Vector3(1, 0, 0.55f) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.ZDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(1, 1, 0.55f), new Vector3(0, 1, 0.55f), new Vector3(1, 0, 0.55f), new Vector3(0, 0, 0.55f) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });

            uvList = provider.GetTexture(BlockFaceDirection.ZIncreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZIncreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(0, 1, 0.45f), new Vector3(1, 1, 0.45f), new Vector3(0, 0, 0.45f), new Vector3(1, 0, 0.45f) },
                new Vector2[] { uvList[0], uvList[1], uvList[5], uvList[2] },
                new short[] { 0, 1, 3, 0, 3, 2 });

            uvList = provider.GetTexture(BlockFaceDirection.ZDecreasing);
            AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.ZDecreasing,
                new float[] { sunLight, sunLight, sunLight, sunLight },
                new Color[] { localLight, localLight, localLight, localLight },
                new Vector3[] { new Vector3(1, 1, 0.45f), new Vector3(0, 1, 0.45f), new Vector3(1, 0, 0.45f), new Vector3(0, 0, 0.45f) },
                new Vector2[] { uvList[0], uvList[1], uvList[2], uvList[5] },
                new short[] { 0, 1, 2, 2, 1, 3 });


            //uvList = TextureHelper.UvMappings[(int)texture * 6 + (int)BlockFaceDirection.YIncreasing];
            //AddPlane(chunk, blockType, blockPosition, chunkRelativePosition, BlockFaceDirection.YIncreasing,
            //    new float[] { sunLight, sunLight, sunLight, sunLight },
            //    new Color[] { localLight, localLight, localLight, localLight },
            //    new Vector3[] { new Vector3(0.55f, 0.45f, 0.55f), new Vector3(0.45f, 0.45f, 0.55f), new Vector3(0.55f, 0.45f, 0.45f), new Vector3(0.45f, 0.45f, 0.45f) },
            //    new Vector2[] { uvList[4], uvList[5], uvList[1], uvList[3] },
            //    new short[] { 3, 2, 0, 3, 0, 1 });
        }
    }
}
