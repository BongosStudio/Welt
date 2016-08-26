using System;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering.Effects
{
    public class ParticlesTextureEffect : ITextureEffect
    {
        public Type[] ArgumentTypes => new[]
        {
            typeof (string)
        };
        public void Process(Texture2D texture)
        {
            throw new NotImplementedException();
        }
    }
}