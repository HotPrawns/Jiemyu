using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Behaviors.Attack
{
    public class Attack
    {
        public Vector2 TargetSpace { get; set; }
        public uint Damage { get; set; }

        public Attack(Vector2 targetSpace, uint damage)
        {
            this.TargetSpace = targetSpace;
            this.Damage = damage;
        }
    }
}
