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
using Welt.API;
using Welt.API.Forge;
using Welt.Extensions;
using Welt.Core.Extensions;
using Welt.Core.Forge;

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
            m_LeftClickCooldown = TimeSpan.Zero;
            m_RightClickCooldown = TimeSpan.Zero;
            m_Input = InputController.Instance;
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
            UpdatePosition(gameTime);
            CalculateLookAt();
            var hb = GetPlayerHeadBob();
            var headbobOffset = (float)(Math.Sin(Player.HeadBob) * hb.Size + 0.15f) * hb.Speed;
            if (Player.IsPlayerInWater() || Player.IsFlying) headbobOffset = 0;
            CameraController.Camera.Position = Player.Position + new Vector3(0, headbobOffset + 0.7f, 0);
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
        
        private readonly InputController m_Input;
        private readonly GraphicsDevice m_GraphicsDevice;
        private readonly Viewport m_Viewport;

        // SelectionBlock
        public Model SelectionBlock;
        private BasicEffect m_SelectionBlockEffect;
        private Texture2D m_SelectionBlockTexture;
        private TimeSpan m_LeftClickCooldown;
        private TimeSpan m_RightClickCooldown;
        public Vector3I LookingAtPosition;
        public Vector3I LookingAtNeighbor;
        public Block LookingAt { get; private set; }
        public bool FreeCam;

        #endregion

        #region SelectionBlock

        public void RenderSelectionBlock(GameTime gameTime)
        {
            m_GraphicsDevice.BlendState = BlendState.NonPremultiplied;
                // allows any transparent pixels in original PNG to draw transparent

            
        }

        private void CalculateLookAt()
        {
            var lookVector = Camera.LookVector;
            for (var i = 0.5f; i < 8; i += 0.1f)
            {
                var check = Camera.Position + (Camera.RotationMatrix.Forward * i);
                var block = Player.World.GetBlock(check);
                var provider = Player.BlockRepository.GetBlockProvider(block.Id);
                if (!provider.IsSelectable) continue;
                if (!provider.GetBoundingBox(block.Metadata).HasValue) continue;
                var b = provider.GetBoundingBox(block.Metadata).Value.OffsetBy(check.Floor());
                if (b.Contains(check) != ContainmentType.Contains) continue;
                CalculateLookAtFace(i);
                LookingAtPosition = check;
                LookingAt = block;
                return;
            }
            LookingAt = new Block();
        }

        private void CalculateLookAtFace(float x)
        {
            for (var i = x; i > 0.7f; i -= 0.1f)
            {
                var check = Camera.Position + (Camera.RotationMatrix.Forward * i);
                var block = Player.World.GetBlock(check);
                var provider = Player.BlockRepository.GetBlockProvider(block.Id);
                if (provider.IsSelectable) continue;
                LookingAtNeighbor = check;
                return;
            }
            LookingAtNeighbor = Vector3.Zero;
        }

        private void DrawSelectionBlockMesh(GraphicsDevice graphicsdevice, ModelMesh mesh, Effect effect)
        {
            
        }

        #endregion

        private (float Size, float Speed) GetPlayerHeadBob()
        {
            if (Player.Entity.IsCrouching) return (0.01f, 0.5f);
            if (Player.Entity.IsSprinting) return (0.1f, 1);
            if (Player.IsPlayerInWater()) return (0, 0);
            if (Player.IsFlying) return (0, 0);
            return (0.05f, 0.75f);

        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = Player.Position - new Vector3(0, 1.4f, 0);
            var atFeet = Player.World.GetBlockProvider(footPosition);
            
            // do carryover velocity (ie. falling, jumping, etc)
            if (!Player.IsFlying)
            {
                var gravity = Player.World.GetGravity() * (float)gameTime.ElapsedGameTime.TotalSeconds * 0.05f;
                Player.Position += Player.Velocity;
                var newVelocityY = (Player.Velocity.Y + gravity) * (1 - atFeet.Density);
                var newAtFeet = Player.World.GetBlockProvider(Player.Position);
                if (atFeet.Id != newAtFeet.Id)
                {
                    WeltGame.Instance.Audio.PlayStepFor(atFeet.Id);
                }
                if (Player.World.GetBlockProvider(Player.Position).Density >= 1)
                {
                    Player.Position += new Vector3(0, 1.4f, 0);
                    newVelocityY = 0;
                }
                
                var newVelocityX = FastMath.Centerize(0, Player.Velocity.X, atFeet.Density);
                var newVelocityZ = FastMath.Centerize(0, Player.Velocity.Z, atFeet.Density);
                FastMath.Adjust(-Player.TerminalVelocity, Player.TerminalVelocity, ref newVelocityY);
                FastMath.Adjust(-20, 20, ref newVelocityX);
                FastMath.Adjust(-20, 20, ref newVelocityZ);
                Player.Velocity = new Vector3(newVelocityX, newVelocityY, newVelocityZ);
            }
            else
            {
                Player.Velocity = Vector3.Zero;
            }
            
            var moveVector = new Vector3(0, 0, 0);
            if (!Player.IsPaused)
            {
                var keyState = Keyboard.GetState();
                if (keyState.GetPressedKeys().Length == 0) return;

                if (m_Input.IsInputPressed(InputController.InputAction.MoveForward))
                {
                    moveVector += Vector3.Forward;
                }
                if (m_Input.IsInputPressed(InputController.InputAction.MoveBackward))
                {
                    moveVector += Vector3.Backward;
                }
                if (m_Input.IsInputPressed(InputController.InputAction.StrafeLeft))
                {
                    moveVector += Vector3.Left;
                }
                if (m_Input.IsInputPressed(InputController.InputAction.StrafeRight))
                {
                    moveVector += Vector3.Right;
                }
                if (m_Input.IsInputPressed(InputController.InputAction.Sprint))
                {
                    if (Player.IsFlying)
                    {
                        moveVector += Vector3.Down;
                    }
                    else
                    {
                        Player.Entity.IsSprinting = true;
                    }
                }
                if (m_Input.IsInputPressed(InputController.InputAction.Jump))
                {
                    if (Player.IsFlying || Player.IsPlayerInWater())
                    {
                        moveVector += Vector3.Up;
                    }
                    else if (Player.World.World.IsBlockAround(BlockType.LADDER, Player.Position, 1, 1, 1, out var ladderPos))
                    {
                        moveVector += new Vector3(0, 0.5f, 0);
                    }
                    else if (atFeet.Density >= 0 && Player.Velocity.Y == 0)
                    {
                        Player.Velocity += new Vector3(0, 0.3f, 0)*atFeet.Density;
                        Player.Position += new Vector3(0, 1.05f, 0)*atFeet.Density;
                    }
                }

            }
            if (moveVector != Vector3.Zero)
            {
                var rotationMatrix = Matrix.CreateRotationY(Camera.LeftRightRotation);
                var rotatedVector = Vector3.Transform(moveVector, rotationMatrix);
                if (TryMoveTo(rotatedVector, gameTime))
                {
                    WeltGame.Instance.Audio.PlayStepFor(Player.World.GetBlock(Player.Position).Id);
                }
                else
                {

                }
            }
        }

        private void DoHeadBob()
        {

        }

        private bool TryMoveTo(Vector3 moveVector, GameTime gameTime)
        {
            if (Player.Entity.IsSprinting)
            {
                moveVector *= new Vector3(1.5f, 1, 1.5f);
            }
            // Build a "test vector" that is a little longer than the move vector.
            var moveLength = moveVector.Length();
            var testVector = moveVector;
            testVector.Normalize();
            testVector = testVector * moveLength;

            // Apply this test vector.
            var movePosition = Player.Position + testVector;
            var midBodyPoint = movePosition + new Vector3(0, 0.5f, 0);
            var lowerBodyPoint = movePosition - new Vector3(0, 0.5f, 0);

            var lowerBlock = Player.BlockRepository.GetBlockProvider(Player.World.GetBlock(lowerBodyPoint).Id);
            var midBlock = Player.BlockRepository.GetBlockProvider(Player.World.GetBlock(midBodyPoint).Id);
            var currentBlock = Player.BlockRepository.GetBlockProvider(Player.World.GetBlock(Player.Position).Id);

            if (lowerBlock.Density < 1 && midBlock.Density < 1)
            {
                Player.Position += (moveVector * (1 - FastMath.Max(lowerBlock.Density, midBlock.Density)))*.15f;
                if (moveVector != Vector3.Zero)
                {
                    Player.HeadBob += 0.2;
                }
                return true;
            }
            else if (lowerBlock.Density >= 1 && midBlock.Density < 1)
            {
                Player.Velocity += new Vector3(0, 0.3f, 0);
                Player.Position += new Vector3(0, 1.05f, 0);
                return true;
            }
            return false;

        }
    }
}