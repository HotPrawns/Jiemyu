using JiemyuDll.Entities.Behaviors.Attack;
using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace JiemyuDll.Entities
{
    public class Entity
    {
        /// <summary>
        /// Unique identifier for this entity
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Current location of the Entity
        /// </summary>
        public Point Position;

        /// <summary>
        /// Sprite to draw
        /// </summary>
        public Texture2D EntityTexture { get; set; }

        /// <summary>
        /// Vector to represent which way the entity is face in map coordinates. Positive Y is down, positive X is right
        /// </summary>
        public Vector2 Forward { get; set; }

        /// <summary>
        /// 
        /// </summary>
        protected MoveBehavior _MoveBehavior;

        /// <summary>
        /// Public read-only access to MoveBehavior
        /// </summary>
        public MoveBehavior MoveBehavior { get { return _MoveBehavior; } }

        /// <summary>
        /// 
        /// </summary>
        protected AttackBehavior _AttackBehavior;

        /// <summary>
        /// Public read-only access to AttackBehavior
        /// </summary>
        public AttackBehavior AttackBehavior { get { return _AttackBehavior; } }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Entity()
        {
            _AttackBehavior = new AttackBehavior();
            _MoveBehavior = new MoveBehavior();
            Forward = new Vector2(0, 1); // Default to facing down
        }

        /// <summary>
        /// Total HitPoints for a unit
        /// </summary>
        public int HitPoints { get; set; }

        /// <summary>
        /// How far an entity can move, based on MoveBehavior
        /// </summary>
        public uint MoveDistance
        {
            get
            {
                return _MoveBehavior.MoveDistance;
            }
            set 
            {
                _MoveBehavior.MoveDistance = value;
            }
        }

        /// <summary>
        /// How far an entity can attack, based on AttackBehavior
        /// </summary>
        public uint AttackRange
        {
            get
            {
                return _AttackBehavior.Range;
            }
            set
            {
                _AttackBehavior.Range = value;
            }
        }

        /// <summary>
        /// How far an entity can attack, based on AttackBehavior
        /// </summary>
        public uint AttackDamage
        {
            get
            {
                return _AttackBehavior.Damage;
            }
            set
            {
                _AttackBehavior.Damage = value;
            }
        }
    }
}
