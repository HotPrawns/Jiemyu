using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Entities.Behaviors.Attack;

namespace JiemyuDll.Entities.Jiemyu
{
    public class SoldierA : Entity
    {
        public SoldierA()
        {
            this.HitPoints = 5 ;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Standard;
            this.MoveDistance = 2;
            this.AttackDamage = 1;
            this.AttackRange = 3;
        }
    }
}
