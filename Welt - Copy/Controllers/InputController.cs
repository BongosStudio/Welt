#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Welt.Controllers
{
    public class InputController
    {
        private static readonly Dictionary<Keys, Action> m_Action = new Dictionary<Keys, Action>();

        private double m_KeyCooldown;
        private Keys m_LastKey;
        private KeyboardState m_Keyboard;
        private MouseState m_Mouse;

        #region .ctor

        public static InputController CreateDefault()
        {
            // TODO: apply action event mapping
            m_Action.Clear();
            return new InputController();
        }

        #endregion

        public MouseState GetMouseState()
        {
            m_Mouse = Mouse.GetState();
            return m_Mouse;
        }

        public KeyboardState GetKeyboardState()
        {
            m_Keyboard = Keyboard.GetState();
            return m_Keyboard;
        }

        public void Update(GameTime time)
        {
            GetKeyboardState();
            m_KeyCooldown -= time.ElapsedGameTime.TotalMilliseconds;
            foreach (var key in m_Keyboard.GetPressedKeys())
            {
                if (m_Action.TryGetValue(key, out var action))
                {
                    if (key != m_LastKey)
                    {
                        m_KeyCooldown = 0;
                    }
                    
                    if (m_KeyCooldown <= 0)
                        action.Invoke();
                    m_KeyCooldown = 250; // we'll make it 250ms
                    m_LastKey = key;
                }
            }
        }

        public void Assign(Action action, Keys keys)
        {
            m_Action.Add(keys, action);
        }

        public void Clear()
        {
            m_Action.Clear();
        }

        public enum InputAction
        {
            Break,
            Use,
            Sprint,
            MoveForward,
            MoveBackward,
            StrafeLeft,
            StrafeRight,
            Jump,
            Crouch,
            OpenInventory,
            Escape,

        }
    }
}