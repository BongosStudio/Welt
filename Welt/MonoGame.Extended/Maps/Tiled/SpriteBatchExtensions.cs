using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.MonoGame.Extended.Shapes;

namespace Welt.MonoGame.Extended.Maps.Tiled
{
    public static class SpriteBatchExtensions
    {
        public static void Draw(this SpriteBatch spriteBatch, TiledMap tiledMap, Rectangle? visibleRectangle = null, GameTime gameTime = null)
        {
            tiledMap.Draw(spriteBatch, visibleRectangle, gameTime);
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledMap tiledMap, Camera2D camera, GameTime gameTime = null)
        {
            tiledMap.Draw(spriteBatch, camera, gameTime);
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledLayer layer, Rectangle? visibleRectangle = null, GameTime gameTime = null)
        {
            layer.Draw(spriteBatch, visibleRectangle, gameTime: gameTime);
        }

        public static void Draw(this SpriteBatch spriteBatch, TiledLayer layer, Camera2D camera, GameTime gameTime = null)
        {
            layer.Draw(spriteBatch, camera.GetBoundingRectangle().ToRectangle(), gameTime: gameTime);
        }
    }
}
