using System;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using System.Collections.Concurrent;

namespace Welt.Components
{
    // taken from https://gist.github.com/raizam/df2f62801ab989dbb55226d911dd917f
   
    /// <summary>
    /// This component provides a simple background threading mecanism, 
    /// and synchronization operation in the main thread when the task is done
    /// </summary>
    public class TaskManagerComponent : GameComponent
    {
        public TaskManagerComponent(Game game) : base(game) { UpdateOrder = -1; }

        readonly ConcurrentQueue<Action> m_ActionQueue = new ConcurrentQueue<Action>();

        public override void Update(GameTime gameTime)
        {
            while (m_ActionQueue.TryDequeue(out var action))
            {
                action();
            }
            base.Update(gameTime);
        }

        public GameTask<T> ExecuteInBackground<T>(Func<T> background, bool longRunning = false)
        {
            var task = new GameTask<T>(this, Task.Factory.StartNew(background, longRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.PreferFairness));
            return task;
        }

        public GameTask ExecuteInBackground(Action background, bool longRunning = false)
        {
            var task = new GameTask(this, Task.Factory.StartNew(background, longRunning ? TaskCreationOptions.LongRunning : TaskCreationOptions.PreferFairness));
            return task;
        }

        #region Nested
        public class GameTask
        {
            internal GameTask(TaskManagerComponent manager, Task task)
            {
                this.Manager = manager;
                this.Task = task;
            }

            readonly protected Task Task;
            readonly protected TaskManagerComponent Manager;

            public void Then(Action<Task> toExecuteInGameThread)
            {
                Task.ContinueWith(t => Manager.m_ActionQueue.Enqueue(() => toExecuteInGameThread(t)), TaskContinuationOptions.ExecuteSynchronously);
            }
        }

        public class GameTask<T> : GameTask
        {
            internal GameTask(TaskManagerComponent manager, Task<T> task) : base(manager, task) { }

            public void Then(Action<Task<T>> toExecuteInGameThread)
            {
                ((Task<T>)Task).ContinueWith(t => Manager.m_ActionQueue.Enqueue(() => toExecuteInGameThread(t)), TaskContinuationOptions.ExecuteSynchronously);
            }
        }
        #endregion
    }
}