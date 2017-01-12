using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Rendering.Effects
{
    public class OpacityTextureEffect : ITextureEffect
    {
        public Type[] ArgumentTypes => new[]
        {
            typeof (double)
        };

        public void Process(Texture2D texture)
        {
            throw new NotImplementedException();
        }
    }
}
