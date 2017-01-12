using System;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering.Effects
{
    public class BlendTextureEffect : ITextureEffect
    {
        public Type[] ArgumentTypes => new[]
        {
            typeof (byte[]),
            typeof (byte[]),
            typeof (byte[]),
            typeof (byte[])
        };

        public void Process(Texture2D texture)
        {
            throw new NotImplementedException();
        }
    }
}