#region Copyright
// COPYRIGHT 2015 JUSTIN COX (CONJI)
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Welt.Controllers;
using Welt.Managers;
using Welt.Models;
using Welt.UI;
using Keys = Microsoft.Xna.Framework.Input.Keys;

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
            = new Dictionary<string, UIComponent>(64);

        private KeyboardState _previousKeyState;
        private readonly Dictionary<Keys[], Func<bool>> _keyMap; 

        protected Scene(Game game) : base(game)
        {
            IsEnabled = true;
            Opacity = 1;
            _keyMap = new Dictionary<Keys[], Func<bool>>();
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
            _previousKeyState = Keyboard.GetState();
            WeltGame.SetCursor(Cursors.Arrow);
        }

        public new virtual void Update(GameTime time)
        {          
            SceneUpdate?.Invoke(this, EventArgs.Empty);
            var currentKeyState = Keyboard.GetState();
            if (currentKeyState != _previousKeyState)
            {
                var pressedKeys = currentKeyState.GetPressedKeys();
                
                foreach (
                    var kvp in
                        _keyMap.Where(
                            kvp =>
                                kvp.Key.All(key => pressedKeys.Contains(key)) && kvp.Key.Length == pressedKeys.Length))
                {
                    kvp.Value.Invoke();
                }
                _previousKeyState = currentKeyState;
            }

            TaskManager.Update(time);

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

        public void Schedule(Action action, long ticks)
        {
            TaskManager.Queue(o => action.Invoke(), ticks);
        }

        public void Schedule(Action<object> action, long ticks)
        {
            TaskManager.Queue(action, ticks);
        }

        public void AddComponent(UIComponent component)
        {
            //component.Initialize();
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

        public Maybe<TComponent, NullReferenceException> GetComponent<TComponent>(string name) where TComponent : UIComponent
        {
            return Maybe<TComponent, NullReferenceException>.Check(() => (TComponent) UIComponents[name]);
        }

        protected void AssignKeyToEvent(Func<bool> func, params Keys[] keys)
        {
            _keyMap.Add(keys, func);
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