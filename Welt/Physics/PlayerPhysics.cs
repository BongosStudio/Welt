#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
#region Using Statements

using System;
using System.ComponentModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Welt.API.Forge;
using Welt.Blocks;
using Welt.Cameras;
using Welt.Forge;
using Welt.Models;
using static Welt.Core.FastMath;

#endregion

namespace Welt.Physics
{
    public class PlayerPhysics
    {
        #region Fields

        private readonly Player _mPlayer;
        private readonly FirstPersonCamera _mCamera;

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
            _mPlayer = playerRenderer.Player;
            _mCamera = playerRenderer.Camera;
            _mPlayer.Entity.PropertyChanged += OnEntityPropertyChanged;
        }

        public void Move(GameTime gameTime)
        {
            UpdateEntity(gameTime);
            UpdatePosition(gameTime);

            var headbobOffset = (float) Math.Sin(_mPlayer.HeadBob)*GetPlayerHeadBob() + 0.15f;
            if (_mPlayer.Entity.IsInWater) headbobOffset = 0;
            _mCamera.Position = _mPlayer.Position + new Vector3(0, headbobOffset, 0);
        }

        #region UpdatePosition

        private void OnEntityPropertyChanged(object sender, PropertyChangedEventArgs args)
        {
            switch (args.PropertyName.ToLower())
            {
                case "isrunning":
                    // TODO: change FOV and change stamina
                    if (_mPlayer.Entity.IsRunning)
                    {
                        _mPlayer.Entity.Stamina -= 0.005f;
                    }
                    break;
                case "stamina":
                    if (_mPlayer.Entity.Stamina == 0)
                    {
                        _staminaReset = TimeSpan.FromSeconds(3);
                    }
                    if (_mPlayer.Entity.Stamina > 1)
                    {
                        _mPlayer.Entity.Stamina = 1;
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
            _mPlayer.Entity.IsRunning = kstate[Keys.LeftShift] == KeyState.Down && _mPlayer.Entity.Stamina > 0;
            _mPlayer.Entity.IsCrouching = kstate[Keys.LeftControl] == KeyState.Down;
            if (_staminaReset > TimeSpan.Zero) _staminaReset -= time.ElapsedGameTime;
            if (_staminaReset <= TimeSpan.Zero)
            {
                if (_mPlayer.Entity.Stamina < 0.25f)
                {
                    _mPlayer.Entity.Stamina = 0.25f;
                    _staminaReset = TimeSpan.FromSeconds(5);
                }
                else
                {
                    _mPlayer.Entity.Stamina += 0.005f;
                }
            }
        }

        private float GetPlayerSpeed()
        {
            if (_mPlayer.Entity.IsInWater) return 1.7f;
            if (_mPlayer.Entity.IsRunning) return 5.0f;
            if (_mPlayer.Entity.IsCrouching) return 1.7f;
            return 3.5f;
        }

        private float GetPlayerHeadBob()
        {
            if (_mPlayer.Entity.IsCrouching) return 0.01f;
            if (_mPlayer.Entity.IsRunning) return 0.15f;
            return 0.1f;
        }

        private float GetPlayerGravity()
        {
            return _mPlayer.Entity.IsInWater ? PLAYERGRAVITY/2 : PLAYERGRAVITY;
        }

        private void UpdatePosition(GameTime gameTime)
        {
            var footPosition = _mPlayer.Position + new Vector3(0f, -1.5f, 0f);
            var headPosition = _mPlayer.Position + new Vector3(0f, 0.1f, 0f);
            // adjust to if in water
            _mPlayer.Entity.IsInWater = _mPlayer.World.GetBlock(footPosition).Id == BlockType.WATER;

            var velocity = GetPlayerGravity()*(float) gameTime.ElapsedGameTime.TotalSeconds;
            var min = _mPlayer.Entity.IsInWater ? -MAX_VELOCITY_WATER : -MAX_VELOCITY;
            var max = _mPlayer.Entity.IsInWater ? MAX_VELOCITY_WATER : MAX_VELOCITY;
            _mPlayer.Velocity.Y += velocity;
            Adjust(min, max, ref _mPlayer.Velocity.Y);

            //TODO _isAboveSnowline = headPosition.Y > WorldSettings.SNOWLINE;

            if (Block.IsSolidBlock(_mPlayer.World.GetBlock(footPosition).Id) ||
                Block.IsSolidBlock(_mPlayer.World.GetBlock(headPosition).Id))
            {
                var standingOnBlock = _mPlayer.World.GetBlock(footPosition).Id;
                var hittingHeadOnBlock = _mPlayer.World.GetBlock(headPosition).Id;

                // TODO: fall damage

                // If the player has their head stuck in a block, push them down.
                if (Block.IsSolidBlock(_mPlayer.World.GetBlock(headPosition).Id))
                {
                    var blockIn = (int) (headPosition.Y);
                    _mPlayer.Position.Y = blockIn - 0.15f;
                }

                // If the player is stuck in the ground, bring them out.
                // This happens because we're standing on a block at -1.5, but stuck in it at -1.4, so -1.45 is the sweet spot.
                if (Block.IsSolidBlock(_mPlayer.World.GetBlock(footPosition).Id))
                {
                    var blockOn = (int) (footPosition.Y);
                    _mPlayer.Position.Y = (float) (blockOn + 1 + 1.45);
                }

                _mPlayer.Velocity.Y = 0;

                // Logic for standing on a block.
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

            _mPlayer.Position += _mPlayer.Velocity*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var kstate = Keyboard.GetState();

            var moveVector = new Vector3();

            var speedNormalize = _mPlayer.Entity.IsInWater ? 1f : 2f;
            if (kstate.IsKeyDown(Keys.Space))
            {
                if (_mPlayer.Entity.IsInWater)
                {
                    _mPlayer.Position.Y += 0.05f;
                }
                // ReSharper disable once CompareOfFloatsByEqualityOperator
                else if ((Block.IsSolidBlock(_mPlayer.World.GetBlock(footPosition).Id) && _mPlayer.Velocity.Y == 0))
                {
                    _mPlayer.Velocity.Y = PLAYERJUMPVELOCITY;
                    var amountBelowSurface = ((ushort) footPosition.Y) + 1 - footPosition.Y;
                    _mPlayer.Position.Y += amountBelowSurface + 0.05f;
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

            _mPlayer.Entity.IsMoving = moveVector != new Vector3();
            //moveVector.Normalize();
            moveVector *= GetPlayerSpeed()*(float) gameTime.ElapsedGameTime.TotalSeconds;

            var rotatedMoveVector = Vector3.Transform(moveVector, Matrix.CreateRotationY(_mCamera.LeftRightRotation));

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
            var movePosition = _mPlayer.Position + testVector;
            var midBodyPoint = movePosition + new Vector3(0, -0.7f, 0);
            var lowerBodyPoint = movePosition + new Vector3(0, -1.4f, 0);

            if (!Block.IsSolidBlock(_mPlayer.World.GetBlock(movePosition).Id) &&
                !Block.IsSolidBlock(_mPlayer.World.GetBlock(lowerBodyPoint).Id) &&
                !Block.IsSolidBlock(_mPlayer.World.GetBlock(midBodyPoint).Id))
            {
                _mPlayer.Position = _mPlayer.Position + moveVector;
                if (moveVector != Vector3.Zero)
                {
                    _mPlayer.HeadBob += 0.2;
                }
                return true;
            }

            // It's solid there, so while we can't move we have officially collided with it.
            var lowerBlock = _mPlayer.World.GetBlock(lowerBodyPoint).Id;
            var midBlock = _mPlayer.World.GetBlock(midBodyPoint).Id;
            var upperBlock = _mPlayer.World.GetBlock(movePosition).Id;

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
