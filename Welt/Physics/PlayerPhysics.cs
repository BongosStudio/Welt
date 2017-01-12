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
                    // TODO: change model height and view height
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
                speed /= 2;
            }
            return speed;
        }

        private (float Size, float Speed) GetPlayerHeadBob()
        {
            if (m_Player.Entity.IsCrouching) return (0.01f, 0.5f);
            if (m_Player.Entity.IsRunning) return (0.1f, 1);
            return (0.05f, 0.75f);
        }

        private float GetPlayerGravity()
        {
            return m_Player.Entity.IsInWater ? 0 : PLAYERGRAVITY;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = m_Player.Position + new Vector3(0f, -1.5f, 0f);
            var headPosition = m_Player.Position + new Vector3(0f, 0.1f, 0f);
            var kstate = Keyboard.GetState();
            var velocity = GetPlayerGravity()*(float) gameTime.ElapsedGameTime.TotalSeconds;
            var min = m_Player.Entity.IsInWater ? -MAX_VELOCITY_WATER : -MAX_VELOCITY;
            var max = m_Player.Entity.IsInWater ? MAX_VELOCITY_WATER : MAX_VELOCITY;

            m_Player.Velocity.Y += velocity;
            Adjust(min, max, ref m_Player.Velocity.Y);
            m_Player.Entity.IsInWater = m_Player.World.GetBlock(footPosition).Id == BlockType.WATER;
            if (Block.IsSolidBlock(m_Player.World.GetBlock(footPosition).Id) ||
                Block.IsSolidBlock(m_Player.World.GetBlock(headPosition).Id))
            {
                var standingOnBlock = m_Player.World.GetBlock(footPosition).Id;
                var hittingHeadOnBlock = m_Player.World.GetBlock(headPosition).Id;

                // TODO: fall damage

                // If the player has their head stuck in a block, push them down.
                if (Block.IsSolidBlock(m_Player.World.GetBlock(headPosition).Id))
                {
                    var blockIn = (int) (headPosition.Y);
                    m_Player.Position.Y = blockIn - 0.15f;
                }

                // If the player is stuck in the ground, bring them out.
                // This happens because we're standing on a block at -1.5, but stuck in it at -1.4, so -1.45 is the sweet spot.
                if (Block.IsSolidBlock(m_Player.World.GetBlock(footPosition).Id))
                {
                    var blockOn = (int) (footPosition.Y);
                    m_Player.Position.Y = (float) (blockOn + 1 + 1.45);
                }

                //if (!m_Player.Entity.IsInWater) 
                m_Player.Velocity.Y = 0;

                // Logic for standing on a block.
                WeltGame.Instance.Audio.PlayStepFor(m_Player.World.GetBlock(footPosition).Id);
                switch (standingOnBlock)
                {
                    case BlockType.WATER:
                        // play swimming sound
                        break;
                    case BlockType.DIRT:
                        // play dirt sound
                        break;
                    case BlockType.GRASS:
                    case BlockType.LONG_GRASS:
                    case BlockType.LEAVES:
                        // play rustling mix with grass movement
                        // maybe make an easter egg where it plays the pokemon thing with "A WILD BLAHBLAHBLAH APPEARED"?
                        // Idk, I like that idea so I may do it.
                        break;
                }

                //Logic for bumping your head on a block.
                switch (hittingHeadOnBlock)
                {
                    case BlockType.LAVA:
                        // set the player on fire
                        break;
                }
            }

            // Death by falling off the map.
            //if (_P.playerPosition.Y < -30)
            //{
            //    _P.KillPlayer(Defines.deathByMiss);
            //    return;
            //}

            m_Player.Position += m_Player.Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;

            if (kstate.IsKeyDown(Keys.Space))
            {
                if (m_Player.Entity.IsInWater)
                {
                    m_Player.Position.Y += 0.05f;
                    return;
                }
                else
                {
                    if ((Block.IsSolidBlock(m_Player.World.GetBlock(footPosition).Id) && m_Player.Velocity.Y == 0))
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
                moveVector += Vector3.Forward*speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                moveVector += Vector3.Backward*speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.A))
            {
                moveVector += Vector3.Left*speedNormalize;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                moveVector += Vector3.Right*speedNormalize;
            }

            m_Player.Entity.IsMoving = moveVector != new Vector3();
            //moveVector.Normalize();
            moveVector.X *= GetPlayerSpeed()*(float) gameTime.ElapsedGameTime.TotalSeconds;
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

            if (!Block.IsSolidBlock(upperBlock) &&
                !Block.IsSolidBlock(lowerBlock) &&
                !Block.IsSolidBlock(midBlock))
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
            //if (upperBlock == BlockType.Ladder || lowerBlock == BlockType.Ladder || midBlock == BlockType.Ladder)
            //{
            //    _P.playerVelocity.Y = CLIMBVELOCITY;
            //    Vector3 footPosition = _P.playerPosition + new Vector3(0f, -1.5f, 0f);
            //    if (_P.blockEngine.SolidAtPointForPlayer(footPosition))
            //        _P.playerPosition.Y += 0.1f;
            //    return true;
            //}

            return false;
        }

        #endregion
    }
}
