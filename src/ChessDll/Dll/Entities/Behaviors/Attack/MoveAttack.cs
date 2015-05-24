using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities.Behaviors.Move;

namespace ChessDll.Entities.Behaviors.Attack
{
    /// <summary>
    /// An attack with an attached move behavior. The move behavior
    /// defines where the attacker can move to
    /// </summary>
    public class MoveAttack : AttackBehavior
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
