using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Threading;
using Welt.Extensions;
using Welt.API;
using Welt.API.Forge;
using Welt.Core;
using Welt.Core.Forge;

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

        public Song[] Songs;
        private bool m_IsSongPlaying;

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
                while (Game.IsRunning)
                {
                    if (MediaPlayer.State != MediaState.Playing)
                    {
                        var willPlay = FastMath.NextRandom(1000) < 1;
                        if (willPlay)
                        {
                            var song = Songs[FastMath.NextRandom(Songs.Length)];
                            MediaPlayer.Play(song);
                        }
                    }
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
            //WaterWaves = content.Load<SoundEffect>("Sounds\\waves").CreateInstance();
            Songs = new Song[]
            {
                content.Load<Song>("Music\\Feather"),
                content.Load<Song>("Music\\snowfall")
            };
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
            
        }
    }
}
