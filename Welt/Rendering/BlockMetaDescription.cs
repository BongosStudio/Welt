using System.IO;
using Microsoft.Xna.Framework;
using Welt.API.Forge;

namespace Welt.Rendering
{
    public struct BlockMetaDescription
    {
        public Vector2[] Textures;
        public LightStruct LightMeta;
        public int AnimationLengthMeta;
        public AnimationCall AnimationCallMeta;
        public int[] AnimationMeta;
        public double TextureScaleMeta;
        public ITextureEffect[] TextureEffectMeta;

        public static BlockMetaDescription FromFile(string file)
        {
            // TODO
            return new BlockMetaDescription();
        }
    }
}