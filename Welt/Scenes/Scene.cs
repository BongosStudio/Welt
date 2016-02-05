#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Welt.Controllers;
using Welt.Managers;
using Welt.Models;
using Welt.UI;

namespace Welt.Scenes
{
    public abstract class Scene : DrawableGameComponent
    {
        public float Opacity { get; set; }
        public bool IsEnabled;
        public static TaskManager TaskManager { get; } = new TaskManager();
        public static SceneController Controller;

        protected static bool IsDrawing;
        protected static bool IsPaused;
        
        protected virtual Color BackColor { get; } = Color.CornflowerBlue;
        protected InputController InputController { get; } = InputController.CreateDefault();
        public Dictionary<string, UIComponent> UIComponents { get; } 
            = new Dictionary<string, UIComponent>(32); 

        protected Scene(Game game) : base(game)
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
            foreach (var child in UIComponents.Values)
            {
                child.Initialize();
            }
        }

        public override void Update(GameTime time)
        {          
            SceneUpdate?.Invoke(this, EventArgs.Empty);
            NextUpdate?.Invoke(this, EventArgs.Empty);
            NextUpdate = null;
            TaskManager.Update();
            foreach (var child in UIComponents.Values)
            {
                child.Update(time);
            }
        }

        public new virtual void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(BackColor);
            base.Draw(gameTime);
            foreach (var child in UIComponents.Values.Where(child => child.IsActive))
            {
                child.Draw(gameTime);
            }
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

        public void Schedule(Action action, double ticks)
        {
            TaskManager.Queue(o => action.Invoke(), ticks);
        }

        public void Schedule(Action<object> action, double ticks)
        {
            TaskManager.Queue(action, ticks);
        }

        public void AddComponent(UIComponent component)
        {
            component.Initialize();
            UIComponents.Add(component.Name, component);
        }

        public void RemoveComponent(string name)
        {
            var comp = UIComponents[name];
            comp.Dispose();
            UIComponents.Remove(name);
        }

        public Maybe<UIComponent, NullReferenceException> GetComponent(string name)
        {
            return Maybe<UIComponent, NullReferenceException>.Check(() => UIComponents[name]);
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
            foreach (var comp in UIComponents.Values)
            {
                comp.Dispose();
            }
            base.Dispose(true);
        }
    }
}