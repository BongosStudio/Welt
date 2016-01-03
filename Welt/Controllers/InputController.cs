#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using Microsoft.Xna.Framework.Input;

namespace Welt.Controllers
{
    public class InputController
    {
        private KeyboardState m_keyboard;
        private MouseState m_mouse;

        #region .ctor

        public static InputController CreateDefault()
        {
            // TODO: apply action event mapping
            return new InputController();
        }

        #endregion

        public MouseState GetMouseState()
        {
            m_mouse = Mouse.GetState();
            return m_mouse;
        }

        public KeyboardState GetKeyboardState()
        {
            m_keyboard = Keyboard.GetState();
            return m_keyboard;
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