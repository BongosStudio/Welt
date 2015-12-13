#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Controllers;
using Welt.Forge;
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
            _graphicsDevice = graphicsDevice;
            this.Player = player;
            _viewport = graphicsDevice.Viewport;
            Camera = new FirstPersonCamera(_viewport);
            _cameraController = new FirstPersonCameraController(Camera);
            _physics = new PlayerPhysics(this);
        }

        public void Initialize()
        {
            Camera.Initialize();
            Camera.Position = new Vector3(World.Origin*Chunk.SIZE.X, Chunk.SIZE.Y, World.Origin*Chunk.SIZE.Z);
            Player.Position = Camera.Position;
            Camera.LookAt(Vector3.Down);

            _cameraController.Initialize();

            // SelectionBlock
            _selectionBlockEffect = new BasicEffect(_graphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            // SelectionBlock
            SelectionBlock = content.Load<Microsoft.Xna.Framework.Graphics.Model>("Models\\SelectionBlock");
            _selectionBlockTexture = content.Load<Texture2D>("Textures\\SelectionBlock");
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            var previousView = Camera.View;

            if (FreeCam)
            {
                _cameraController.ProcessInput(gameTime);
                Player.Position = Camera.Position;
            }

            _cameraController.Update(gameTime);

            Camera.Update(gameTime);

            //Do not change methods order, its not very clean but works fine
            if (!FreeCam)
                _physics.Move(gameTime);

            //do not do this each tick
            if (!previousView.Equals(Camera.View))
            {
                _lookVector = Camera.LookVector;
                _lookVector.Normalize();

                var waterSelectable = false;
                var x = SetPlayerSelectedBlock(waterSelectable);
                if (x != 0) // x==0 is equivalent to payer.currentSelection == null
                {
                    SetPlayerAdjacentSelectedBlock(x + 0.5f);
                }
            }

            var mouseState = Mouse.GetState();

            var scrollWheelDelta = _previousMouseState.ScrollWheelValue - mouseState.ScrollWheelValue;

            if (mouseState.RightButton == ButtonState.Pressed
                && _previousMouseState.RightButton != ButtonState.Pressed)
            {
                Player.RightTool.Use();
            }

            if (mouseState.LeftButton == ButtonState.Pressed
                && _previousMouseState.LeftButton != ButtonState.Pressed)
            {
                Player.LeftTool.Use();
            }

            Player.RightTool.switchType(scrollWheelDelta);
            Player.LeftTool.switchType(scrollWheelDelta);

            _previousMouseState = Mouse.GetState();
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
        }

        #endregion

        #region Fields

        public readonly Player Player;
        private readonly Viewport _viewport;
        public readonly FirstPersonCamera Camera;
        private readonly FirstPersonCameraController _cameraController;

        private Vector3 _lookVector;

        private MouseState _previousMouseState;

        private readonly GraphicsDevice _graphicsDevice;

        private readonly PlayerPhysics _physics;

        // SelectionBlock
        public Microsoft.Xna.Framework.Graphics.Model SelectionBlock;
        private BasicEffect _selectionBlockEffect;
        private Texture2D _selectionBlockTexture;
        public bool FreeCam;

        #endregion

        #region SelectionBlock

        public void RenderSelectionBlock(GameTime gameTime)
        {
            _graphicsDevice.BlendState = BlendState.NonPremultiplied;
                // allows any transparent pixels in original PNG to draw transparent

            if (!Player.CurrentSelection.HasValue)
            {
                return;
            }

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
            _selectionBlockEffect.World = identity;
            _selectionBlockEffect.View = Camera.View;
            _selectionBlockEffect.Projection = Camera.Projection;
            _selectionBlockEffect.Texture = _selectionBlockTexture;
            _selectionBlockEffect.TextureEnabled = true;

            // apply the effect
            foreach (var pass in _selectionBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawSelectionBlockMesh(_graphicsDevice, SelectionBlock.Meshes[0], _selectionBlockEffect);
            }
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            var count = mesh.MeshParts.Count;
            for (var i = 0; i < count; i++)
            {
                var parts = mesh.MeshParts[i];
                if (parts.NumVertices <= 0) continue;
                _graphicsDevice.Indices = parts.IndexBuffer;
                //TODO better use DrawUserIndexedPrimitives for fully dynamic content
                _graphicsDevice.SetVertexBuffer(parts.VertexBuffer);
                //graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, parts.NumVertices,
                //    parts.StartIndex, parts.PrimitiveCount);
                graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, parts.NumVertices, parts.PrimitiveCount);
            }
        }

        private float SetPlayerSelectedBlock(bool waterSelectable)
        {
            for (var x = 0.5f; x < 8f; x += 0.1f)
            {
                var targetPoint = Camera.Position + (_lookVector*x);

                var block = Player.World.GetBlock(targetPoint);

                if (block.Type != BlockType.None && (waterSelectable || block.Type != BlockType.Water))
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
                var targetPoint = Camera.Position + (_lookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                
                if (Player.World.GetBlock(targetPoint).Type != BlockType.None) continue;
                Player.CurrentSelectedAdjacent = new PositionedBlock(targetPoint, block);
                break;
            }
        }

        #endregion
    }
}