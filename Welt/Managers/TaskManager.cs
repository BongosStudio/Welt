#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;

namespace Welt.Managers
{
    public class TaskManager : IDisposable
    {
        private readonly Queue<GameTask> m_tasks;

        public TaskManager()
        {
            m_tasks = new Queue<GameTask>();
        }
         
        public void Queue(Action<object> action)
        {
            m_tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now});
        }

        public void Queue(Action<object> action, TimeSpan wait)
        {
            m_tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.Add(wait)});
        }

        public void Queue(Action<object> action, double ticks)
        {
            m_tasks.Enqueue(new GameTask {Action = action, ToBeExecutedAt = DateTime.Now.AddMilliseconds(ticks)});
        }

        public void Update()
        {
            for (var i = 0; i < m_tasks.Count; i++)
            {
                var task = m_tasks.Dequeue();
                if (task.ToBeExecutedAt > DateTime.Now)
                {
                    // queue it back to the line
                    m_tasks.Enqueue(task);
                }
                else
                {
                    // execute the event on the main form thread
                    task.Action.Invoke(null);
                }
            }
            m_tasks.TrimExcess();
        }


        public struct GameTask
        {
            public Action<object> Action;
            public DateTime ToBeExecutedAt;
        }

        public void Dispose()
        {
            m_tasks.Clear();
        }
    }
}
