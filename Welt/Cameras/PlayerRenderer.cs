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

#endregion

namespace Welt.Cameras
{
    /*render player and his hands / tools / attached parts */

    public class PlayerRenderer
    {
        public PlayerRenderer(GraphicsDevice graphicsDevice, Player player)
        {
            m_graphicsDevice = graphicsDevice;
            Player = player;
            m_viewport = graphicsDevice.Viewport;
            Camera = new FirstPersonCamera(m_viewport);
            m_cameraController = new FirstPersonCameraController(Camera);
            m_physics = new PlayerPhysics(this);
            m_fog = new FogRenderer(graphicsDevice);
            m_input = InputController.CreateDefault();
        }

        public void Initialize()
        {
            Camera.Initialize();
            Camera.Position = new Vector3(World.Origin*Chunk.Size.X, Chunk.Size.Y, World.Origin*Chunk.Size.Z);
            Player.Position = Camera.Position;
            Camera.LookAt(Vector3.Down);

            m_cameraController.Initialize();

            // SelectionBlock
            m_selectionBlockEffect = new BasicEffect(m_graphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            // SelectionBlock
            SelectionBlock = content.Load<Model>("Models\\SelectionBlock");
            m_selectionBlockTexture = content.Load<Texture2D>("Textures\\SelectionBlock");
            m_fog.LoadContent();
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            var previousView = Camera.View;

            if (FreeCam)
            {
                m_cameraController.ProcessInput(gameTime);
                Player.Position = Camera.Position;
            }

            m_cameraController.Update(gameTime);

            Camera.Update(gameTime);

            //Do not change methods order, its not very clean but works fine
            if (!FreeCam) m_physics.Move(gameTime);

            var mouseState = m_input.GetMouseState();
            var scrollWheelDelta = m_input.GetMouseState().ScrollWheelValue - mouseState.ScrollWheelValue;
            
            if (m_leftClickCooldown > 0) m_leftClickCooldown--;
            if (m_rightClickCooldown > 0) m_rightClickCooldown--;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (m_rightClickCooldown == 0)
                {
                    m_rightClickCooldown = COOLDOWN_TIME;
                    Player.RightTool.Use(DateTime.Now);
                    m_forceUpdate = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (m_leftClickCooldown == 0)
                {
                    m_leftClickCooldown = COOLDOWN_TIME;
                    Player.LeftTool.Use(DateTime.Now);
                    m_forceUpdate = true;
                }
            }

            Player.RightTool.SwitchType(scrollWheelDelta);
            Player.LeftTool.SwitchType(scrollWheelDelta);
            
            //do not do this each tick
            if (!previousView.Equals(Camera.View) || m_forceUpdate)
            {
                m_lookVector = Camera.LookVector;
                m_lookVector.Normalize();

                var x = SetPlayerSelectedBlock(false);
                if (x != 0) // x==0 is equivalent to payer.currentSelection == null
                {
                    SetPlayerAdjacentSelectedBlock(x + 0.5f);
                }
            }
            m_forceUpdate = false;
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
            m_fog.Draw();
        }

        #endregion

        #region Fields

        private const int COOLDOWN_TIME = 30;

        public readonly Player Player;       
        public readonly FirstPersonCamera Camera;
        private readonly FirstPersonCameraController m_cameraController;
        private readonly FogRenderer m_fog;

        private Vector3 m_lookVector;
        private readonly InputController m_input;
        private readonly GraphicsDevice m_graphicsDevice;
        private readonly Viewport m_viewport;
        private readonly PlayerPhysics m_physics;

        // SelectionBlock
        public Model SelectionBlock;
        private BasicEffect m_selectionBlockEffect;
        private Texture2D m_selectionBlockTexture;
        private int m_leftClickCooldown;
        private int m_rightClickCooldown;
        private bool m_forceUpdate;
        public bool FreeCam;

        #endregion

        #region SelectionBlock

        public void RenderSelectionBlock(GameTime gameTime)
        {
            m_graphicsDevice.BlendState = BlendState.NonPremultiplied;
                // allows any transparent pixels in original PNG to draw transparent

            if (!Player.CurrentSelection.HasValue) return;
            //TODO why the +0.5f for rendering slection block ?
            var position = (Vector3) Player.CurrentSelection.Value.Position + new Vector3(0.5f, 0.5f, 0.5f);

            Matrix matrixA, matrixB;
            var identity = Matrix.Identity; // setup the matrix prior to translation and scaling  
            Matrix.CreateTranslation(ref position, out matrixA);
                // translate the position a half block in each direction
            Matrix.CreateScale(0.51f, out matrixB);
                // scales the selection box slightly larger than the targetted block

            identity = Matrix.Multiply(matrixB, matrixA); // the final position of the block

            // set up the World, View and Projection
            m_selectionBlockEffect.World = identity;
            m_selectionBlockEffect.View = Camera.View;
            m_selectionBlockEffect.Projection = Camera.Projection;
            m_selectionBlockEffect.Texture = m_selectionBlockTexture;
            m_selectionBlockEffect.TextureEnabled = true;

            // apply the effect
            foreach (var pass in m_selectionBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawSelectionBlockMesh(m_graphicsDevice, SelectionBlock.Meshes[0], m_selectionBlockEffect);
            }
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            var count = mesh.MeshParts.Count;
            for (var i = 0; i < count; i++)
            {
                var parts = mesh.MeshParts[i];
                if (parts.NumVertices <= 0) continue;
                m_graphicsDevice.Indices = parts.IndexBuffer;
                //TODO better use DrawUserIndexedPrimitives for fully dynamic content
                m_graphicsDevice.SetVertexBuffer(parts.VertexBuffer);
                //graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, parts.NumVertices,
                //    parts.StartIndex, parts.PrimitiveCount);
                graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, parts.NumVertices, parts.PrimitiveCount);
            }
        }

        private float SetPlayerSelectedBlock(bool waterSelectable)
        {
            for (var x = 0.5f; x < 8f; x += 0.1f)
            {
                var targetPoint = Camera.Position + (m_lookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                Console.WriteLine(Block.GetBoundingBox(block.Id, targetPoint).Max.Y > targetPoint.Y);
                if (block.Id != BlockType.None && (waterSelectable || block.Id != BlockType.Water))
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
                var targetPoint = Camera.Position + (m_lookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                
                if (Player.World.GetBlock(targetPoint).Id != BlockType.None) continue;
                Player.CurrentSelectedAdjacent = new PositionedBlock(targetPoint, block);
                break;
            }
        }

        #endregion
    }
}