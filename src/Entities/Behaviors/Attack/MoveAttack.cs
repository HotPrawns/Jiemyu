using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors.Move;

namespace ChessDemo.Entities.Behaviors.Attack
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
