﻿#region Copyright
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
using Welt.UI;
<<<<<<< HEAD
using Welt.UI.Components;
=======
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5

#endregion

namespace Welt.Cameras
{
    /*render player and his hands / tools / attached parts */

    public class PlayerRenderer
    {
        public PlayerRenderer(GraphicsDevice graphicsDevice, Player player)
        {
            _mGraphicsDevice = graphicsDevice;
            Player = player;
            _mViewport = graphicsDevice.Viewport;
            Camera = new FirstPersonCamera(_mViewport);
            _mCameraController = new FirstPersonCameraController(Camera);
            _mPhysics = new PlayerPhysics(this);
            _mFog = new FogRenderer(graphicsDevice);
            _mInput = InputController.CreateDefault();
            _mLeftClickCooldown = TimeSpan.Zero;
            _mRightClickCooldown = TimeSpan.Zero;
        }

        public void Initialize()
        {
            Camera.Initialize();
            Camera.Position = new Vector3(World.Origin*Chunk.Size.X, Chunk.Size.Y, World.Origin*Chunk.Size.Z);
            Player.Position = Camera.Position;
            Camera.LookAt(Vector3.Down);

            _mCameraController.Initialize();

            // SelectionBlock
            _mSelectionBlockEffect = new BasicEffect(_mGraphicsDevice);
        }

        public void LoadContent(ContentManager content)
        {
            // SelectionBlock
            SelectionBlock = content.Load<Model>("Models\\SelectionBlock");
            _mSelectionBlockTexture = content.Load<Texture2D>("Textures\\SelectionBlock");
            _mFog.LoadContent();
        }

        #region Update

        public void Update(GameTime gameTime)
        {
            if (Player.IsPaused) return;
            var previousView = Camera.View;

            if (FreeCam)
            {
                _mCameraController.ProcessInput(gameTime);
                Player.Position = Camera.Position;
            }

            _mCameraController.Update(gameTime);

            Camera.Update(gameTime);

            //Do not change methods order, its not very clean but works fine
            if (!FreeCam) _mPhysics.Move(gameTime);

            var mouseState = _mInput.GetMouseState();
            var scrollWheelDelta = _mInput.GetMouseState().ScrollWheelValue - mouseState.ScrollWheelValue;

            // first we go ahead and subtract the amount of elapsed time since last update
            if (_mLeftClickCooldown > TimeSpan.Zero) _mLeftClickCooldown -= gameTime.ElapsedGameTime;
            if (_mRightClickCooldown > TimeSpan.Zero) _mRightClickCooldown -= gameTime.ElapsedGameTime;

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (_mRightClickCooldown <= TimeSpan.Zero) // this means the cooldown is finished
                {
                    _mRightClickCooldown = TimeSpan.FromMilliseconds(500);
<<<<<<< HEAD
                    Player.RightClick(gameTime);
=======
                    Player.RightTool.Use(DateTime.Now);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
                    _mForceUpdate = true;
                }
            }

            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (_mLeftClickCooldown <= TimeSpan.Zero) // this means the cooldown is finished
                {
                    _mLeftClickCooldown = TimeSpan.FromMilliseconds(500);
<<<<<<< HEAD
                    Player.LeftClick(gameTime);
                    _mForceUpdate = true;
                }
            }
=======
                    Player.LeftTool.Use(DateTime.Now);
                    _mForceUpdate = true;
                }
            }

            Player.RightTool.SwitchType(scrollWheelDelta);
            Player.LeftTool.SwitchType(scrollWheelDelta);
>>>>>>> b2fc2c2fe2bde1de545e4c42ddb20053f36579b5
            
            //do not do this each tick
            if (!previousView.Equals(Camera.View) || _mForceUpdate)
            {
                _mLookVector = Camera.LookVector;
                _mLookVector.Normalize();

                var x = SetPlayerSelectedBlock(false);
                if (x != 0) // x==0 is equivalent to payer.currentSelection == null
                {
                    SetPlayerAdjacentSelectedBlock(x + 0.5f);
                }
            }
            _mForceUpdate = false;
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
            _mFog.Draw();
        }

        #endregion

        #region Fields
        
        public readonly Player Player;       
        public readonly FirstPersonCamera Camera;
        private readonly FirstPersonCameraController _mCameraController;
        private readonly FogRenderer _mFog;

        private Vector3 _mLookVector;
        private readonly InputController _mInput;
        private readonly GraphicsDevice _mGraphicsDevice;
        private readonly Viewport _mViewport;
        private readonly PlayerPhysics _mPhysics;

        // SelectionBlock
        public Model SelectionBlock;
        private BasicEffect _mSelectionBlockEffect;
        private Texture2D _mSelectionBlockTexture;
        private TimeSpan _mLeftClickCooldown;
        private TimeSpan _mRightClickCooldown;
        private bool _mForceUpdate;
        private ButtonComponent _resumeButton;
        public bool FreeCam;

        #endregion

        #region SelectionBlock

        public void RenderSelectionBlock(GameTime gameTime)
        {
            _mGraphicsDevice.BlendState = BlendState.NonPremultiplied;
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
            _mSelectionBlockEffect.World = identity;
            _mSelectionBlockEffect.View = Camera.View;
            _mSelectionBlockEffect.Projection = Camera.Projection;
            _mSelectionBlockEffect.Texture = _mSelectionBlockTexture;
            _mSelectionBlockEffect.TextureEnabled = true;

            // apply the effect
            foreach (var pass in _mSelectionBlockEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                DrawSelectionBlockMesh(_mGraphicsDevice, SelectionBlock.Meshes[0], _mSelectionBlockEffect);
            }
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            var count = mesh.MeshParts.Count;
            for (var i = 0; i < count; i++)
            {
                var parts = mesh.MeshParts[i];
                if (parts.NumVertices <= 0) continue;
                _mGraphicsDevice.Indices = parts.IndexBuffer;
                //TODO better use DrawUserIndexedPrimitives for fully dynamic content
                _mGraphicsDevice.SetVertexBuffer(parts.VertexBuffer);
                //graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, parts.NumVertices,
                //    parts.StartIndex, parts.PrimitiveCount);
                graphicsdevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, parts.NumVertices, parts.PrimitiveCount);
            }
        }

        private float SetPlayerSelectedBlock(bool waterSelectable)
        {
            for (var x = 0.5f; x < 8f; x += 0.1f)
            {
                var targetPoint = Camera.Position + (_mLookVector*x);
                var block = Player.World.GetBlock(targetPoint);
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
                var targetPoint = Camera.Position + (_mLookVector*x);
                var block = Player.World.GetBlock(targetPoint);
                
                if (Player.World.GetBlock(targetPoint).Id != BlockType.None) continue;
                Player.CurrentSelectedAdjacent = new PositionedBlock(targetPoint, block);
                break;
            }
        }

        #endregion
    }
}