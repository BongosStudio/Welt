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

#endregion

namespace Welt.Cameras
{
    /*render player and his hands / tools / attached parts */

    public class PlayerRenderer
    {

        public PlayerRenderer(GraphicsDevice graphicsDevice, MultiplayerClient player)
        {
            m_GraphicsDevice = graphicsDevice;
            Player = player;
            m_Viewport = graphicsDevice.Viewport;
            Camera = new FirstPersonCamera(m_Viewport);
            m_CameraController = new FirstPersonCameraController(Camera);
            m_Fog = new FogRenderer(graphicsDevice);
            m_Input = InputController.CreateDefault();
            m_LeftClickCooldown = TimeSpan.Zero;
            m_RightClickCooldown = TimeSpan.Zero;
        }

        public void Initialize()
        {
            m_CameraController.Initialize();
            Camera.Position = Player.Position;
            Camera.LookAt(Vector3.Forward);
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
            var previousView = Camera.View;
            //TODO: process input here.
            var keyState = m_Input.GetKeyboardState();
            var moveVector = new Vector3();
            if (keyState.IsKeyDown(Keys.W))
                moveVector += new Vector3(0, 0, 0.2f);
            if (keyState.IsKeyDown(Keys.S))
                moveVector += new Vector3(0, 0, -0.2f);
            if (keyState.IsKeyDown(Keys.A))
                moveVector += new Vector3(-0.2f, 0, 0);
            if (keyState.IsKeyDown(Keys.D))
                moveVector += new Vector3(0.2f, 0, 0);
            if (keyState.IsKeyDown(Keys.Space))
                moveVector += new Vector3(0, 0.2f, 0);
            if (keyState.IsKeyDown(Keys.LeftShift))
                moveVector += new Vector3(0, -0.2f, 0);
            Player.Position += moveVector;
            Camera.Position = Player.Position;
            m_CameraController.Update(gameTime);
            Camera.Update(gameTime);
            if (Player.IsPaused) return; // this is here so we can still process the game while paused.

            

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
        
        public readonly MultiplayerClient Player;       
        public readonly FirstPersonCamera Camera;
        private readonly FirstPersonCameraController m_CameraController;
        private readonly FogRenderer m_Fog;

        private Vector3 m_LookVector;
        private readonly InputController m_Input;
        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly Viewport m_Viewport;

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
    }
}