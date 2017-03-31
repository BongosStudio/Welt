using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Welt.Components;

namespace Welt.Effects
{
    public class AnimationBoard : ILogicComponent
    {
        public WeltGame Game { get; }

        protected List<IWeltAnimation> Animations;

        public AnimationBoard(WeltGame game)
        {
            Game = game;
            Animations = new List<IWeltAnimation>();
        }

        public void Dispose()
        {
            Animations.Clear();
        }

        public void Initialize()
        {

        }

        public void Update(GameTime gameTime)
        {
            Parallel.ForEach(Animations, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                animation =>
                {
                    
                });
        }
    }
}
