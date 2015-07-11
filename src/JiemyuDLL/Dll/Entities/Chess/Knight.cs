using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Entities.Behaviors.Attack;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Chess
{
    public class Knight : Entity
    {
        private static Vector2[] _points = {
                                            new Vector2(1, 2),
                                            new Vector2(1, -2),
                                            new Vector2(2, 1),
                                            new Vector2(2, -1),
                                            new Vector2(-1, 2),
                                            new Vector2(-1, -2),
                                            new Vector2(-2, 1),
                                            new Vector2(-2, -1)
                                           };
        public Knight()
        {
            this.HitPoints = 1;
            this._MoveBehavior = new CustomMoveBehavior(_points);
            this._MoveBehavior.Capabilities = Behaviors.Move.MoveBehavior.MoveCapabilities.Jump;

            this._AttackBehavior = new MoveAttack(this._MoveBehavior);
        }
    }
}
