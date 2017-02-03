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
            
            Camera.Position = new Vector3(
                Player.World.GetSpawnPoint().X, 
                Player.World.GetSpawnPoint().Y, 
                Player.World.GetSpawnPoint().Z);
            // TODO: change the Y of the spawn position so we don't fall please?
            Player.Position = Camera.Position;
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

            if (Player.IsPaused) return; // this is here so we can still process the game while paused.
            m_CameraController.ProcessInput(gameTime);
            //Camera.Position = new Vector3(Camera.Position.X % Player.World.Size * Chunk.Size.X, Camera.Position.Y, Camera.Position.Z % Player.World.Size * Chunk.Size.Z);
            Player.Position = Camera.Position;

            m_CameraController.Update(gameTime);
            Camera.Update(gameTime);
            

            var mouseState = m_Input.GetMouseState();
            
            m_ForceUpdate = false;
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            // draw over selected block, current item in hand
            m_Fog.Draw();
        }

        #endregion

        #region Fields
        
        public readonly MultiplayerClient Player;       
        public readonly Camera Camera;
        private readonly CameraController m_CameraController;
        private readonly FogRenderer m_Fog;

        private Vector3 _mLookVector;
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