using System;
using System.Collections.Generic;

namespace Welt.MonoGame.Extended.Animations.Tweens
{
    public abstract class TweenAnimation<T> : Animation
    {
        protected TweenAnimation(T target, Action onCompleteAction = null, bool disposeOnComplete = true)
            : base(onCompleteAction, disposeOnComplete)
        {
            Target = target;
            Tweens = new List<Animation>();
        }

        public T Target { get; }
        public IList<Animation> Tweens { get; }


    }
}