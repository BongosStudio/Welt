using System;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering.Effects
{
    public class NoiseTextureEffect : ITextureEffect
    {
        public Type[] ArgumentTypes => new[]
        {
            typeof (double), // scale
            typeof (double) // frequency
        };

        public void Process(Texture2D texture)
        {
            
        }
    }
}