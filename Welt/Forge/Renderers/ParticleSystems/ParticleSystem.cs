#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework;

namespace Welt.Forge.Renderers.ParticleSystems
{
    public abstract class ParticleSystem : DrawableGameComponent
    {
        protected ParticleSystem(Game game) : base(game)
        {
        }

        public virtual void AddParticle(Vector3 position, Vector3 velocity)
        {
            
        }
    }
}