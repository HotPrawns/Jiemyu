﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Entities.Behaviors.Attack;

namespace JiemyuDll.Entities.Jiemyu
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
