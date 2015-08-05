using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Input
{
    public delegate void MouseEventHandler(MouseProcessor sender, String button);

    public class MouseProcessor
    {
        public event MouseEventHandler Clicked;

        private bool leftPressed;

        private bool rightPressed;

        public void Update(MouseState state)
        {
            bool leftPressedThisFrame = (state.LeftButton == ButtonState.Pressed);
            bool leftWasPressed = leftPressed;
            bool rightPressedThisFrame = (state.RightButton == ButtonState.Pressed);
            bool rightWasPressed = rightPressed;

            leftPressed = leftPressedThisFrame;
            rightPressed = rightPressedThisFrame;


            if (leftPressedThisFrame && !leftWasPressed)
            {
                Clicked(this, "LEFT");
            }

            if (rightPressedThisFrame && !rightWasPressed)
            {
                Clicked(this, "RIGHT");
            }
        }
    }
}
