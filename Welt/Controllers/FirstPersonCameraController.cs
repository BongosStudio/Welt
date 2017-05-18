#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using Welt.Cameras;

#endregion

namespace Welt.Controllers
{
    public class FirstPersonCameraController : CameraController<FirstPersonCamera>
    {
        #region Fields

        private const float MOVEMENTSPEED = 0.25f;
        private const float ROTATIONSPEED = 0.1f;
        private const float HEADBOB = 2.0f;

        private MouseState m_MouseMoveState;
        private MouseState m_MouseState;
        private readonly object m_MouseLock = new object();
        
        public bool ProcessesInput { get; set; }
        
        public static MouseState DefaultMouseState
            =>
                new MouseState(
                    WeltGame.Width/2,
                    WeltGame.Height/2,
                    0,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released,
                    ButtonState.Released
                    );
        
        #endregion

        public FirstPersonCameraController(FirstPersonCamera camera) : base(camera)
        {
        }

        public override void Initialize()
        {
            m_MouseState = Mouse.GetState();
            ProcessesInput = true;
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            lock (m_MouseLock)
            {
                var currentMouseState = Mouse.GetState();
                if (currentMouseState == DefaultMouseState) return;

                float mouseDx = currentMouseState.X - m_MouseMoveState.X;
                float mouseDy = currentMouseState.Y - m_MouseMoveState.Y;

                if (mouseDx != 0)
                {
                    Camera.LeftRightRotation -= ROTATIONSPEED*(mouseDx/50)* Camera.HorizontalLookSensitivity;
                }
                if (mouseDy != 0)
                {
                    Camera.UpDownRotation -= ROTATIONSPEED*(mouseDy/50)* Camera.VerticalLookSensitivity;

                    // Locking camera rotation vertically between +/- 180 degrees
                    var newPosition = Camera.UpDownRotation - ROTATIONSPEED * (mouseDy / 50);
                    if (newPosition < -1.55f)
                        newPosition = -1.55f;
                    else if (newPosition > 1.55f)
                        newPosition = 1.55f;
                    Camera.UpDownRotation = newPosition;
                    // End of locking
                }
                //camera.LeftRightRotation -= GamePad.GetState(Game.ActivePlayerIndex).ThumbSticks.Right.X / 20;
                //camera.UpDownRotation += GamePad.GetState(Game.ActivePlayerIndex).ThumbSticks.Right.Y / 20;

                m_MouseMoveState = new MouseState(Camera.Viewport.Width / 2,
                    Camera.Viewport.Height / 2,
                    0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released,
                    ButtonState.Released);

                Mouse.SetPosition(m_MouseMoveState.X, m_MouseMoveState.Y);
                m_MouseState = Mouse.GetState();
            }
            base.Update(gameTime);
        }
        #endregion
    }
}