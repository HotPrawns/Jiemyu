using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDll.Entities.Behaviors.Attack
{
    public class AttackBehavior
    {
        public AttackBehavior()
        {
            Damage = 1;
        }

        public uint Damage { get; set; }
    }
}
