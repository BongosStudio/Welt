#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Welt.Blocks;
using Welt.Cameras;
using Welt.Forge;
using Welt.Models;
using static Welt.FastMath;

#endregion

namespace Welt.Physics
{
    public class PlayerPhysics
    {
        #region Fields

        private readonly Player m_Player;
        private readonly FirstPersonCamera m_Camera;

        private const float PLAYERJUMPVELOCITY = 6f;
        private const float PLAYERGRAVITY = -15f;
        private const float MAX_VELOCITY = 20f;
        private const float MAX_VELOCITY_WATER = 4f;
        private const float VELOCITY_DAMAGE_CAP = 5f; // each 0.5f would take 0.5f HP. 20f is instant death.
        // TODO: player stamina catcher & updater
        private TimeSpan _staminaReset = TimeSpan.Zero;

        #endregion

        public PlayerPhysics(PlayerRenderer playerRenderer)
        {
            m_Player = playerRenderer.Player;
            m_Camera = playerRenderer.Camera;
            m_Player.Entity.PropertyChanged += OnEntityPropertyChanged;
        }

        public void Move(GameTime gameTime)
        {
            UpdateEntity(gameTime);
            UpdatePosition(gameTime);
            var hb = GetPlayerHeadBob();
            var headbobOffset = (float) (Math.Sin(m_Player.HeadBob)*hb.Size + 0.15f)*hb.Speed;
            if (m_Player.Entity.IsInWater) headbobOffset = 0;
            m_Camera.Position = m_Player.Position + new Vector3(0, headbobOffset, 0);
        }

