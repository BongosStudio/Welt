using System;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering
{
    public interface ITextureEffect
    {
        Type[] ArgumentTypes { get; }
        void Process(Texture2D texture);
    }
}