#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Welt.Models;

#endregion

namespace Welt.Cameras
{
    public class FirstPersonCamera : Camera
    {
        public FirstPersonCamera(Viewport viewport) : base(viewport)
        {
        }

        public Vector3 Target { get; private set; }

        public float LeftRightRotation
        {
            get { return m_leftRightRotation; }
            set
            {
                m_leftRightRotation = value;
                CalculateView();
            }
        }

        public float UpDownRotation
        {
            get { return m_upDownRotation; }
            set
            {
                m_upDownRotation = value;
                CalculateView();
            }
        }

        public Vector3 LookVector { get; private set; }

        #region Initialize

        public override void Initialize()
        {
            m_upDownRotation = 0;
            m_leftRightRotation = 0;

            base.Initialize();
        }

        #endregion

        #region CalculateView

        protected override void CalculateView()
        {
            var rotationMatrix = Matrix.CreateRotationX(m_upDownRotation)*Matrix.CreateRotationY(m_leftRightRotation);
            LookVector = Vector3.Transform(Vector3.Forward, rotationMatrix);

            Target = Position + LookVector;

            var cameraRotatedUpVector = Vector3.Transform(Vector3.Up, rotationMatrix);
            View = Matrix.CreateLookAt(Position, Target, cameraRotatedUpVector);

            base.CalculateView();
        }

        #endregion

        public void LookAt(Vector3 target)
        {
            // Doesn't take into account the rotated UP vector
            // Should calculate rotations here!
            View = Matrix.CreateLookAt(Position, target, Vector3.Up);
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            CalculateView();
            base.Update(gameTime);
        }

        #endregion

        #region FacingCardinal

        public Cardinal FacingCardinal()
        {
            //TODO optimize with modulo (see url)
            //http://gamedev.stackexchange.com/questions/7325/snapping-an-angle-to-the-closest-cardinal-direction

            var a = MathHelper.WrapAngle(m_leftRightRotation);
            a = MathHelper.PiOver4*(float) Math.Round(a/MathHelper.PiOver4);

            if (a == 0)
                return (Cardinal.N);
            if (a.CompareTo(MathHelper.PiOver4) == 0)
                return (Cardinal.Nw);
            if (a.CompareTo(-MathHelper.PiOver4) == 0)
                return (Cardinal.Ne);
            if (a.CompareTo(MathHelper.Pi - MathHelper.PiOver4) == 0)
                return (Cardinal.Sw);
            if (a.CompareTo(-(MathHelper.Pi - MathHelper.PiOver4)) == 0)
                return (Cardinal.Se);
            if (a.CompareTo(MathHelper.PiOver2) == 0)
                return (Cardinal.W);
            if (a.CompareTo(-MathHelper.PiOver2) == 0)
                return (Cardinal.E);
            if (a.CompareTo(MathHelper.Pi) == 0 || a.CompareTo(-MathHelper.Pi) == 0)
                return (Cardinal.S);
            throw new NotImplementedException();
        }

        #endregion

        #region Fields

        private const float ROTATION_SPEED = 0.05f;
        private float m_leftRightRotation;
        private float m_upDownRotation;

        #endregion
    }
}