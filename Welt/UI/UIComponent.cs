#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Design;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

namespace Welt.UI
{
    public abstract class UIComponent : IDisposable
    {
        public virtual string Name { get; }
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public virtual float Opacity { get; set; } = 1;
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
        protected virtual GraphicsDevice Graphics { get; private set; }
        protected virtual UIComponent Parent { get; private set; }
        protected virtual Dictionary<string, UIComponent> Components { get; }
        protected virtual bool IsSizeProcessed { get; set; }

        protected UIComponent(string name, int width, int height, GraphicsDevice device) : this(name, width, height, null, device)
        {
            
        }

        protected UIComponent(string name, int width, int height, UIComponent parent, GraphicsDevice device)
        {
            Name = name;
            if (parent != null)
            {
                Width = width == -1 ? parent.Width : width;
                Height = height == -1 ? parent.Height : height;
            }
            else
            {
                Width = width == -1 ? WeltGame.Instance.Window.ClientBounds.Width : width;
                Height = height == -1 ? WeltGame.Instance.Window.ClientBounds.Height : height;
            }
            
            Components = new Dictionary<string, UIComponent>(8); // default size is 8 children.
            Parent = parent;
            Graphics = device;
            Sprite = new SpriteBatch(device);
            
        }

        protected void ProcessArea()
        {
            if (IsSizeProcessed) return;
            IsSizeProcessed = true;
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
                case HorizontalAlignment.Left:
                    x = X + Margin.Left;
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
                case VerticalAlignment.Top:
                    y = Y + Margin.Top;
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

        public virtual void Initialize()
        {
            IsActive = true;
            ProcessArea();
            // TODO: creation logic here. Mainly spritebatch creation logic.
            foreach (var child in Components.Values)
            {
                child.Parent = this;
                child.Initialize();
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
                // TODO: ARGS for the mouse
                if (!IsMouseOver)
                {

                    Parent?.MouseEnter?.Invoke(this,
                        new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                    MouseEnter?.Invoke(this,
                        new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                    IsMouseOver = true;
                    // change the cursor
                    ((Form) Control.FromHandle(WeltGame.Instance.Window.Handle)).Cursor = Cursor;
                }
                if (mouse.LeftButton == ButtonState.Pressed)
                {
                    if (!IsLeftMouseDown)
                    {
                        Parent?.MouseLeftDown?.Invoke(this,
                            new MouseEventArgs(MouseButtons.Left, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        MouseLeftDown?.Invoke(this,
                            new MouseEventArgs(MouseButtons.Left, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        IsLeftMouseDown = true;
                    }
                }
                else
                {
                    if (IsLeftMouseDown)
                    {
                        Parent?.MouseLeftUp?.Invoke(this,
                            new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        MouseLeftUp?.Invoke(this,
                            new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        IsLeftMouseDown = false;
                    }
                }

                if (mouse.RightButton == ButtonState.Pressed)
                {
                    if (!IsRightMouseDown)
                    {
                        Parent?.MouseRightDown?.Invoke(this,
                            new MouseEventArgs(MouseButtons.Right, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        MouseRightDown?.Invoke(this,
                            new MouseEventArgs(MouseButtons.Right, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        IsRightMouseDown = true;
                    }
                }
                else
                {
                    if (IsRightMouseDown)
                    {
                        Parent?.MouseRightUp?.Invoke(this,
                            new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        MouseRightUp?.Invoke(this,
                            new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                        IsRightMouseDown = false;
                    }
                }
            }
            else
            {
                if (IsMouseOver)
                {
                    Parent?.MouseLeave?.Invoke(this,
                        new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
                    MouseLeave?.Invoke(this,
                        new MouseEventArgs(MouseButtons.None, 0, mouse.X, mouse.Y, mouse.ScrollWheelValue));
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

        #region Private Methods
        


        #endregion

        public event EventHandler<MouseEventArgs> MouseEnter;
        public event EventHandler<MouseEventArgs> MouseLeave;
        public event EventHandler<MouseEventArgs> MouseLeftDown;
        public event EventHandler<MouseEventArgs> MouseLeftUp;
        public event EventHandler<MouseEventArgs> MouseRightDown;
        public event EventHandler<MouseEventArgs> MouseRightUp;

        public static UIProperty OpacityProperty = new UIProperty("opacity");
        public static UIProperty WidthProperty = new UIProperty("width");
        public static UIProperty HeightProperty = new UIProperty("height");

        public object GetPropertyValue(UIProperty property)
        {
            var prop = GetType().GetProperties().First(p => p.Name.ToLower() == property.Name.ToLower());
            return prop.GetValue(this, null);
        }

        public void SetPropertyValue(UIProperty property, object value)
        {
            var prop = GetType().GetProperties().First(p => p.Name.ToLower() == property.Name.ToLower());
            prop.SetValue(this, value, BindingFlags.IgnoreCase, null, null, null);
        }
        
        public virtual void Dispose()
        {
            MouseEnter = null;
            MouseLeave = null;
            MouseLeftDown = null;
            MouseLeftUp = null;
            MouseRightDown = null;
            MouseRightUp = null;
            foreach (var child in Components)
            {
                child.Value.Dispose();
            }
            Components.Clear();
            Parent = null;
            Graphics = null;
        }
    }
}