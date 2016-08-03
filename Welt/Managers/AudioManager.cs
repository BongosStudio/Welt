#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace Welt.Managers
{
    public class AudioManager
    {
        public const string ButtonSound = "Sounds/menu-button";
        public const string WavesSound = "Sounds/waves";

        public const string FeatherSong = "Music/Feather";

        private static Dictionary<string, Song> _songs = new Dictionary<string, Song>();
        private static Dictionary<string, SoundEffectInstance> _sounds = new Dictionary<string, SoundEffectInstance>();  

        public static void Initialize(Game game)
        {
            
        }

        /// <summary>
        ///     Plays a 2 Dimensional sound without respect to any positioning.
        /// </summary>
        /// <param name="name"></param>
        public static void PlaySound(string name)
        {
            if (_sounds.ContainsKey(name)) _sounds[name].Play();
        }

        /// <summary>
        ///     Plays a 2 Dimensional song without respect to any positioning.
        /// </summary>
        /// <param name="name"></param>
        public static void PlaySong(string name)
        {
            if (_songs.ContainsKey(name))
            {
                MediaPlayer.Play(_songs[name]);
            }
        }
    }
}