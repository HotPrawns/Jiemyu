using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Input
{
    public delegate void MouseEventHandler(MouseProcessor sender);

    public class MouseProcessor
    {
        public event MouseEventHandler Clicked;

        private bool pressed;

        public void Update(MouseState state)
        {
            bool pressedThisFrame = (state.LeftButton == ButtonState.Pressed);
            bool wasPressed = pressed;

            pressed = pressedThisFrame;

            if (pressedThisFrame && !wasPressed)
            {
                Clicked(this);
            }
        }
    }
}
