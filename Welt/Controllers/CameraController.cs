using Microsoft.Xna.Framework;
using Welt.Cameras;

namespace Welt.Controllers
{
    public abstract class CameraController<T> where T : Camera
    {
        public T Camera;

        protected CameraController(T camera)
        {
            Camera = camera;
        }

        public virtual void Initialize()
        {
            Camera.Initialize();
        }

        public virtual void Update(GameTime gameTime)
        {
            Camera.Update(gameTime);
        }
    }
}
