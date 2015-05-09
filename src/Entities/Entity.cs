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
        protected MoveBehavior MoveBehavior;

        /// <summary>
        /// 
        /// </summary>
        protected AttackBehavior AttackBehavior;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Entity()
        {
            AttackBehavior = new AttackBehavior();
            MoveBehavior = new MoveBehavior();
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
                return MoveBehavior.MoveDistance;
            }
            set 
            {
                MoveBehavior.MoveDistance = value;
            }
        }

        /// <summary>
        /// Returns a list of points, relative to the current position of the entity, that 
        /// can be moved to.
        /// </summary>
        /// <returns></returns>
        public Vector2[] GetAvailableMovements(int max)
        {
            return MoveBehavior.GetAvailableMovements(max);
        }
    }
}
