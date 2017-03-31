using Microsoft.Xna.Framework;

namespace Welt.Particles
{
    public partial class ParticleSettings
    {
        public static ParticleSettings RainLight = new ParticleSettings
        {
            TextureName = "Textures\\rain",
            MaxParticles = 50,
            MinHorizontalVelocity = 0,
            MaxHorizontalVelocity = 1,
            MinVerticalVelocity = 1,
            MaxVerticalVelocity = 2,
            Gravity = Vector3.Down,
            EndVelocity = 1,
            MinColor = Color.LightBlue,
            MaxColor = Color.White,
        };

        public static ParticleSettings RainMid = new ParticleSettings
        {
            TextureName = "Textures\\rain",
            MaxParticles = 500,
            MinHorizontalVelocity = 0,
            MaxHorizontalVelocity = 1,
            MinVerticalVelocity = 2,
            MaxVerticalVelocity = 4,
            Gravity = Vector3.Down,
            EndVelocity = 1,
            MinColor = Color.LightBlue,
            MaxColor = Color.White,
        };

        public static ParticleSettings RainHeavy = new ParticleSettings
        {
            TextureName = "Textures\\rain",
            MaxParticles = 1000,
            MinHorizontalVelocity = 0,
            MaxHorizontalVelocity = 1,
            MinVerticalVelocity = 6,
            MaxVerticalVelocity = 10,
            Gravity = Vector3.Down,
            EndVelocity = 1,
            MinColor = Color.LightBlue,
            MaxColor = Color.White,
        };
    }
}
