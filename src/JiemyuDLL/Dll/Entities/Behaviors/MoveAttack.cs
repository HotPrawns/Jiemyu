using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jiemyu.Entities.Behaviors
{
    /// <summary>
    /// An attack with an attached move behavior. The move behavior
    /// defines where the attacker can move to
    /// </summary>
    class MoveAttack : AttackBehavior
    {
        public MoveAttack() : base()
        {
        }

        public MoveAttack(MoveBehavior behavior) : base()
        {
            this.MoveBehavior = behavior;
        }

        public MoveBehavior MoveBehavior { get; set; }
    }
}
