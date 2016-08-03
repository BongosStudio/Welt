using Microsoft.Xna.Framework;

namespace Welt.MonoGame.Extended.Particles.Modifiers
{
    public unsafe class VortexModifier : IModifier
    {
        public Vector2 Position { get; set; }
        public float Mass { get; set; }
        public float MaxSpeed { get; set; }

        // Note: not the real-life one
        private const float GRAV_CONST = 100000f;

        public void Update(float elapsedSeconds, ParticleBuffer.ParticleIterator iterator) {
            while (iterator.HasNext) {
                var particle = iterator.Next();
                var diff = Position + particle->TriggerPos - particle->Position;

                var distance2 = diff.LengthSquared();

                var speedGain = GRAV_CONST * Mass / distance2 * elapsedSeconds;
                // normalize distances and multiply by speedGain
                diff.Normalize();
                particle->Velocity += diff * speedGain;
            }
        }
    }
}