#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
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

        private readonly Player m_player;
        private readonly FirstPersonCamera m_camera;

        private const float PLAYERJUMPVELOCITY = 6f;
        private const float PLAYERGRAVITY = -15f;
        private const float MAX_VELOCITY = 20f;
        private const float MAX_VELOCITY_WATER = 4f;
        private const float VELOCITY_DAMAGE_CAP = 5f; // each 0.5f would take 0.5f HP. 20f is instant death.

        #endregion

        public PlayerPhysics(PlayerRenderer playerRenderer)
        {
            m_player = playerRenderer.Player;
            m_camera = playerRenderer.Camera;
        }

        public void Move(GameTime gameTime)
        {
            UpdatePosition(gameTime);

            var headbobOffset = ((float) Math.Sin(m_player.HeadBob)*0.1f) + 0.15f;
            if (m_player.IsInWater) headbobOffset = 0;
            m_camera.Position = m_player.Position + new Vector3(0, headbobOffset, 0);
        }

        #region UpdatePosition

        private float GetPlayerSpeed()
        {
            return m_player.IsInWater ? 1.7f : 3.5f;
        }

        private float GetPlayerGravity()
        {
            return m_player.IsInWater ? PLAYERGRAVITY/2 : PLAYERGRAVITY;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = m_player.Position + new Vector3(0f, -1.5f, 0f);
            var headPosition = m_player.Position + new Vector3(0f, 0.1f, 0f);
            // adjust to if in water
            m_player.IsInWater = m_player.World.GetBlock(footPosition).Id == BlockType.Water;

            var velocity = GetPlayerGravity()*(float) gameTime.ElapsedGameTime.TotalSeconds;
            var min = m_player.IsInWater ? -MAX_VELOCITY_WATER : -MAX_VELOCITY;
            var max = m_player.IsInWater ? MAX_VELOCITY_WATER : MAX_VELOCITY;
            m_player.Velocity.Y += velocity;
            Adjust(min, max, ref m_player.Velocity.Y);

            //TODO _isAboveSnowline = headPosition.Y > WorldSettings.SNOWLINE;

            if (BlockInformation.IsSolidBlock(m_player.World.GetBlock(footPosition).Id) ||
                BlockInformation.IsSolidBlock(m_player.World.GetBlock(headPosition).Id))
            {
                var standingOnBlock = m_player.World.GetBlock(footPosition).Id;
                var hittingHeadOnBlock = m_player.World.GetBlock(headPosition).Id;

                // If we"re hitting the ground with a high velocity, die!
                //if (standingOnBlock != BlockType.None && _P.playerVelocity.Y < 0)
                //{
                //    float fallDamage = Math.Abs(_P.playerVelocity.Y) / DIEVELOCITY;
                //    if (fallDamage >= 1)
                //    {
                //        _P.PlaySoundForEveryone(InfiniminerSound.GroundHit, _P.playerPosition);
                //        _P.KillPlayer(Defines.deathByFall);//"WAS KILLED BY GRAVITY!");
                //        return;
                //    }
                //    else if (fallDamage > 0.5)
                //    {
                //        // Fall damage of 0.5 maps to a screenEffectCounter value of 2, meaning that the effect doesn't appear.
                //        // Fall damage of 1.0 maps to a screenEffectCounter value of 0, making the effect very strong.
                //        if (standingOnBlock != BlockType.Jump)
                //        {
                //            _P.screenEffect = ScreenEffect.Fall;
                //            _P.screenEffectCounter = 2 - (fallDamage - 0.5) * 4;
                //            _P.PlaySoundForEveryone(InfiniminerSound.GroundHit, _P.playerPosition);
                //        }
                //    }
                //}

                // If the player has their head stuck in a block, push them down.
                if (BlockInformation.IsSolidBlock(m_player.World.GetBlock(headPosition).Id))
                {
                    var blockIn = (int) (headPosition.Y);
                    m_player.Position.Y = blockIn - 0.15f;
                }

                // If the player is stuck in the ground, bring them out.
                // This happens because we're standing on a block at -1.5, but stuck in it at -1.4, so -1.45 is the sweet spot.
                if (BlockInformation.IsSolidBlock(m_player.World.GetBlock(footPosition).Id))
                {
                    var blockOn = (int) (footPosition.Y);
                    m_player.Position.Y = (float) (blockOn + 1 + 1.45);
                }

                m_player.Velocity.Y = 0;

                // Logic for standing on a block.
                switch (standingOnBlock)
                {
                    //case BlockType.Jump:
                    //    _player.playerVelocity.Y = 2.5f*JUMPVELOCITY;
                    //    _P.PlaySoundForEveryone(InfiniminerSound.Jumpblock, _P.playerPosition);
                    //    break;

                    //case BlockType.Road:
                    //    movingOnRoad = true;
                    //    break;

                    //case BlockType.Lava:
                    //    _P.KillPlayer(Defines.deathByLava);
                    //    return;
                    case BlockType.Water:

                        break;
                }

                //Logic for bumping your head on a block.
                switch (hittingHeadOnBlock)
                {
                    //case BlockType.Shock:
                    //    _P.KillPlayer(Defines.deathByElec);
                    //    return;

                    //case BlockType.Lava:
                    //    _P.KillPlayer(Defines.deathByLava);
                    //    return;
                }
            }

            // Death by falling off the map.
            //if (_P.playerPosition.Y < -30)
            //{
            //    _P.KillPlayer(Defines.deathByMiss);
            //    return;
            //}

            m_player.Position += m_player.Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();

            var moveVector = new Vector3();

            var speedNormalize = m_player.IsInWater ? 1f : 2f;
            if (kstate.IsKeyDown(Keys.Space))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((BlockInformation.IsSolidBlock(m_player.World.GetBlock(footPosition).Id) && m_player.Velocity.Y == 0))
                {
                    m_player.Velocity.Y = PLAYERJUMPVELOCITY;
                    var amountBelowSurface = ((ushort) footPosition.Y) + 1 - footPosition.Y;
                    m_player.Position.Y += amountBelowSurface + 0.05f;
                }
                else if (m_player.IsInWater)
                {
                    // handle swimming
                    // TODO: to be completely frank, idk wtf to do here. Adjusting the velocity doesn't change
                    // and we already slowed down the moving speed by using GetPlayerSpeed()
                }
            }

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

            m_player.IsMoving = moveVector != new Vector3();
            //moveVector.Normalize();
            moveVector *= GetPlayerSpeed()*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var rotatedMoveVector = Vector3.Transform(moveVector, Matrix.CreateRotationY(m_camera.LeftRightRotation));

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
            var movePosition = m_player.Position + testVector;
            var midBodyPoint = movePosition + new Vector3(0, -0.7f, 0);
            var lowerBodyPoint = movePosition + new Vector3(0, -1.4f, 0);

            if (!BlockInformation.IsSolidBlock(m_player.World.GetBlock(movePosition).Id) &&
                !BlockInformation.IsSolidBlock(m_player.World.GetBlock(lowerBodyPoint).Id) &&
                !BlockInformation.IsSolidBlock(m_player.World.GetBlock(midBodyPoint).Id))
            {
                m_player.Position = m_player.Position + moveVector;
                if (moveVector != Vector3.Zero)
                {
                    m_player.HeadBob += 0.2;
                }
                return true;
            }

            // It's solid there, so while we can't move we have officially collided with it.
            var lowerBlock = m_player.World.GetBlock(lowerBodyPoint).Id;
            var midBlock = m_player.World.GetBlock(midBodyPoint).Id;
            var upperBlock = m_player.World.GetBlock(movePosition).Id;

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
