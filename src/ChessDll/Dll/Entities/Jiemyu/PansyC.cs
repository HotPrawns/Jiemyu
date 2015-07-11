using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities.Behaviors.Move;
using ChessDll.Entities.Behaviors.Attack;

namespace ChessDll.Entities.Jiemyu
{
    public class PansyC : Entity
    {
        public PansyC()
        {
            this.HitPoints = 1;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Standard;
            this.MoveDistance = 3;

            var attackBehavior = new MoveAttack(this._MoveBehavior);
            attackBehavior.Damage = 1;

            this._AttackBehavior = attackBehavior;
        }
    }
}
