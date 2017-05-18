using Microsoft.Xna.Framework;
using Welt.Cameras;

namespace Welt.Controllers
{
    public class VehicleCameraController : CameraController<VehicleCamera>
    {
        public float Friction;

        public VehicleCameraController(VehicleCamera camera) : base(camera)
        {
        }
    }
}