        #region UpdatePosition

        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName.ToLower())
            {
                case "isrunning":
                    // TODO: change FOV and change stamina
                    if (m_Player.Entity.IsRunning)
                    {
                        m_Player.Entity.Stamina -= 0.005f;
                    }
                    break;
                case "stamina":
                    if (m_Player.Entity.Stamina == 0)
                    {
                        _staminaReset = TimeSpan.FromSeconds(3);
                    }
                    if (m_Player.Entity.Stamina > 1)
                    {
                        m_Player.Entity.Stamina = 1;
                    }
                    break;
                case "isinwater":
                    break;
                case "iscrouching":
                    if (m_Player.Entity.IsCrouching)
                        m_Camera.Position = m_Player.Position + new Vector3(0, -0.5f, 0);
                    else
                        m_Camera.Position = m_Player.Position;
                    break;
            }
        }

        private void UpdateEntity(GameTime time)
        {
            var kstate = Keyboard.GetState();
            m_Player.Entity.IsRunning = kstate[Keys.LeftShift] == KeyState.Down && m_Player.Entity.Stamina > 0;
            m_Player.Entity.IsCrouching = kstate[Keys.LeftControl] == KeyState.Down;
            if (_staminaReset > TimeSpan.Zero) _staminaReset -= time.ElapsedGameTime;
            if (_staminaReset <= TimeSpan.Zero)
            {
                if (m_Player.Entity.Stamina < 0.25f)
                {
                    m_Player.Entity.Stamina = 0.25f;
                    _staminaReset = TimeSpan.FromSeconds(5);
                }
                else
                {
                    m_Player.Entity.Stamina += 0.005f;
                }
            }
        }

        private float GetPlayerSpeed()
        {
            const float emptySpaceRun = 5.0f;
            float speed;
            if (m_Player.Entity.IsRunning)
                speed = emptySpaceRun;
            else if (m_Player.Entity.IsCrouching)
                speed = emptySpaceRun / 3;
            else
                speed = emptySpaceRun / 2;
            if (m_Player.Entity.IsInWater)
            {
                speed = emptySpaceRun / 2;
            }
            return speed;
        }

        private (float Size, float Speed) GetPlayerHeadBob()
        {
            if (m_Player.Entity.IsCrouching) return (0.01f, 0.5f);
            if (m_Player.Entity.IsRunning) return (0.1f, 1);
            return (0.05f, 0.75f);
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = m_Player.Position + new Vector3(0f, -1.5f, 0f);
            var headPosition = m_Camera.Position;
            var kstate = Keyboard.GetState();
            var velocity = PLAYERGRAVITY * (float)gameTime.ElapsedGameTime.TotalSeconds;
            var min = m_Player.Entity.IsInWater ? -MAX_VELOCITY_WATER : -MAX_VELOCITY;
            var max = m_Player.Entity.IsInWater ? MAX_VELOCITY_WATER : MAX_VELOCITY;
            var blockAtFeet = m_Player.World.GetBlock(footPosition).Id;
            var blockAtHead = m_Player.World.GetBlock(headPosition).Id;
            m_Player.Entity.IsInWater = blockAtFeet == BlockType.WATER;

            m_Player.Velocity.Y += velocity;

            Adjust(min, max, ref m_Player.Velocity.Y);
            if (Block.HasCollision(blockAtFeet) ||
                Block.HasCollision(blockAtHead))
            {
                if (Block.HasCollision(blockAtHead))
                {
                    var blockIn = (int)(headPosition.Y);
                    m_Player.Position.Y = blockIn - 0.15f;
                }

                if (Block.HasCollision(blockAtFeet))
                {
                    var blockOn = (int)(footPosition.Y);
                    m_Player.Position.Y = (float)(blockOn + 1 + 1.45);
                }
                m_Player.Velocity.Y = 0;
                WeltGame.Instance.Audio.PlayStepFor(blockAtFeet);
            }

            m_Player.Position += m_Player.Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Space))
            {
                if (m_Player.Entity.IsInWater)
                {
                    m_Player.Velocity = new Vector3(0, 2, 0);
                    return;
                }
                else
                {
                    if ((Block.HasCollision(blockAtFeet) && m_Player.Velocity.Y == 0))
                    {
                        m_Player.Velocity.Y = PLAYERJUMPVELOCITY;
                        var amountBelowSurface = ((ushort)footPosition.Y) + 1 - footPosition.Y;
                        m_Player.Position.Y += amountBelowSurface + 0.05f;
                    }
                }
            }

            var moveVector = new Vector3();

            var speedNormalize = m_Player.Entity.IsInWater ? 1f : 2f;

            if (kstate.IsKeyDown(Keys.W))
            {
                moveVector += Vector3.Forward * speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                moveVector += Vector3.Backward * speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.A))
            {
                moveVector += Vector3.Left * speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                moveVector += Vector3.Right * speedNormalize;
            }

            m_Player.Entity.IsMoving = moveVector != new Vector3();
            //moveVector.Normalize();
            moveVector.X *= GetPlayerSpeed() * (float)gameTime.ElapsedGameTime.TotalSeconds;
            moveVector.Z *= GetPlayerSpeed() * (float)gameTime.ElapsedGameTime.TotalSeconds;

            var rotatedMoveVector = Vector3.Transform(moveVector, Matrix.CreateRotationY(m_Camera.LeftRightRotation));

            // Attempt to move, doing collision stuff.
            if (TryToMoveTo(rotatedMoveVector, gameTime))
            {
            }
            else if (!TryToMoveTo(new Vector3(0, 0, rotatedMoveVector.Z), gameTime))
            {
            }
            else if (!TryToMoveTo(new Vector3(rotatedMoveVector.X, 0, 0), gameTime))
            {
            }
        }

        #endregion

        #region TryToMoveTo

        private bool TryToMoveTo(Vector3 moveVector, GameTime gameTime)
        {
            // Build a "test vector" that is a little longer than the move vector.
            var moveLength = moveVector.Length();
            var testVector = moveVector;
            testVector.Normalize();
            testVector = testVector*(moveLength + 0.3f);

            // Apply this test vector.
            var movePosition = m_Player.Position + testVector;
            var midBodyPoint = movePosition + new Vector3(0, -0.7f, 0);
            var lowerBodyPoint = movePosition + new Vector3(0, -1.4f, 0);

            var lowerBlock = m_Player.World.GetBlock(lowerBodyPoint).Id;
            var midBlock = m_Player.World.GetBlock(midBodyPoint).Id;
            var upperBlock = m_Player.World.GetBlock(movePosition).Id;
            var currentBlock = m_Player.World.GetBlock(m_Player.Position).Id;
            var currentLowerBlock = m_Player.World.GetBlock(m_Player.Position + new Vector3(0, -1.4f, 0)).Id;

            if (!Block.HasCollision(upperBlock) &&
                !Block.HasCollision(lowerBlock) &&
                !Block.HasCollision(midBlock))
            {
                m_Player.Position += moveVector;
                if (moveVector != Vector3.Zero)
                {
                    m_Player.HeadBob += 0.2;
                }
                return true;
            }
            //else if (!Block.IsSolidBlock(midBlock) && !Block.IsSolidBlock(lowerBlock) && currentBlock == BlockType.WATER)
            //{
            //    m_Player.Position += moveVector;
            //    m_Player.Position.Y += 1;
            //    return true;
            //}

            // It's solid there, so while we can't move we have officially collided with it.


            // It's solid there, so see if it's a lava block. If so, touching it will kill us!
            //if (upperBlock == BlockType.Lava || lowerBlock == BlockType.Lava || midBlock == BlockType.Lava)
            //{
            //    _P.KillPlayer(Defines.deathByLava);
            //    return true;
            //}

            // If it's a ladder, move up.
            if (currentBlock == BlockType.LADDER || currentLowerBlock == BlockType.LADDER)
            {
                m_Player.Velocity.Y = 2.5f;
                Vector3 footPosition = m_Player.Position + new Vector3(0f, -1.5f, 0f);
                return true;
            }

            return false;
        }

        #endregion
    }
}
