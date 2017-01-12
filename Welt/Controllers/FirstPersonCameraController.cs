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

        private MouseState _mMouseMoveState;
        private MouseState _mMouseState;
        private readonly object _mMouseLock = new object();

        public readonly FirstPersonCamera Camera;
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

        public FirstPersonCameraController(FirstPersonCamera camera)
        {
            this.Camera = camera;
        }

        public void Initialize()
        {
            _mMouseState = Mouse.GetState();
            ProcessesInput = true;
        }

        #region ProcessInput
        public void ProcessInput(GameTime gameTime)
        {
            //PlayerIndex activeIndex;
            if (!ProcessesInput) return;

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
                var rotationMatrix = Matrix.CreateRotationX(Camera.UpDownRotation)*
                                     Matrix.CreateRotationY(Camera.LeftRightRotation);
                var rotatedVector = Vector3.Transform(moveVector, rotationMatrix);
                Camera.Position += rotatedVector*MOVEMENTSPEED;
            }
        }
        #endregion

        #region Update

        public void Update(GameTime gameTime)
        {
            lock (_mMouseLock)
            {
                // BUG: for some reason, mouse movement eventually slows down. I'd like to figure out why and if it has
                // anything to do with MonoGame, to try another method.
                var currentMouseState = Mouse.GetState();
                if (currentMouseState == DefaultMouseState) return;

                float mouseDx = currentMouseState.X - _mMouseMoveState.X;
                float mouseDy = currentMouseState.Y - _mMouseMoveState.Y;

                if (mouseDx != 0)
                {
                    Camera.LeftRightRotation -= ROTATIONSPEED*(mouseDx/50)*Camera.HorizontalLookSensitivity;
                }
                if (mouseDy != 0)
                {
                    Camera.UpDownRotation -= ROTATIONSPEED*(mouseDy/50)*Camera.VerticalLookSensitivity;

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

                _mMouseMoveState = new MouseState(Camera.Viewport.Width / 2,
                    Camera.Viewport.Height / 2,
                    0, ButtonState.Released, ButtonState.Released, ButtonState.Released, ButtonState.Released,
                    ButtonState.Released);

                Mouse.SetPosition(_mMouseMoveState.X, _mMouseMoveState.Y);
                _mMouseState = Mouse.GetState();
            }          
        }
        #endregion

    }
}