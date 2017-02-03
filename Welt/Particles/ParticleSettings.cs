using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Welt.Particles
{
    public partial class ParticleSettings
    {
        public string TextureName = null;
        public int MaxParticles = 100;
        public TimeSpan Duration = TimeSpan.FromSeconds(1);
        public float DurationRandomness = 0;
        public float EmitterVelocitySensitivity = 1;

        public float MinHorizontalVelocity = 0;
        public float MaxHorizontalVelocity = 0;
        public float MinVerticalVelocity = 0;
        public float MaxVerticalVelocity = 0;
        public Vector3 Gravity = Vector3.Zero;

        // 0 means they'll come to a stop when they die, 1 means they'll stay the same speed,
        // and higher than 1 means they'll speed up
        public float EndVelocity = 1;

        public Color MinColor = Color.White;
        public Color MaxColor = Color.White;

        public float MinRotateSpeed = 0;
        public float MaxRotateSpeed = 0;

        public float MinStartSize = 100;
        public float MaxStartSize = 100;

        public float MinEndSize = 100;
        public float MaxEndSize = 100;

        [ContentSerializerIgnore]
        public BlendState BlendState = BlendState.NonPremultiplied;

        [ContentSerializer(ElementName = "BlendState")]
        private string BlendStateString
        {
            get { return BlendState.Name.Split('.')[1]; }
            set
            {
                switch (value)
                {
                    case "AlphaBlend":
                        BlendState = BlendState.AlphaBlend;
                        break;
                    case "NonPremultiplied":
                        BlendState = BlendState.NonPremultiplied;
                        break;
                    case "Opaque":
                        BlendState = BlendState.Opaque;
                        break;
                    case "Additive":
                        BlendState = BlendState.Additive;
                        break;
                    default:
                        throw new ArgumentException($"Unknown blendstate value: {value}");
                }
            }
        }
    }
}
