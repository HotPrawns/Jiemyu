using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Behaviors.Move
{
    class CustomMoveBehavior : MoveBehavior
    {
        private Vector2[] availablePoints;

        public CustomMoveBehavior(Vector2[] availablePoints) : base()
        {
            this.availablePoints = availablePoints;
        }

        public override Vector2[] GetAvailableMovements(int max)
        {
            return this.availablePoints;
        }
    }
}
