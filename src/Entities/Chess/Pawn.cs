using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Behaviors;
using Microsoft.Xna.Framework;

namespace ChessDemo.Entities.Chess
{
    class Pawn : Entity
    {
        public Pawn()
        {
            this.HitPoints = 1;
            this.MoveDistance = 1;
            this._MoveBehavior.MoveType = MoveBehavior.MoveTypes.Forward;

            // Set up the attack behavior
            var attackBehavior = new MoveAttack();
            attackBehavior.Damage = 1;

            Vector2[] attacks = new Vector2[2];
            attacks[0] = new Vector2(1, 1); // Front Right
            attacks[1] = new Vector2(-1, 1); // Front Left

            attackBehavior.MoveBehavior = new CustomMoveBehavior(attacks);
            attackBehavior.MoveBehavior.MoveDistance = 1;
            attackBehavior.MoveBehavior.MoveType = MoveBehavior.MoveTypes.Forward | MoveBehavior.MoveTypes.Diagonal;

            this._AttackBehavior = attackBehavior;
        }
    }
}
