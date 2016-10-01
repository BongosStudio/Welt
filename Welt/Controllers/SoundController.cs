using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Welt.Controllers
{
    public class SoundController
    {
        public static SoundController Instance { get; private set; }

        public SoundController()
        {
            Instance = this;
        }

        public void PlayWalk(ushort id)
        {

        }
    }
}
