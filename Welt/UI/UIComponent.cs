#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace Welt.UI
{
    public abstract class UIComponent
    {
        public virtual string Name { get; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual int Opacity { get; set; }
        public virtual Cursor Cursor { get; set; }
        public virtual BoundsBox Margin { get; set; }
        public virtual BoundsBox Padding { get; set; }
        public virtual HorizontalAlignment HorizontalAlignment { get; set; }
        public virtual VerticalAlignment VerticalAlignment { get; set; }
        public virtual bool IsActive { get; set; }

        public bool IsMouseOver { get; private set; }
        public bool IsLeftMouseDown { get; private set; }
        public bool IsRightMouseDown { get; private set; }

        protected virtual int X { get; set; }
        protected virtual int Y { get; set; }
        protected virtual SpriteBatch Sprite { get; set; }
        protected virtual Texture2D Texture { get; set; }
        protected virtual GraphicsDevice Graphics { get; }
        protected virtual UIComponent Parent { get; }
        protected virtual Dictionary<string, UIComponent> Components { get; }  

        protected UIComponent(string name, int width, int height, GraphicsDevice device) : this(name, width, height, null, device)
        {
            
        }

        protected UIComponent(string name, int width, int height, UIComponent parent, GraphicsDevice device)
        {
            Name = name;
            Width = width;
            Height = height;
            Components = new Dictionary<string, UIComponent>(8); // default size is 8 children.
            Parent = parent;
            Graphics = device;
            Sprite = new SpriteBatch(device);
        }

        protected void ProcessArea()
        {
            float x = X;
            float y = Y;
            float width;
            float height;

            switch (HorizontalAlignment)
            {
                case HorizontalAlignment.Right:
                    width = Parent?.Width ?? WeltGame.Instance.Window.ClientBounds.Width;
                    x = X + (width - Width) - Margin.Right;
                    break;
                case HorizontalAlignment.Center:
                    width = Parent?.Width ?? WeltGame.Instance.Window.ClientBounds.Width;
                    x = X + (width - Width)/2 + Margin.Left - Margin.Right;
                    break;
            }

            switch (VerticalAlignment)
            {
                case VerticalAlignment.Bottom:
                    height = Parent?.Height ?? WeltGame.Instance.Window.ClientBounds.Height;
                    y = Y + (height - Height) - Margin.Bottom;
                    break;
                case VerticalAlignment.Center:
                    height = Parent?.Height ?? WeltGame.Instance.Window.ClientBounds.Height;
                    y = Y + (height - Height)/2 + Margin.Top - Margin.Bottom;
                    break;
            }

            X = (int) x;
            Y = (int) y;
        }

        protected bool GetMouseOver(out MouseState mouse)
        {
            mouse = Mouse.GetState();
            var bounds = new RectangleF(X, Y, Width, Height);
            return bounds.Contains(mouse.X, mouse.Y);
        }

        #region Public Methods

        public virtual void Initialize(Game game, GameTime time)
        {
            // TODO: creation logic here. Mainly spritebatch creation logic.
            foreach (var child in Components.Values)
            {
                child.Initialize(game, time);
            }
        }

        public virtual void Update(GameTime time)
        {
            foreach (var child in Components.Values)
            {
                child.Update(time);
            }

            MouseState mouse;
            if (GetMouseOver(out mouse))
            {
                if (!IsMouseOver)
                {
                    Parent?.MouseEnter?.Invoke(this, null);
                    MouseEnter?.Invoke(this, null);
                    IsMouseOver = true;
                    // change the cursor
                    ((Form) Control.FromHandle(WeltGame.Instance.Window.Handle)).Cursor = Cursor;
                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!IsLeftMouseDown)
                    {
                        Parent?.MouseLeftDown?.Invoke(this, null);
                        MouseLeftDown?.Invoke(this, null);
                        IsLeftMouseDown = true;
                    }
                }
                else
                {
                    if (IsLeftMouseDown)
                    {
                        Parent?.MouseLeftUp?.Invoke(this, null);
                        MouseLeftUp?.Invoke(this, null);
                        IsLeftMouseDown = false;
                    }
                }
                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (!IsRightMouseDown)
                    {
                        Parent?.MouseRightDown?.Invoke(this, null);
                        MouseRightDown?.Invoke(this, null);
                        IsRightMouseDown = true;
                    }
                }
                else
                {
                    if (IsRightMouseDown)
                    {
                        Parent?.MouseRightUp?.Invoke(this, null);
                        MouseRightUp?.Invoke(this, null);
                        IsRightMouseDown = false;
                    }
                }
            }
            else
            {
                if (IsMouseOver)
                {
                    Parent?.MouseLeave?.Invoke(this, null);
                    MouseLeave?.Invoke(this, null);
                    IsMouseOver = false;
                    ((Form) Control.FromHandle(WeltGame.Instance.Window.Handle)).ResetCursor();
                }
                IsRightMouseDown = false;
                IsLeftMouseDown = false;
            }
        }

        public virtual void Draw(GameTime time)
        {
            foreach (var child in Components.Values.Where(child => child.IsActive))
            {
                child.Draw(time);
            }
        }

        public void AddComponent(UIComponent component)
        {
            Components.Add(component.Name, component);
        }

        public void RemoveComponent(string name)
        {
            Components.Remove(name);
        }

        #endregion

        public event EventHandler<MouseEventArgs> MouseEnter;
        public event EventHandler<MouseEventArgs> MouseLeave;
        public event EventHandler<MouseEventArgs> MouseLeftDown;
        public event EventHandler<MouseEventArgs> MouseLeftUp;
        public event EventHandler<MouseEventArgs> MouseRightDown;
        public event EventHandler<MouseEventArgs> MouseRightUp;
    }
}