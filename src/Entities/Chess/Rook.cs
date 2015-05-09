using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors;

namespace ChessDemo.Entities.Chess
{
    class Rook : Entity
    {
        public Rook()
        {
            this.HitPoints = 1;
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Linear;

            var attackBehavior = new MoveAttack(this.MoveBehavior);
            attackBehavior.Damage = 1;
            this.AttackBehavior = attackBehavior;
        }
    }
}
