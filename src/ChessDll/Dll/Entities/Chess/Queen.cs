using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities.Behaviors.Move;
using ChessDll.Entities.Behaviors.Attack;

namespace ChessDll.Entities.Chess
{
    public class Queen : Entity
    {
        public Queen()
        {
            this.HitPoints = 1;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Diagonal | MoveBehavior.MoveTypes.Linear;

            var attackBehavior = new MoveAttack(this._MoveBehavior);
            attackBehavior.Damage = 1;
            this._AttackBehavior = attackBehavior;
        }
    }
}
