using Microsoft.Xna.Framework;

namespace Welt.MonoGame.Extended
{
    public class FramesPerSecondCounterComponent : DrawableGameComponent
    {
        FramesPerSecondCounter _fpsCounter;

        public FramesPerSecondCounterComponent(Game game, int maximumSamples = 100)
            : base(game)
        {
            _fpsCounter = new FramesPerSecondCounter(maximumSamples);
        }

        public long TotalFrames
        {
            get { return _fpsCounter.TotalFrames; }
        }

        public float AverageFramesPerSecond
        {
            get { return _fpsCounter.AverageFramesPerSecond; }
        }

        public float CurrentFramesPerSecond
        {
            get { return _fpsCounter.CurrentFramesPerSecond; }
        }

        public int MaximumSamples
        {
            get { return _fpsCounter.MaximumSamples; }
        }

        public void Reset()
        {
            _fpsCounter.Reset();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _fpsCounter.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
            base.Draw(gameTime);
        }
    }
}

