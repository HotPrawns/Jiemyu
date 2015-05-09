using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors;

namespace ChessDemo.Entities.Chess
{
    class Pawn : Entity
    {
        public Pawn()
        {
            this.HitPoints = 1;
            this.MoveDistance = 1;
            this.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Forward;

            // Set up the attack behavior
            var attackBehavior = new MoveAttack();
            attackBehavior.Damage = 1;
            attackBehavior.MoveBehavior = new MoveBehavior();
            attackBehavior.MoveBehavior.MoveDistance = 1;
            attackBehavior.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Forward | MoveBehavior.MoveTypes.Diagonal;

            this.AttackBehavior = attackBehavior;
        }
    }
}
