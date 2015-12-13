#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;

namespace Welt.Managers
{
    public class TaskManager : IDisposable
    {
        private readonly Queue<GameTask> _tasks;

        public TaskManager()
        {
            _tasks = new Queue<GameTask>();
        }
         
        public void Queue(Action<object> action)
        {
            _tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now});
        }

        public void Queue(Action<object> action, TimeSpan wait)
        {
            _tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.Add(wait)});
        }

        public void Queue(Action<object> action, double ticks)
        {
            _tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.AddMilliseconds(ticks)});
        }

        public void Update()
        {
            for (var i = 0; i < _tasks.Count; i++)
            {
                var task = _tasks.Dequeue();
                if (task.ToBeExecutedAt > DateTime.Now)
                {
                    // queue it back to the line
                    _tasks.Enqueue(task);
                }
                else
                {
                    // execute the event on the main form thread
                    task.Action.Invoke(null);
                }
            }
            _tasks.TrimExcess();
        }


        public struct GameTask
        {
            public Action<object> Action;
            public DateTime ToBeExecutedAt;
        }

        public void Dispose()
        {
            _tasks.Clear();
        }
    }
}
