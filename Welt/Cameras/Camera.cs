#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements


#endregion

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Cameras
{
    public abstract class Camera
    {
        protected Camera(Viewport viewport)
        {
            this.Viewport = viewport;
        }

        public Matrix View { get; protected set; }
        public Matrix Projection { get; protected set; }

        public Vector3 Position
        {
            get { return _position; }
            set
            {
                _position = value;

                CalculateView();
            }
        }

        protected virtual void CalculateProjection()
        {
            Projection = Matrix.CreatePerspectiveFieldOfView(_viewAngle, Viewport.AspectRatio, _nearPlane, _farPlane);
        }

        protected virtual void CalculateView()
        {
        }

        public virtual void Initialize()
        {
            CalculateView();
            CalculateProjection();
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        #region Fields

        protected Vector3 _position = Vector3.Zero;

        private readonly float _viewAngle = MathHelper.PiOver4;
        private readonly float _nearPlane = 0.01f;
        private readonly float _farPlane = 220*4;

        public readonly Viewport Viewport;

        #endregion
    }
}