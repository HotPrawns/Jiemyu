using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        protected MoveBehavior MoveBehavior;

        public Entity()
        {
            MoveBehavior = new MoveBehavior();
            Forward = new Vector2(0, 1); // Default to facing down
        }

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
        public Vector2[] GetAvailableMovements()
        {
            return MoveBehavior.GetAvailableMovements();
        }

        //TODO: Other Non-Chess ones, such as entities that can 'attack'
    }
}
