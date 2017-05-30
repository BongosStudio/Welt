#region Copyright
// COPYRIGHT 2016 JUSTIN COX (CONJI)
#endregion

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Linq;

namespace Welt.Controllers
{
    public delegate bool InputHandler(MultiplayerClient client);

    public class InputController
    {
        public static InputController Instance { get; private set; }
        // keyboard
        private Dictionary<InputAction, InputHandler> m_Handlers = new Dictionary<InputAction, InputHandler>();
        private Dictionary<InputAction, bool> m_Status = new Dictionary<InputAction, bool>();
        private Keys[] m_MappedKeys = new Keys[byte.MaxValue];
        private KeyboardState m_Ks;
        // mouse
        private MouseState m_Ms;
        private bool m_IsMouseRighthanded;
        private TimeSpan m_LeftMouseReset = TimeSpan.Zero;
        private TimeSpan m_RightMouseReset = TimeSpan.Zero;
        private InputHandler m_LeftMouseHandler;
        private InputHandler m_RightMouseHandler;

        public WeltGame Game { get; }

        #region .ctor

        public InputController(WeltGame game)
        {
            Game = game;
            m_MappedKeys[(int)InputAction.Sprint] = game.GameSettings.SprintKey;
            m_MappedKeys[(int)InputAction.MoveForward] = game.GameSettings.MoveForwardKey;
            m_MappedKeys[(int)InputAction.MoveBackward] = game.GameSettings.MoveBackwardKey;
            m_MappedKeys[(int)InputAction.StrafeLeft] = game.GameSettings.StrafeLeftKey;
            m_MappedKeys[(int)InputAction.StrafeRight] = game.GameSettings.StrafeRightKey;
            m_MappedKeys[(int)InputAction.Jump] = game.GameSettings.JumpKey;
            m_MappedKeys[(int)InputAction.Crouch] = game.GameSettings.CrouchKey;
            m_MappedKeys[(int)InputAction.OpenInventory] = game.GameSettings.InventoryKey;
            m_MappedKeys[(int)InputAction.Interact] = game.GameSettings.InteractKey;
            m_MappedKeys[(int)InputAction.Flight] = game.GameSettings.FlightKey;
            m_MappedKeys[(int)InputAction.Hotbar0] = game.GameSettings.Hotbar0;
            m_MappedKeys[(int)InputAction.Hotbar1] = game.GameSettings.Hotbar1;
            m_MappedKeys[(int)InputAction.Hotbar2] = game.GameSettings.Hotbar2;
            m_MappedKeys[(int)InputAction.Hotbar3] = game.GameSettings.Hotbar3;
            m_MappedKeys[(int)InputAction.Hotbar4] = game.GameSettings.Hotbar4;
            m_MappedKeys[(int)InputAction.Hotbar5] = game.GameSettings.Hotbar5;
            m_MappedKeys[(int)InputAction.Hotbar6] = game.GameSettings.Hotbar6;
            m_MappedKeys[(int)InputAction.Hotbar7] = game.GameSettings.Hotbar7;
            m_MappedKeys[(int)InputAction.Hotbar8] = game.GameSettings.Hotbar8;
            m_MappedKeys[(int)InputAction.Hotbar9] = game.GameSettings.Hotbar9;

            m_IsMouseRighthanded = game.GameSettings.IsMouseRightHanded;

            Instance = this;
            m_Ks = Keyboard.GetState();
        }

        public void Update(GameTime gameTime)
        {
            m_Ks = Keyboard.GetState();
            if (m_Ks.GetPressedKeys().Length > 0)
            {
                foreach (var handler in m_Handlers)
                {
                    if (IsInputPressed(handler.Key))
                    {
                        if (!m_Status[handler.Key])
                        {
                            handler.Value?.Invoke(Game.Client);
                            m_Status[handler.Key] = true;
                        }

                    }
                    else
                    {
                        m_Status[handler.Key] = false;
                    }
                }

            }

            m_Ms = Mouse.GetState();
            if (m_LeftMouseReset > TimeSpan.Zero) m_LeftMouseReset -= gameTime.ElapsedGameTime;
            if (m_RightMouseReset > TimeSpan.Zero) m_RightMouseReset -= gameTime.ElapsedGameTime;
            if (m_Ms.LeftButton == ButtonState.Pressed)
            {
                if (m_LeftMouseReset <= TimeSpan.Zero)
                {
                    m_LeftMouseReset = TimeSpan.FromMilliseconds(500);
                    m_LeftMouseHandler?.Invoke(Game.Client);
                }
            }
            else
            {
                m_LeftMouseReset = TimeSpan.Zero;
            }
            if (m_Ms.RightButton == ButtonState.Pressed)
            {
                if (m_RightMouseReset <= TimeSpan.Zero)
                {
                    m_RightMouseReset = TimeSpan.FromMilliseconds(500);
                    m_RightMouseHandler?.Invoke(Game.Client);
                }
            }
            else
            {
                m_RightMouseReset = TimeSpan.Zero;
            }
        }

        public void RegisterHandler(InputHandler handler, InputAction action)
        {
            switch (action)
            {
                case InputAction.Hit:
                    if (m_IsMouseRighthanded)
                        m_LeftMouseHandler = handler;
                    else
                        m_RightMouseHandler = handler;
                    break;
                case InputAction.Place:
                    if (m_IsMouseRighthanded)
                        m_RightMouseHandler = handler;
                    else
                        m_LeftMouseHandler = handler;
                    break;
                default:
                    if (!m_Handlers.ContainsKey(action))
                        m_Handlers.Add(action, handler);
                    else
                        m_Handlers[action] += handler;
                    if (!m_Status.ContainsKey(action))
                        m_Status.Add(action, false);
                    break;
            }
        }

        public void UnregisterHandler(InputHandler handler, InputAction action)
        {
            if (m_Handlers.ContainsKey(action))
                m_Handlers[action] -= handler;
        }

        public Keys GetKeyFor(InputAction action)
        {
            return m_MappedKeys[(int)action];
        }

        public bool IsInputPressed(InputAction action)
        {
            return m_Ks.IsKeyDown(m_MappedKeys[(int)action]);
        }
        
        #endregion
        
        public enum InputAction
        {
            Sprint,
            MoveForward,
            MoveBackward,
            StrafeLeft,
            StrafeRight,
            Jump,
            Crouch,
            OpenInventory,
            Interact,
            Escape,
            Flight,
            Hotbar0, Hotbar1, Hotbar2, Hotbar3, Hotbar4, Hotbar5, Hotbar6, Hotbar7, Hotbar8, Hotbar9,
            Hit,
            Place
        }
    }
}