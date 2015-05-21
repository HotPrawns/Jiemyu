using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors.Move;
using ChessDemo.Entities.Behaviors.Attack;

namespace ChessDemo.Entities.Chess
{
    class King : Entity
    {
        public King()
        {
            this.HitPoints = 1;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal | MoveBehavior.MoveTypes.Linear;
            this.MoveDistance = 1;

            var attackBehavior = new MoveAttack(this._MoveBehavior);
            attackBehavior.Damage = 1;
            this._AttackBehavior = attackBehavior;
        }
    }
}
