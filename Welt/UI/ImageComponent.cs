#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Color = Microsoft.Xna.Framework.Color;

namespace Welt.UI
{
    public class ImageComponent : UIComponent
    {
        public string File { get; }
        private readonly SpriteBatch _sprite;
        private Texture2D _image;

        public ImageComponent(string file, string name, GraphicsDevice device) : this(file, name, -2, -2, device)
        {
            
        }

        public ImageComponent(string file, string name, int width, int height, GraphicsDevice device)
            : this(file, name, width, height, null, device)
        {

        }

        public ImageComponent(string file, string name, int width, int height, UIComponent parent, GraphicsDevice device)
            : base(name, width, height, parent, device)
        {
            File = file;
            _sprite = new SpriteBatch(device);
        }

        public override void Initialize()
        {
            //base.Initialize();
            IsActive = true;
            _image = WeltGame.Instance.Content.Load<Texture2D>(File);
            Width = _image.Width;
            Height = _image.Height;
            ProcessArea();
        }

        public override void Draw(GameTime time)
        {
            base.Draw(time);
            _sprite.Begin();
            _sprite.Draw(_image, new Vector2(X, Y), Color.FromNonPremultiplied(255, 255, 255, (int) (Opacity*255)));
            _sprite.End();
        }
    }
}