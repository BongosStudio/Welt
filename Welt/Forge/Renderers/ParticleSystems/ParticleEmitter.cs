#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;

namespace Welt.Forge.Renderers.ParticleSystems
{
    public class ParticleEmitter
    {
        private readonly ParticleSystem _mParticleSystem;
        private readonly float _mTimeBetweenParticles;
        private Vector3 _mPreviousPosition;
        private float _mTimeLeftOver;

        public ParticleEmitter(ParticleSystem system, float particlesPerSecond, Vector3 initialPosition)
        {
            _mParticleSystem = system;
            _mTimeBetweenParticles = 1.0f/particlesPerSecond;
            _mPreviousPosition = initialPosition;
        }

        public void Update(GameTime time, Vector3 position)
        {
            if (time == null) throw new ArgumentException(nameof(time));

            var elapsedTime = (float) time.ElapsedGameTime.TotalSeconds;
            if (elapsedTime > 0)
            {
                var velocity = (position - _mPreviousPosition)/elapsedTime;
                var timeToSpend = _mTimeLeftOver + elapsedTime;
                var currentTime = -_mTimeLeftOver;

                while (timeToSpend > _mTimeBetweenParticles)
                {
                    currentTime += _mTimeBetweenParticles;
                    timeToSpend -= _mTimeBetweenParticles;

                    var mu = currentTime/elapsedTime;

                    var p = Vector3.Lerp(_mPreviousPosition, position, mu);
                    _mParticleSystem.AddParticle(p, velocity);
                }

                _mTimeLeftOver = timeToSpend;
            }
            _mPreviousPosition = position;
        }
    }
}