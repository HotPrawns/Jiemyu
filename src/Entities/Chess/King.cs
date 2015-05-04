using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Chess
{
    class King : Entity
    {
        public King()
        {
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal | MoveBehavior.MoveTypes.Linear;
            this.MoveDistance = 1;
        }
    }
}
