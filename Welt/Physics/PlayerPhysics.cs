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

        private readonly Player _player;
        private readonly FirstPersonCamera _camera;

        private const float PLAYERJUMPVELOCITY = 6f;
        private const float PLAYERGRAVITY = -15f;
        private const float MAX_VELOCITY = 20f;
        private const float MAX_VELOCITY_WATER = 4f;
        private const float VELOCITY_DAMAGE_CAP = 5f; // each 0.5f would take 0.5f HP. 20f is instant death.

        #endregion

        public PlayerPhysics(PlayerRenderer playerRenderer)
        {
            _player = playerRenderer.Player;
            _camera = playerRenderer.Camera;
        }

        public void Move(GameTime gameTime)
        {
            UpdatePosition(gameTime);

            var headbobOffset = ((float) Math.Sin(_player.HeadBob)*0.1f) + 0.15f;
            if (_player.IsInWater) headbobOffset = 0;
            _camera.Position = _player.Position + new Vector3(0, headbobOffset, 0);
        }

        #region UpdatePosition

        private float GetPlayerSpeed()
        {
            return _player.IsInWater ? 1.7f : 3.5f;
        }

        private float GetPlayerGravity()
        {
            return _player.IsInWater ? PLAYERGRAVITY/2 : PLAYERGRAVITY;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = _player.Position + new Vector3(0f, -1.5f, 0f);
            var headPosition = _player.Position + new Vector3(0f, 0.1f, 0f);
            // adjust to if in water
            _player.IsInWater = _player.World.GetBlock(footPosition).Type == BlockType.Water;

            var velocity = GetPlayerGravity()*(float) gameTime.ElapsedGameTime.TotalSeconds;
            var min = _player.IsInWater ? -MAX_VELOCITY_WATER : -MAX_VELOCITY;
            var max = _player.IsInWater ? MAX_VELOCITY_WATER : MAX_VELOCITY;
            _player.Velocity.Y += velocity;
            Adjust(min, max, ref _player.Velocity.Y);

            //TODO _isAboveSnowline = headPosition.Y > WorldSettings.SNOWLINE;

            if (BlockInformation.IsSolidBlock(_player.World.GetBlock(footPosition).Type) ||
                BlockInformation.IsSolidBlock(_player.World.GetBlock(headPosition).Type))
            {
                var standingOnBlock = _player.World.GetBlock(footPosition).Type;
                var hittingHeadOnBlock = _player.World.GetBlock(headPosition).Type;

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
                if (BlockInformation.IsSolidBlock(_player.World.GetBlock(headPosition).Type))
                {
                    var blockIn = (int) (headPosition.Y);
                    _player.Position.Y = blockIn - 0.15f;
                }

                // If the player is stuck in the ground, bring them out.
                // This happens because we're standing on a block at -1.5, but stuck in it at -1.4, so -1.45 is the sweet spot.
                if (BlockInformation.IsSolidBlock(_player.World.GetBlock(footPosition).Type))
                {
                    var blockOn = (int) (footPosition.Y);
                    _player.Position.Y = (float) (blockOn + 1 + 1.45);
                }

                _player.Velocity.Y = 0;

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

            _player.Position += _player.Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();

            var moveVector = new Vector3();

            if (kstate.IsKeyDown(Keys.Space))
            {
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                if ((BlockInformation.IsSolidBlock(_player.World.GetBlock(footPosition).Type) && _player.Velocity.Y == 0))
                {
                    _player.Velocity.Y = PLAYERJUMPVELOCITY;
                    var amountBelowSurface = ((ushort) footPosition.Y) + 1 - footPosition.Y;
                    _player.Position.Y += amountBelowSurface + 0.05f;
                }
                else if (_player.IsInWater)
                {
                    _player.Position.Y += 1f;
                    
                }
            }

            if (kstate.IsKeyDown(Keys.W))
            {
                moveVector += Vector3.Forward*2;
            }
            if (kstate.IsKeyDown(Keys.S))
            {
                moveVector += Vector3.Backward*2;
            }
            if (kstate.IsKeyDown(Keys.A))
            {
                moveVector += Vector3.Left*2;
            }
            if (kstate.IsKeyDown(Keys.D))
            {
                moveVector += Vector3.Right*2;
            }

            //moveVector.Normalize();
            moveVector *= GetPlayerSpeed()*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var rotatedMoveVector = Vector3.Transform(moveVector, Matrix.CreateRotationY(_camera.LeftRightRotation));

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
            var movePosition = _player.Position + testVector;
            var midBodyPoint = movePosition + new Vector3(0, -0.7f, 0);
            var lowerBodyPoint = movePosition + new Vector3(0, -1.4f, 0);

            if (!BlockInformation.IsSolidBlock(_player.World.GetBlock(movePosition).Type) &&
                !BlockInformation.IsSolidBlock(_player.World.GetBlock(lowerBodyPoint).Type) &&
                !BlockInformation.IsSolidBlock(_player.World.GetBlock(midBodyPoint).Type))
            {
                _player.Position = _player.Position + moveVector;
                if (moveVector != Vector3.Zero)
                {
                    _player.HeadBob += 0.2;
                }
                return true;
            }

            // It's solid there, so while we can't move we have officially collided with it.
            var lowerBlock = _player.World.GetBlock(lowerBodyPoint).Type;
            var midBlock = _player.World.GetBlock(midBodyPoint).Type;
            var upperBlock = _player.World.GetBlock(movePosition).Type;

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
