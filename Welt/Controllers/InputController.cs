#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Welt.MonoGame.Extended.InputListeners;

namespace Welt.Controllers
{
    public class InputController
    {
        private static readonly Dictionary<InputAction, Action> Actions = new Dictionary<InputAction, Action>();
        private KeyboardState _mKeyboard;
        private MouseState _mMouse;

        private readonly InputMapping _mapping;

        #region .ctor

        public static InputController CreateDefault()
        {
            // TODO: apply action event mapping
            Actions.Clear();

            return new InputController(InputMapping.Default);
        }

        public InputController(InputMapping mapping)
        {
            _mapping = mapping;
        }

        #endregion

        public MouseState GetMouseState()
        {
            return _mMouse;
        }

        public KeyboardState GetKeyboardState()
        {
            return _mKeyboard;
        }

        public void Update(GameTime time)
        {
            var k = GetKeyboardState();
            var m = GetMouseState();
            if (_mKeyboard == k) return;

            if (k != _mKeyboard)
            {
                foreach (var key in _mKeyboard.GetPressedKeys())
                {
                    Actions[_mapping.GetKeyAction(key)].Invoke();
                }
                _mKeyboard = k;
            }
            if (_mMouse.LeftButton == ButtonState.Pressed)
                Actions[_mapping.GetMouseAction(MouseButton.Left)].Invoke();
            if (_mMouse.RightButton == ButtonState.Pressed)
                Actions[_mapping.GetMouseAction(MouseButton.Right)].Invoke();
            if (_mMouse.MiddleButton == ButtonState.Pressed)
                Actions[_mapping.GetMouseAction(MouseButton.Middle)].Invoke();
            _mMouse = m;
        }

        public void Assign(Action action, InputAction input)
        {
            Actions.Add(input, action);
        }

        public void Clear()
        {
            Actions.Clear();
        }

        public enum InputAction
        {
            Empty,
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

        public class InputMapping
        {
            protected readonly InputAction[] KeyMap;
            protected readonly InputAction[] MouseMap;
            protected readonly InputAction[] ControllerMap;

            public InputMapping()
            {
                KeyMap = new InputAction[255];
                MouseMap = new InputAction[255]; // TODO: determine how many buttons the mouse actually has?
                ControllerMap = new InputAction[0]; // TODO:
            }

            public void SetKey(InputAction action, Keys key)
            {
                KeyMap[(byte) key] = action;
            }

            public InputAction GetKeyAction(Keys key)
            {
                return KeyMap[(byte) key];
            }

            public void SwapKey(InputAction left, InputAction right)
            {              
                for (var b = 0;; b++)
                {
                    if (KeyMap[b] != left) continue;
                    for (var a = 0;; a++)
                    {
                        if (KeyMap[a] != right) continue;
                        KeyMap[b] = right;
                        KeyMap[a] = left;
                    }
                }
            }

            public void SetMouse(InputAction action, MouseButton mouse)
            {
                MouseMap[(byte) mouse] = action;
            }

            public InputAction GetMouseAction(MouseButton mouse)
            {
                return MouseMap[(byte) mouse];
            }

            public void SwapMouse(InputAction left, InputAction right)
            {
                for (var b = 0; ; b++)
                {
                    if (MouseMap[b] != left) continue;
                    for (var a = 0; ; a++)
                    {
                        if (MouseMap[a] != right) continue;
                        MouseMap[b] = right;
                        MouseMap[a] = left;
                    }
                }
            }

            public static InputMapping Default
            {
                get
                {
                    var map = new InputMapping();

                    map.MouseMap[(byte) MouseButton.Left] = InputAction.Break;
                    map.MouseMap[(byte) MouseButton.Right] = InputAction.Use;

                    map.KeyMap[(byte) Keys.LeftShift] = InputAction.Sprint;
                    map.KeyMap[(byte) Keys.LeftControl] = InputAction.Crouch;
                    map.KeyMap[(byte) Keys.W] = InputAction.MoveForward;
                    map.KeyMap[(byte) Keys.A] = InputAction.StrafeLeft;
                    map.KeyMap[(byte) Keys.D] = InputAction.StrafeRight;
                    map.KeyMap[(byte) Keys.S] = InputAction.MoveBackward;
                    map.KeyMap[(byte) Keys.Escape] = InputAction.Escape;

                    return map;
                }
            }
        }
    }
}