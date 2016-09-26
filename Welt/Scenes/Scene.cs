#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Controllers;
using Welt.Managers;
using Welt.Models;
using Keys = Microsoft.Xna.Framework.Input.Keys;

namespace Welt.Scenes
{
    public abstract class Scene : DrawableGameComponent
    {
        internal abstract UIRoot UI { get; }
        internal abstract ViewModelBase DataContext { get; set; }

        public float Opacity { get; set; }
        public bool IsEnabled;
        public static TaskManager TaskManager { get; } = new TaskManager();
        public static SceneController Controller;
        
        protected virtual Color BackColor { get; } = Color.CornflowerBlue;
        protected InputController InputController { get; } = InputController.CreateDefault();

        protected Scene(WeltGame game) : base(game)
        {
            IsEnabled = true;
            Opacity = 1;
        }

        public virtual void OnExiting(object sender, EventArgs args)
        {
            
        }

        public override void Initialize()
        {
            base.Initialize();
            WeltGame.SetCursor(Cursors.Arrow);
            UI.DataContext = DataContext;
            WeltGame.SetUI(UI, DataContext);
        }

        public new virtual void Update(GameTime time)
        {
            SceneUpdate?.Invoke(this, EventArgs.Empty);
            InputController.Update(time);
            TaskManager.Update(time);
            //UI.Update(time);
        }
        
        public new virtual void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackColor);

            //UI.Draw(gameTime);
            base.Draw(gameTime);
        }

        public void Schedule(Action action)
        {
            TaskManager.Queue(o => action.Invoke());
        }

        public void Schedule(Action<object> action)
        {
            TaskManager.Queue(action);
        }

        public void Schedule(Action action, TimeSpan when)
        {
            TaskManager.Queue(o => action.Invoke(), when);
        }

        public void Schedule(Action<object> action, TimeSpan when)
        {
            TaskManager.Queue(action, when);
        }

        public void Schedule(Action action, long ticks)
        {
            TaskManager.Queue(o => action.Invoke(), ticks);
        }

        public void Schedule(Action<object> action, long ticks)
        {
            TaskManager.Queue(action, ticks);
        }

        protected void AssignKeyToEvent(Action action, InputController.InputAction check)
        {
            InputController.Assign(action, check);
        }

        #region Events
        
        public event EventHandler SceneUpdate;
        public event EventHandler NextUpdate; 

        #endregion

        ~Scene()
        {
            Dispose();
        }

        public new void Dispose()
        {
            base.Dispose(true);
        }
    }
}