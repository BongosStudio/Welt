using Microsoft.Xna.Framework;

namespace Welt.Graphics
{
    public struct TextureCell
    {
        public Vector2 Location;
        public string Name;

        public TextureCell(Vector2 location, string name)
        {
            Location = location;
            Name = name;
        }
    }
}
