#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements


#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace Welt.Forge.Renderers
{
    public interface IRenderer
    {
        void Initialize();
        void Update(GameTime gameTime);
        void Draw(GameTime gameTime);
        void LoadContent(ContentManager content);
        void Stop();
    }
}