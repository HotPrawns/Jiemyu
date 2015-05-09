using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors;

namespace ChessDemo.Entities.Chess
{
    class King : Entity
    {
        public King()
        {
            this.HitPoints = 1;
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal | MoveBehavior.MoveTypes.Linear;
            this.MoveDistance = 1;

            var attackBehavior = new MoveAttack(this.MoveBehavior);
            attackBehavior.Damage = 1;
            this.AttackBehavior = attackBehavior;
        }
    }
}
