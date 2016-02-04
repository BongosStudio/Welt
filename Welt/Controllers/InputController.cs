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
        private static readonly Dictionary<Keys[], Action> _actions = new Dictionary<Keys[], Action>(); 

        private KeyboardState _mKeyboard;
        private MouseState _mMouse;

        #region .ctor

        public static InputController CreateDefault()
        {
            // TODO: apply action event mapping
            _actions.Clear();
            return new InputController();
        }

        #endregion

        public MouseState GetMouseState()
        {
            _mMouse = Mouse.GetState();
            return _mMouse;
        }

        public KeyboardState GetKeyboardState()
        {
            _mKeyboard = Keyboard.GetState();
            return _mKeyboard;
        }

        public void Update(GameTime time)
        {
            GetKeyboardState();
            if (_actions.ContainsKey(_mKeyboard.GetPressedKeys()))
            {
                _actions[_mKeyboard.GetPressedKeys()].Invoke();
            }
        }

        public void Assign(Action action, params Keys[] keys)
        {
            _actions.Add(keys, action);
        }

        public void Clear()
        {
            _actions.Clear();
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