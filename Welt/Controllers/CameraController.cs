using Microsoft.Xna.Framework;
using Welt.Cameras;

namespace Welt.Controllers
{
    public abstract class CameraController
    {
        public Camera Camera;

        protected CameraController(Camera camera)
        {
            Camera = camera;
        }

        public abstract void ProcessInput(GameTime gameTime);

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
