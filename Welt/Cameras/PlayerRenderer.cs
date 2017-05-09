#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using System.Diagnostics;
using Welt.Core;

#endregion

namespace Welt.Cameras
{
    /*render player and his hands / tools / attached parts */

    public class PlayerRenderer
    {
        public PlayerRenderer(GraphicsDevice graphicsDevice)
        {
            m_GraphicsDevice = graphicsDevice;
            m_Viewport = graphicsDevice.Viewport;
            CameraController = new FirstPersonCameraController(new FirstPersonCamera(m_Viewport));
            m_Fog = new FogRenderer(graphicsDevice);
            m_Input = InputController.CreateDefault();
            m_LeftClickCooldown = TimeSpan.Zero;
            m_RightClickCooldown = TimeSpan.Zero;
        }

        public void Initialize()
        {
            CameraController.Initialize();
            CameraController.Camera.Position = Player.Position;
            CameraController.Camera.LookAt(Vector3.Forward);
            // TODO: load the previous data of position

            // SelectionBlock
            m_SelectionBlockEffect = new BasicEffect(m_GraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            // SelectionBlock
            SelectionBlock = content.Load<Model>("Models\\SelectionBlock");
            m_SelectionBlockTexture = content.Load<Texture2D>("Textures\\SelectionBlock");
            m_Fog.LoadContent();
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            CameraController.Update(gameTime);
            if (Player.IsPaused) return; // this is here so we can still process the game while paused.
            //TODO: process input here.
            ProcessInput(gameTime);
            CameraController.Camera.Position = Player.Position;
            
            var mouseState = m_Input.GetMouseState();
            
            m_ForceUpdate = false;
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            // draw over selected block, current item in hand
            //m_Fog.Draw();
        }

        #endregion

        #region Fields

        public MultiplayerClient Player => WeltGame.Instance.Client;
        public FirstPersonCamera Camera => CameraController.Camera;
        public FirstPersonCameraController CameraController { get; }

        private readonly FogRenderer m_Fog;

        private Vector3 m_LookVector;
        private readonly InputController m_Input;
        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly Viewport m_Viewport;

        private const float MOVEMENTSPEED = 0.25f;

        // SelectionBlock
        public Model SelectionBlock;
        private BasicEffect m_SelectionBlockEffect;
        private Texture2D m_SelectionBlockTexture;
        private TimeSpan m_LeftClickCooldown;
        private TimeSpan m_RightClickCooldown;
        private bool m_ForceUpdate;
        public bool FreeCam;

        #endregion

        #region SelectionBlock

        public void RenderSelectionBlock(GameTime gameTime)
        {
            m_GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                // allows any transparent pixels in original PNG to draw transparent

            
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            
        }

        private float SetPlayerSelectedBlock(bool waterSelectable)
        {
            
            return 0;
        }

        private void SetPlayerAdjacentSelectedBlock(float xStart)
        {
            
        }

        #endregion

        private void ProcessInput(GameTime gameTime)
        {
            var moveVector = new Vector3(0, 0, 0);
            var keyState = Keyboard.GetState();
            if (keyState.GetPressedKeys().Length == 0) return;

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
            if (keyState.IsKeyDown(Keys.LeftShift) && Camera.Position.Y > -20 && Camera.Position.Y < 240)
            {
                moveVector += Vector3.Down;
            }
            if (keyState.IsKeyDown(Keys.Space))
            {
                moveVector += Vector3.Up;
            }

            if (moveVector != Vector3.Zero)
            {
                var rotationMatrix = //Matrix.CreateRotationX(Camera.UpDownRotation)*
                                     Matrix.CreateRotationY(Camera.LeftRightRotation);
                var rotatedVector = Vector3.Transform(moveVector, rotationMatrix);
                
                Player.Position += rotatedVector * MOVEMENTSPEED;
            }
            Player.Physics.Update(gameTime.ElapsedGameTime);
        }
    }
}