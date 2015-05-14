using ChessDemo.Entities.Behaviors;
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
            this.HitPoints = 1;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal;

            var attackBehavior = new MoveAttack(this._MoveBehavior);
            attackBehavior.Damage = 1;
            this._AttackBehavior = attackBehavior;
        }
    }
}
