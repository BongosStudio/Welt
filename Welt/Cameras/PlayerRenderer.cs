#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Controllers;
using Welt.Forge;
using Welt.Forge.Renderers;
using Welt.Models;
using Welt.Physics;
using Welt.Types;
using Welt.Extensions;

#endregion

namespace Welt.Cameras
{
    /*render player and his hands / tools / attached parts */

    public class PlayerRenderer
    {
        public PlayerRenderer(GraphicsDevice graphicsDevice, Player player)
        {
            m_GraphicsDevice = graphicsDevice;
            Player = player;
            m_Viewport = graphicsDevice.Viewport;
            Camera = new FirstPersonCamera(m_Viewport);
            m_CameraController = new FirstPersonCameraController(Camera);
            m_Physics = new PlayerPhysics(this);
            m_Fog = new FogRenderer(graphicsDevice);
            m_Input = InputController.CreateDefault();
            m_LeftClickCooldown = TimeSpan.Zero;
            m_RightClickCooldown = TimeSpan.Zero;
        }

        public void Initialize()
        {
            Camera.Initialize();
            
            Camera.Position = new Vector3(
                Player.World.Origin.X*Chunk.Size.X, 
                128, 
                Player.World.Origin.Y*Chunk.Size.Z);
            // TODO: change the Y of the spawn position so we don't fall please?
            Player.Position = Camera.Position;
            Camera.LookAt(Vector3.Forward);
            // TODO: load the previous data of position
            m_CameraController.Initialize();

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
            if (!FreeCam)
                m_Physics.Move(gameTime);
            else
            {
                m_CameraController.ProcessInput(gameTime);
                //Camera.Position = new Vector3(Camera.Position.X % Player.World.Size * Chunk.Size.X, Camera.Position.Y, Camera.Position.Z % Player.World.Size * Chunk.Size.Z);
                Player.Position = Camera.Position;
            }

            m_CameraController.Update(gameTime);

            Camera.Update(gameTime);
            

            var mouseState = m_Input.GetMouseState();
            var scrollWheelDelta = m_Input.GetMouseState().ScrollWheelValue - mouseState.ScrollWheelValue;

            // first we go ahead and subtract the amount of elapsed time since last update
            if (m_LeftClickCooldown > TimeSpan.Zero) m_LeftClickCooldown -= gameTime.ElapsedGameTime;
            if (m_RightClickCooldown > TimeSpan.Zero) m_RightClickCooldown -= gameTime.ElapsedGameTime;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (m_RightClickCooldown <= TimeSpan.Zero) // this means the cooldown is finished
                {
                    m_RightClickCooldown = TimeSpan.FromMilliseconds(500);
                    Player.RightClick(gameTime);
                    m_ForceUpdate = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (m_LeftClickCooldown <= TimeSpan.Zero) // this means the cooldown is finished
                {
                    m_LeftClickCooldown = TimeSpan.FromMilliseconds(500);
                    Player.LeftClick(gameTime);
                    m_ForceUpdate = true;
                }
            }
            
            //do not do this each tick
            if (!previousView.Equals(Camera.View) || m_ForceUpdate)
            {
                _mLookVector = Camera.LookVector;
                _mLookVector.Normalize();

                var x = SetPlayerSelectedBlock(false);
                if (x != 0) // x==0 is equivalent to payer.currentSelection == null
                {
                    SetPlayerAdjacentSelectedBlock(x + 0.5f);
                }
            }
            m_ForceUpdate = false;
        }

        #endregion

        #region Draw

        public void Draw(GameTime gameTime)
        {
            //TODO draw the player / 3rd person /  tools

            if (Player.CurrentSelection.HasValue)
            {
                RenderSelectionBlock(gameTime);
            }
            m_Fog.Draw();
        }

        #endregion

        #region Fields
        
        public readonly Player Player;       
        public readonly FirstPersonCamera Camera;
        private readonly FirstPersonCameraController m_CameraController;
        private readonly FogRenderer m_Fog;

        private Vector3 _mLookVector;
        private readonly InputController m_Input;
        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly Viewport m_Viewport;
        private readonly PlayerPhysics m_Physics;

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

            if (!Player.CurrentSelection.HasValue) return;
            //TODO why the +0.5f for rendering slection block ?
            var position = (Vector3) Player.CurrentSelection.Value.Position + new Vector3(0.5f, 0.5f, 0.5f);
            var identity = Matrix.Identity; // setup the matrix prior to translation and scaling  
            Matrix.CreateTranslation(ref position, out var matrixA);
                // translate the position a half block in each direction
            Matrix.CreateScale(0.51f, out var matrixB);
                // scales the selection box slightly larger than the targetted block

            identity = Matrix.Multiply(matrixB, matrixA); // the final position of the block

            // set up the World, View and Projection
            m_SelectionBlockEffect.World = identity;
            m_SelectionBlockEffect.View = Camera.View;
            m_SelectionBlockEffect.Projection = Camera.Projection;
            m_SelectionBlockEffect.Texture = m_SelectionBlockTexture;
            m_SelectionBlockEffect.TextureEnabled = true;

            // apply the effect
            foreach (var pass in m_SelectionBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawSelectionBlockMesh(m_GraphicsDevice, SelectionBlock.Meshes[0], m_SelectionBlockEffect);
            }
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            var count = mesh.MeshParts.Count;
            for (var i = 0; i < count; i++)
            {
                var parts = mesh.MeshParts[i];
                if (parts.NumVertices <= 0) continue;
                m_GraphicsDevice.Indices = parts.IndexBuffer;
                //TODO better use DrawUserIndexedPrimitives for fully dynamic content
                m_GraphicsDevice.SetVertexBuffer(parts.VertexBuffer);
                graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, parts.NumVertices, parts.PrimitiveCount);
            }
        }

        private float SetPlayerSelectedBlock(bool waterSelectable)
        {
            for (var x = 0.5f; x < 8f; x += 0.1f)
            {
                var targetPoint = Camera.Position + (_mLookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                if ((block.Id != BlockType.NONE && (waterSelectable || block.Id != BlockType.WATER)) && 
                    Block.GetBoundingBox(block.Id, targetPoint.Floor()).Contains(targetPoint) == ContainmentType.Contains)
                {
                    Player.CurrentSelection = new PositionedBlock(targetPoint, block);
                    
                    return x;
                }
                Player.CurrentSelection = null;
                Player.CurrentSelectedAdjacent = null;
            }

            return 0;
        }

        private void SetPlayerAdjacentSelectedBlock(float xStart)
        {
            for (var x = xStart; x > 0.7f; x -= 0.1f)
            {
                var targetPoint = Camera.Position + (_mLookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                
                if (Player.World.GetBlock(targetPoint).Id != BlockType.NONE) continue;
                Player.CurrentSelectedAdjacent = new PositionedBlock(targetPoint, block);
                break;
            }
        }

        #endregion
    }
}