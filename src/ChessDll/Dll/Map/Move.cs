using ChessDll.Entities.Behaviors.Move;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace ChessDll.Map
{
    /// <summary>
    /// Provides a convenient wrapper for representing a move option
    /// </summary>
    public class Move
    {
        /// <summary>
        /// Vector that represents the move in map coordinates
        /// +y = down (on map)
        /// +x = right (on map)
        /// </summary>
        public Vector2 Vector { get; private set; }

        /// <summary>
        /// Vector that represents 1 step in the direction of the move
        /// </summary>
        Vector2? _Direction = null;
        public Vector2 DirectionalVector
        {
            get
            {
                if (_Direction == null)
                {
                    float x = (Vector.X == 0) ? 0 : Vector.X / Math.Abs(Vector.X);
                    float y = (Vector.Y == 0) ? 0 : Vector.Y / Math.Abs(Vector.Y);

                    _Direction = new Vector2(x, y);
                }

                return (Vector2)_Direction;
            }
        }

        uint? _Distance = null;
        private Entities.Behaviors.Move.MoveBehavior.MoveCapabilities moveCapabilities;

        public uint Distance
        {
            get
            {
                if (_Distance == null)
                {
                    if (Vector.X != 0)
                    {
                        // For now, just assume that diagonals are treated as one unit.
                        // Thus, if Vector.X and Vector.Y are both non-zero, just use the
                        // x component
                        _Distance = (uint)Math.Abs(Vector.X);
                    }
                    else
                    {
                        _Distance = (uint)Math.Abs(Vector.Y);
                    }

                }

                return (uint)_Distance;
            }
        }

        public Move(Vector2 vector)
        {
            Vector = vector;
        }

        public Move(Vector2 vector, MoveBehavior.MoveCapabilities moveCapabilities)
        {
            Vector = vector;
            this.moveCapabilities = moveCapabilities;
        }

        /// <summary>
        /// Returns true if a given relative point is within this move
        /// </summary>
        /// <param name="relativePoint"></param>
        /// <returns></returns>
        public bool InMove(Vector2 relativePoint)
        {
            if (relativePoint.X == 0 && relativePoint.Y == 0)
            {
                return false;
            }

            // If an entity can jump, just check the end points
            if (this.moveCapabilities.HasFlag(MoveBehavior.MoveCapabilities.Jump))
            {
                return relativePoint == Vector;
            }

            bool inX = InRange(Vector.X, relativePoint.X);
            bool inY = InRange(Vector.Y, relativePoint.Y);
            bool inMove = (inX && inY);
            
            if (relativePoint.X != 0 && relativePoint.Y != 0)
            {
                return inMove && (Math.Abs(relativePoint.X) == Math.Abs(relativePoint.Y));
            }

            return inMove;
        }

        /// <summary>
        /// Easy way to calculate that a given max contains a value
        /// </summary>
        /// <param name="thisMax"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        private bool InRange(float thisMax, float value)
        {
            if (thisMax < 0)
            {
                return (thisMax <= value) && (value < 0);
            }
            else if (thisMax > 0)
            {
                return (thisMax >= value) && (value > 0);
            }
            else
            {
                return (thisMax == value);
            }
        }

        /// <summary>
        /// Returns true if the given point is within a move from the given origin
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="point"></param>
        /// <returns></returns>
        public bool InMove(Vector2 origin, Vector2 point)
        {
            var newPoint = point - origin;
            return InMove(newPoint);
        }

        public bool SameDirection(Move newMove)
        {
            return (newMove.DirectionalVector.X == DirectionalVector.X) && (newMove.DirectionalVector.Y == DirectionalVector.Y);
        }

        public static Vector2 ToMapDirection(Vector2 direction, Vector2 forward)
        {
            // Don't allow for diagonal facing
            Debug.Assert((forward.X != 0) ^ (forward.Y != 0));

            // Facing down (+y)
            if (forward.Y > 0)
            {
                return new Vector2(-1 * direction.X, direction.Y);
            }
            // Facing up (-y)
            else if (forward.Y < 0)
            {
                return new Vector2(direction.X, -1 * direction.Y);
            }
            // Facing right (+x)
            else if (forward.X > 0)
            {
                return new Vector2(direction.Y, direction.X);
            }
            // Facing left (-x)
            else
            {
                return new Vector2(-1 * direction.Y, -1 * direction.X);
            }
        }
    }
}
