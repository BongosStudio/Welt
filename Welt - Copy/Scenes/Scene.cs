#region Copyright

// COPYRIGHT 2015 JUSTIN COX (CONJI)

#endregion Copyright

using EmptyKeys.UserInterface.Controls;
using EmptyKeys.UserInterface.Mvvm;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Welt.Components;
using Welt.Controllers;
using Welt.Extensions;

namespace Welt.Scenes
{
    public abstract class Scene : IDisposable
    {
        public static SceneController Controller;
        /// <summary>
        ///     Cookies that are persisted in between scenes. Cookies from the previous scene
        ///     will be deleted when the next scene is initialized.
        /// </summary>
        private static readonly Dictionary<string, object> m_Cookies = new Dictionary<string, object>();
        protected List<ILogicComponent> Components = new List<ILogicComponent>();

        protected Scene(WeltGame game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            IsEnabled = true;
            Opacity = 1;
            Input = InputController.CreateDefault();
        }

        ~Scene()
        {
            Dispose(false);
        }

        public WeltGame Game { get; set; }
        public GraphicsDevice GraphicsDevice { get; set; }
        public float Opacity { get; set; }
        public bool IsEnabled { get; set; }
        internal virtual ViewModelBase DataContext
        {
            get
            {
                return UI.DataContext as ViewModelBase;
            }
            set
            {
                UI.DataContext = value;
            }
        }
        internal abstract UIRoot UI { get; set; }
        internal virtual Color BackColor { get; } = Color.CornflowerBlue;

        protected InputController Input { get; set; }

        private bool m_IsDisposed;


        public virtual void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public abstract void Draw(GameTime gameTime);

        internal void I_Initialize()
        {
            LoadContent(Game.Content);
            foreach (var comp in Components)
            {
                var c = comp as IVisualComponent;
                c?.LoadContent(Game.Content);
                comp.Initialize();
            }
            WeltGame.SetCursor(Cursors.Arrow);
            if (UI != null)
                UI.DataContext = DataContext;
            m_Cookies.Clear();
            Game.Window.ClientSizeChanged += (sender, args) =>
            {
                UI.Resize(WeltGame.Width, WeltGame.Height);
            };
            Initialize();
        }

        public abstract void Initialize();

        public virtual void OnExiting(object sender, EventArgs args)
        {
        }

        public abstract void Update(GameTime gameTime);

        protected void SetCookie<T>(string key, T cookie) where T : class
        {
            if (m_Cookies.TryGetValue(key, out var value))
                m_Cookies[key] = cookie;
            else
                m_Cookies.Add(key, cookie);
        }

        protected T GetCookie<T>(string key) where T : class
        {
            if (m_Cookies.TryGetValue(key, out var value))
                return (T)value;
            return null;
        }
        

        protected void Dispose(bool disposing)
        {
            if (m_IsDisposed)
                return;
            PreDispose?.Invoke(this, null);
            foreach (var comp in Components)
            {
                comp.Dispose();
            }
            m_IsDisposed = true;
            PostDispose?.Invoke(this, null);
        }

        protected virtual void LoadContent(ContentManager content)
        {
            foreach (var comp in Components)
            {
                var c = comp as IVisualComponent;
                if (c == null) continue;
                c.LoadContent(content);
            }
        }

        protected void Next(Scene scene)
        {
            scene.OnExiting(this, null);
            NextUpdate?.Invoke(this, null);
            SceneController.Load(scene);
        }

        protected virtual void UnloadContent()
        {
        }

        internal void I_Update(GameTime time)
        {
            SceneUpdate?.Invoke(this, EventArgs.Empty);
            foreach (var c in Components)
            {
                c.Update(time);
            }
            Update(time);
            UI.Update(time);
        }

        internal void I_Draw(GameTime time)
        {
            foreach (var c in Components)
            {
                var comp = c as IVisualComponent;
                if (comp == null) continue;
                comp.Draw(time);
            }
            Draw(time);
            UI.Draw(time);
        }

        #region Events

        public event EventHandler NextUpdate;

        public event EventHandler SceneUpdate;

        public event EventHandler PreDispose;

        public event EventHandler PostDispose;

        #endregion Events
    }
}