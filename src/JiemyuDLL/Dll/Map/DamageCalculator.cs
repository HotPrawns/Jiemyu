using JiemyuDll.Entities;
using JiemyuDll.Entities.Behaviors.Attack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Map
{
    public sealed class DamageCalculator
    {
        /// <summary>
        /// Calculates damage and performs attack by performer upon target
        /// </summary>
        /// <param name="performer"></param>
        /// <param name="target"></param>
        /// <param name="attack"></param>
        public static void performAttack(Entity performer, Entity targetEntity, Attack attack)
        {
            targetEntity.HitPoints = (int)(targetEntity.HitPoints - attack.Damage);    
        }
    }
}
