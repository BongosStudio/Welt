using Microsoft.Xna.Framework.Graphics;

namespace Welt.MonoGame.Extended.SceneGraphs
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, SceneGraph sceneGraph)
        {
            sceneGraph.Draw(spriteBatch);
        }
    }
}