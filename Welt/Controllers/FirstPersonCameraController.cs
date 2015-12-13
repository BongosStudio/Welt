#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Welt.Cameras;

#endregion

namespace Welt.Controllers
{
    public class FirstPersonCameraController
    {
        #region Fields

        private const float MOVEMENTSPEED = 0.25f;
        private const float ROTATIONSPEED = 0.1f;

        private MouseState _mouseMoveState;
        private MouseState _mouseState;

        private readonly FirstPersonCamera _camera;

        #endregion

        public FirstPersonCameraController(FirstPersonCamera camera)
        {
            this._camera = camera;
        }

        public void Initialize()
        {
            _mouseState = Mouse.GetState();
        }

        #region ProcessInput
        public void ProcessInput(GameTime gameTime)
        {
            //PlayerIndex activeIndex;

            var moveVector = new Vector3(0, 0, 0);
            var keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.W))
            {
                moveVector += Vector3.Forward;
            }
            if (keyState.IsKeyDown(Keys.S))
            {
                moveVector += Vector3.Backward;
            }
            if (keyState.IsKeyDown(Keys.A))
            {
                moveVector += Vector3.Left;
            }
            if (keyState.IsKeyDown(Keys.D))
            {
                moveVector += Vector3.Right;
            }

            if (moveVector != Vector3.Zero)
            {
                var rotationMatrix = Matrix.CreateRotationX(_camera.UpDownRotation) * Matrix.CreateRotationY(_camera.LeftRightRotation);
                var rotatedVector = Vector3.Transform(moveVector, rotationMatrix);
                _camera.Position += rotatedVector * MOVEMENTSPEED;
            }
        }
        #endregion

        #region Update
        public void Update(GameTime gameTime)
        {
            var currentMouseState = Mouse.GetState();

            float mouseDx = currentMouseState.X - _mouseMoveState.X;
            float mouseDy = currentMouseState.Y - _mouseMoveState.Y;

            if (mouseDx != 0)
            {
                _camera.LeftRightRotation -= ROTATIONSPEED * (mouseDx / 50);
            }
            if (mouseDy != 0)
            {
                _camera.UpDownRotation -= ROTATIONSPEED * (mouseDy / 50);

                // Locking camera rotation vertically between +/- 180 degrees
                var newPosition = _camera.UpDownRotation - ROTATIONSPEED * (mouseDy / 50);  
                if (newPosition < -1.55f)  
                    newPosition = -1.55f;  
                else if (newPosition > 1.55f)  
                    newPosition = 1.55f;  
                _camera.UpDownRotation = newPosition;  
                // End of locking
            }

            //camera.LeftRightRotation -= GamePad.GetState(Game.ActivePlayerIndex).ThumbSticks.Right.X / 20;
            //camera.UpDownRotation += GamePad.GetState(Game.ActivePlayerIndex).ThumbSticks.Right.Y / 20;

            _mouseMoveState = new MouseState(_camera.Viewport.Width / 2,
                    _camera.Viewport.Height / 2,
                    0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released);

            Mouse.SetPosition((int)_mouseMoveState.X, (int)_mouseMoveState.Y);
            _mouseState = Mouse.GetState();
        }
        #endregion

    }
}