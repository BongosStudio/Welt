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
        public static FirstPersonCamera Instance;

        public FirstPersonCamera(Viewport viewport) : base(viewport)
        {
            Instance = this;
        }

        public Vector3 Target { get; private set; }

        public float LeftRightRotation
        {
            get { return _mLeftRightRotation; }
            set
            {
                _mLeftRightRotation = value;
                CalculateView();
            }
        }

        public float UpDownRotation
        {
            get { return _mUpDownRotation; }
            set
            {
                _mUpDownRotation = value;
                CalculateView();
            }
        }

        public Vector3 LookVector { get; private set; }
        public float VerticalLookSensitivity { get; set; } = 0.8f;
        public float HorizontalLookSensitivity { get; set; } = 0.8f;

        #region Initialize

        public override void Initialize()
        {
            _mUpDownRotation = 0;
            _mLeftRightRotation = 0;

            base.Initialize();
        }

        #endregion

        #region CalculateView

        protected override void CalculateView()
        {
            var rotationMatrix = Matrix.CreateRotationX(_mUpDownRotation)*Matrix.CreateRotationY(_mLeftRightRotation);
            LookVector = Vector3.Transform(Vector3.Forward, rotationMatrix);

            Target = Position + LookVector;

            var cameraRotatedUpVector = Vector3.Transform(Vector3.Up, rotationMatrix);
            View = Matrix.CreateLookAt(Position, Target, cameraRotatedUpVector);

            var invertPos = Position;
            invertPos.Y = -Position.Y + (-Position.Y * 20f); // TODO: add height*2f
            var lookAt = invertPos + Vector3.Transform(Vector3.Forward, rotationMatrix);
            ReflectionViewMatrix = Matrix.CreateLookAt(invertPos, lookAt, cameraRotatedUpVector);
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

            var a = MathHelper.WrapAngle(_mLeftRightRotation);
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
        private float _mLeftRightRotation;
        private float _mUpDownRotation;

        #endregion
    }
}