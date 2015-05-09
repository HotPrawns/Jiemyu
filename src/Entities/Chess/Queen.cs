using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors;

namespace ChessDemo.Entities.Chess
{
    class Queen : Entity
    {
        public Queen()
        {
            this.HitPoints = 1;
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal | MoveBehavior.MoveTypes.Linear;

            var attackBehavior = new MoveAttack(this.MoveBehavior);
            attackBehavior.Damage = 1;
            this.AttackBehavior = attackBehavior;
        }
    }
}
