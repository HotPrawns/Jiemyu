using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Chess
{
    class Pawn : Entity
    {
        public Pawn()
        {
            this.MoveDistance = 1;
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Forward;
        }
    }
}
