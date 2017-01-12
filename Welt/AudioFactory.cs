using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Welt.Extensions;
using Welt.Forge;
using Welt.Models;
using Welt.Scenes;

namespace Welt
{
    // the Update loop will be running on a background thread
    public class AudioFactory
    {
        public SoundEffectInstance ButtonSound;
        public SoundEffectInstance WaterWaves;
        public SoundEffectInstance Fire;
        public SoundEffectInstance Rain;
        public SoundEffectInstance RainHeavy;
        public SoundEffectInstance Wind;
        public SoundEffectInstance StepSnow;
        public SoundEffectInstance Splash;

        public Song SongFeather;
        public Song SongSnowfall;

        public readonly WeltGame Game;
        
        private bool m_IsRunning;
        private Vector3 m_WaterDistance = new Vector3(5, 5, 5);

        private AudioListener m_AudioListener;
        private AudioEmitter m_AudioEmitter;

        public AudioFactory(WeltGame game)
        {
            Game = game;
            m_AudioListener = new AudioListener();
            m_AudioEmitter = new AudioEmitter();
            
        }

        public void BeginWatch()
        {
            m_IsRunning = true;
            new Thread(() =>
            {
                while (m_IsRunning)
                {
                    Update();
                    Thread.Sleep(500);
                }
            })
            { IsBackground = true }.Start();
        }

        public void Destroy()
        {
            m_IsRunning = false;
        }

        public void LoadContent(ContentManager content)
        {
            ButtonSound = content.Load<SoundEffect>("Sounds\\menu-button").CreateInstance();
            WaterWaves = content.Load<SoundEffect>("Sounds\\waves").CreateInstance();
            SongFeather = content.Load<Song>("Music\\Feather");
            SongSnowfall = content.Load<Song>("Music\\snowfall");
            Splash = content.Load<SoundEffect>("Sounds\\splash").CreateInstance();
        }

        public void PlayButtonSound()
        {
            ButtonSound.Play();
        }

        public void PlayStepFor(ushort block)
        {
            switch (block)
            {
                case BlockType.WATER:
                    Splash.Play();
                    break;
                default:
                    break;
            }
        }

        private void Update()
        {
            var player = Player.Current;
            if (player == null) return;

            if (player.World.IsBlockAround(BlockType.WATER, player.Position, 10, 10, 10, out var position))
            {
                var vol = 1f - Vector3.Distance(player.Position, position) / 10;
                FastMath.Adjust(0, 1, ref vol);
                WaterWaves.Volume = vol;
                if (WaterWaves.State == SoundState.Playing) return;
                WaterWaves.IsLooped = true;
                WaterWaves.Play();
            }
            else
            {
                WaterWaves.Stop();
            }
        }
    }
}
