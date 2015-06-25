using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities.Behaviors.Move;
using ChessDll.Entities.Behaviors.Attack;

namespace ChessDll.Entities.Jiemyu
{
    public class SoldierA : Entity
    {
        public SoldierA()
        {
            this.HitPoints = 5 ;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Standard;
            this.MoveDistance = 2;

            var attackBehavior = new MoveAttack(this._MoveBehavior);
            attackBehavior.Damage = 1;

            this._AttackBehavior = attackBehavior;
        }
    }
}
