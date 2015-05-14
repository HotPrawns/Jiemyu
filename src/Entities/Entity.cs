using ChessDemo.Entities.Behaviors;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace ChessDemo.Entities
{
    class Entity
    {

        /// <summary>
        /// Current location of the Entity
        /// </summary>
        public Point Position;

        /// <summary>
        /// Sprite to draw
        /// </summary>
        public Texture2D EntityTexture;

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
        public int MoveDistance
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
        /// Returns a list of points, relative to the current position of the entity, that 
        /// can be moved to.
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetAvailableMovements(int max)
        {
            return _MoveBehavior.GetAvailableMovements(max);
        }

        /// <summary>
        /// Attacks another entity.
        /// </summary>
        /// <param name="targetEntity"></param>
        public void Attack(Entity targetEntity)
        {
            // For now do a simple calculation for damage
            targetEntity.HitPoints = (int) (targetEntity.HitPoints - this.AttackBehavior.Damage);
        }
    }
}
