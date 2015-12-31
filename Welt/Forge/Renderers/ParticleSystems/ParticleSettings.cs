#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Welt.Forge.Renderers.ParticleSystems
{
    public class ParticleSettings
    {
        public string TextureName;
        public int MaxParticles;
        public TimeSpan Duration;
        public float DurationRandomness;
        public float EmitterVelocitySensitivity;

        public float MinHorizontalVelocity;
        public float MaxHorizontalVelocity;

        public float MinVerticalVelocity;
        public float MaxVerticalVelocity;

        public Vector3 Gravity;
        public float EndVelocity;

        public Color MinColor;
        public Color MaxColor;

        public float MinRotationSpeed;
        public float MaxRotationSpeed;

        public float MinStartSize;
        public float MaxStartSize;

        public float MinEndSize;
        public float MaxEndSize;

        public BlendState Blend;

        public ParticleSettings(
            string texture,
            int maxp,
            TimeSpan dur,
            float durR,
            float evs,
            float minhv,
            float maxhv,
            float minvv,
            float maxvv,
            Vector3 grav,
            float ev,
            Color minc,
            Color maxc,
            float minrs,
            float maxrs,
            float minss,
            float maxss,
            float mines,
            float maxes,
            BlendState state)
        {
            TextureName = texture;
            MaxParticles = maxp;
            Duration = dur;
            DurationRandomness = durR;
            EmitterVelocitySensitivity = evs;
            MinHorizontalVelocity = minhv;
            MaxHorizontalVelocity = maxhv;
            MinVerticalVelocity = minvv;
            MaxVerticalVelocity = maxvv;
            Gravity = grav;
            EndVelocity = ev;
            MinColor = minc;
            MaxColor = maxc;
            MinRotationSpeed = minrs;
            MaxRotationSpeed = maxrs;
            MinStartSize = minss;
            MaxStartSize = maxss;
            MinEndSize = mines;
            MaxEndSize = maxes;
            Blend = state;
        }
    }
}