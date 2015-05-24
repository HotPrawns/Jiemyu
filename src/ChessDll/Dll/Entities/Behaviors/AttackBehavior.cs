using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Entities.Behaviors
{
    class AttackBehavior
    {
        public AttackBehavior()
        {
            Damage = 1;
        }

        public uint Damage { get; set; }
    }
}
