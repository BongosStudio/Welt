#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;

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
            _mTasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now});
        }

        public void Queue(Action<object> action, TimeSpan wait)
        {
            _mTasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.Add(wait)});
        }

        public void Queue(Action<object> action, double ticks)
        {
            _mTasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.AddMilliseconds(ticks)});
        }

        public void Update()
        {
            for (var i = 0; i < _mTasks.Count; i++)
            {
                var task = _mTasks.Dequeue();
                if (task.ToBeExecutedAt > DateTime.Now)
                {
                    // queue it back to the line
                    _mTasks.Enqueue(task);
                }
                else
                {
                    // execute the event on the main form thread
                    task.Action.Invoke(null);
                }
            }
            _mTasks.TrimExcess();
        }


        public struct GameTask
        {
            public Action<object> Action;
            public DateTime ToBeExecutedAt;
        }

        public void Dispose()
        {
            _mTasks.Clear();
        }
    }
}
