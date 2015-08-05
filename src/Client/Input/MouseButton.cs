using Jiemyu.Util;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Input
{
    delegate void MouseButtonChanged(MouseButton button);

    class MouseButton
    {
        public event MouseButtonChanged Pressed;
        public event MouseButtonChanged Clicked;
        public event MouseButtonChanged Released;

        public static readonly EventInfo<MouseButton, MouseButtonChanged> PressedEvent = new EventInfo<MouseButton, MouseButtonChanged>((button, handler) => button.Pressed += handler, (button, handler) => button.Pressed -= handler);
        public static readonly EventInfo<MouseButton, MouseButtonChanged> ClickedEvent = new EventInfo<MouseButton, MouseButtonChanged>((button, handler) => button.Clicked += handler, (button, handler) => button.Clicked -= handler);
        public static readonly EventInfo<MouseButton, MouseButtonChanged> ReleasedEvent = new EventInfo<MouseButton, MouseButtonChanged>((button, handler) => button.Released += handler, (button, handler) => button.Released -= handler);

        ButtonState previousState = ButtonState.Released;

        public String Name { get; private set; }
        public MouseButton(String name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Update method that will trigger click events as needed
        /// </summary>
        /// <param name="state"></param>
        public void Update(ButtonState state)
        {
            if (state == ButtonState.Pressed && previousState != ButtonState.Pressed)
            {
                if (Pressed != null)
                {
                    Pressed(this);
                }
            }
            else if (state == ButtonState.Released && previousState == ButtonState.Pressed)
            {
                // For now, define a click as a release. In the future, maybe 
                // have click be more restrictive or even driven by components
                // rather than a generic input handler
                if (Clicked != null)
                {
                    Clicked(this);
                }

                if (Released != null)
                {
                    Released(this);
                }
            }

            previousState = state;
        }
    }
}
