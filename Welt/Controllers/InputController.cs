#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework.Input;

namespace Welt.Controllers
{
    public class InputController
    {
        private KeyboardState _mKeyboard;
        private MouseState _mMouse;

        #region .ctor

        public static InputController CreateDefault()
        {
            // TODO: apply action event mapping
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