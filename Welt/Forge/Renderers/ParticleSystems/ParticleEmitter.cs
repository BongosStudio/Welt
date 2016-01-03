#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;

namespace Welt.Forge.Renderers.ParticleSystems
{
    public class ParticleEmitter
    {
        private readonly ParticleSystem m_particleSystem;
        private readonly float m_timeBetweenParticles;
        private Vector3 m_previousPosition;
        private float m_timeLeftOver;

        public ParticleEmitter(ParticleSystem system, float particlesPerSecond, Vector3 initialPosition)
        {
            m_particleSystem = system;
            m_timeBetweenParticles = 1.0f/particlesPerSecond;
            m_previousPosition = initialPosition;
        }

        public void Update(GameTime time, Vector3 position)
        {
            if (time == null) throw new ArgumentException(nameof(time));

            var elapsedTime = (float) time.ElapsedGameTime.TotalSeconds;
            if (elapsedTime > 0)
            {
                var velocity = (position - m_previousPosition)/elapsedTime;
                var timeToSpend = m_timeLeftOver + elapsedTime;
                var currentTime = -m_timeLeftOver;

                while (timeToSpend > m_timeBetweenParticles)
                {
                    currentTime += m_timeBetweenParticles;
                    timeToSpend -= m_timeBetweenParticles;

                    var mu = currentTime/elapsedTime;

                    var p = Vector3.Lerp(m_previousPosition, position, mu);
                    m_particleSystem.AddParticle(p, velocity);
                }

                m_timeLeftOver = timeToSpend;
            }
            m_previousPosition = position;
        }
    }
}