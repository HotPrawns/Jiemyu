using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Chess
{
    class Rook : Entity
    {
        public Rook()
        {
            this.MoveBehavior.MoveType = Entities.MoveBehavior.MoveTypes.Linear;
        }
    }
}
