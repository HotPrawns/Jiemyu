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

        protected MoveBehavior MoveBehavior;

        public Entity()
        {
            MoveBehavior = new MoveBehavior();
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
