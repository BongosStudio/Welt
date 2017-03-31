using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Components
{
    public interface IVisualComponent : ILogicComponent
    {
        GraphicsDevice Graphics { get; }

        void LoadContent(ContentManager content);
        void Draw(GameTime gameTime);
    }
}
