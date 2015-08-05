using Jiemyu.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Jiemyu.Input
{
    public delegate void MousemoveEventHandler(Point newLocation);

    class TheMouse : GameComponent
    {
        private static TheMouse _instance;

        public static TheMouse Instance()
        {
            return _instance;
        }

        public static void SetInstance(TheMouse mouse)
        {
            Debug.Assert(_instance == null);
            _instance = mouse;
        }

        private Point lastPosition = new Point();

        public MouseButton LeftButton { get; private set; }
        public MouseButton RightButton { get; private set; }
        public MouseButton MiddleButton { get; private set; }

        public event MousemoveEventHandler MouseMove;
        public static readonly EventInfo<TheMouse, MousemoveEventHandler> MouseMoveEvent = new EventInfo<TheMouse, MousemoveEventHandler>((mouse, handler) => mouse.MouseMove += handler, (mouse, handler) => mouse.MouseMove -= handler);

        /// <summary>
        /// Private constructor to enforce use of singleton
        /// </summary>
        internal TheMouse(Game game) : base(game)
        {
            this.LeftButton = new MouseButton("Left");
            this.RightButton = new MouseButton("Right");
            this.MiddleButton = new MouseButton("Middle");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            var currentState = Mouse.GetState();

            LeftButton.Update(currentState.LeftButton);
            RightButton.Update(currentState.RightButton);
            MiddleButton.Update(currentState.MiddleButton);

            var newPoint = new Point(currentState.X, currentState.Y);

            if (!newPoint.Equals(lastPosition))
            {
                MouseMove(newPoint);
                lastPosition = newPoint;
            }
        }
    }
}
