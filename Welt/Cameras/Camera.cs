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
            Viewport = viewport;
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

        protected virtual Matrix CalculateProjection()
        {
            return Matrix.CreatePerspectiveFieldOfView(m_ViewAngle, Viewport.AspectRatio, m_NearPlane, m_FarPlane);
        }

        protected virtual void CalculateView()
        {
        }

        public virtual void Initialize()
        {
            CalculateView();
            Projection = CalculateProjection();
        }

        public virtual void Update(GameTime gameTime)
        {
        }

        public void LookAt(Vector3 target)
        {
            // Doesn't take into account the rotated UP vector
            // Should calculate rotations here!
            View = Matrix.CreateLookAt(Position, target, Vector3.Up);
        }

        public void LookAt(Matrix view)
        {
            View = view;
        }

        /// <summary>
        /// Applies this camera to the specified effect.
        /// </summary>
        /// <param name="effect">The effect to apply this camera to.</param>
        public void ApplyTo(IEffectMatrices effectMatrices)
        {
            Projection = CalculateProjection();
            effectMatrices.View = View;
            effectMatrices.Projection = Projection;
        }

        public void ApplyTo(Effect effect)
        {
            Projection = CalculateProjection();
            effect.Parameters["View"].SetValue(View);
            effect.Parameters["Projection"].SetValue(Projection);
        }

        #region Fields

        protected Vector3 _position = Vector3.Zero;

        protected readonly float m_ViewAngle = MathHelper.PiOver4;
        protected readonly float m_NearPlane = 0.01f;
        protected readonly float m_FarPlane = 220*4;

        public readonly Viewport Viewport;

        #endregion
    }
}