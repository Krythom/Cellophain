using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace Cellophain
{
    //Class to handle detecting if buttons were just pressed vs held
    class InputHandler
    {
        private KeyboardState previousState;
        private KeyboardState currentState;
        private MouseState previousMouse;
        private MouseState currentMouse;

        public bool KeyHeld(Keys key)
        {
            return currentState.IsKeyDown(key);
        }

        public bool KeyPressed(Keys key)
        {
            return currentState.IsKeyDown(key) && !previousState.IsKeyDown(key);
        }

        public bool KeyReleased(Keys key)
        {
            return !currentState.IsKeyDown(key) && previousState.IsKeyDown(key);
        }
        public bool LeftClick()
        {
            return currentMouse.LeftButton == ButtonState.Pressed && previousMouse.LeftButton == ButtonState.Released;
        }

        public bool RightClick()
        {
            return currentMouse.RightButton == ButtonState.Pressed && previousMouse.RightButton == ButtonState.Released;
        }

        public bool MWheelUp()
        {
            return currentMouse.ScrollWheelValue > previousMouse.ScrollWheelValue;
        }

        public bool MWheelDown()
        {
            return currentMouse.ScrollWheelValue < previousMouse.ScrollWheelValue;
        }

        public void Update()
        {
            previousState = currentState;
            currentState = Keyboard.GetState();
            previousMouse = currentMouse;
            currentMouse = Mouse.GetState();
        }
    }
}
