using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Chess
{
    class Bishop : Entity
    {
        public Bishop()
        {
            this.MoveBehavior.MoveType = Entities.MoveBehavior.MoveTypes.Diagonal;
        }
    }
}
