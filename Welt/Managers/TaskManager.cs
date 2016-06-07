#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Welt.Managers
{
    public class TaskManager : IDisposable
    {
        private readonly Queue<GameTask> _mTasks;

        public TaskManager()
        {
            _mTasks = new Queue<GameTask>();
        }

        public void Queue(Action<object> action)
        {
            _mTasks.Enqueue(new GameTask { Action = action, Wait = TimeSpan.Zero });
        }

        public void Queue(Action<object> action, TimeSpan wait)
        {
            _mTasks.Enqueue(new GameTask { Action = action, Wait = wait });
        }

        public void Queue(Action<object> action, long ticks)
        {
            _mTasks.Enqueue(new GameTask { Action = action, Wait = TimeSpan.FromTicks(ticks) });
        }

        public void Update(GameTime time)
        {
            for (var i = 0; i < _mTasks.Count; i++)
            {
                var task = _mTasks.Dequeue();
                if (task.Wait > TimeSpan.Zero)
                {
                    // queue it back to the line
                    task.Wait -= time.ElapsedGameTime;
                    _mTasks.Enqueue(task);
                }
                else
                {
                    // execute the event on the main form thread
                    task.Action.Invoke(this);
                }
            }
            _mTasks.TrimExcess();
        }


        public struct GameTask
        {
            public Action<object> Action;
            public TimeSpan Wait;
        }

        public void Dispose()
        {
            _mTasks.Clear();
        }
    }
}
