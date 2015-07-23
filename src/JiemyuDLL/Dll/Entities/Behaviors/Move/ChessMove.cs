using JiemyuDll.Entities.Behaviors.Move;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Behaviors.Move
{
    /// <summary>
    /// Provides a convenient wrapper for representing a move option
    /// </summary>
    public class ChessMove
    {
        float X { get; set; }
        float Y { get; set; }

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
                    float x = (this.X == 0) ? 0 : this.X / Math.Abs(this.X);
                    float y = (this.Y == 0) ? 0 : this.Y / Math.Abs(this.Y);

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
                    if (this.X != 0)
                    {
                        // For now, just assume that diagonals are treated as one unit.
                        // Thus, if Vector.X and Vector.Y are both non-zero, just use the
                        // x component
                        _Distance = (uint)Math.Abs(this.X);
                    }
                    else
                    {
                        _Distance = (uint)Math.Abs(this.Y);
                    }

                }

                return (uint)_Distance;
            }
        }

        public ChessMove(Vector2 vector)
        {
            this.X = vector.X;
            this.Y = vector.Y;
        }

        public ChessMove(Vector2 vector, MoveBehavior.MoveCapabilities moveCapabilities)
        {
            this.X = vector.X;
            this.Y = vector.Y;
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
                return relativePoint == new Vector2(this.X, this.Y);
            }

            bool inX = InRange(this.X, relativePoint.X);
            bool inY = InRange(this.Y, relativePoint.Y);
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

        public bool SameDirection(ChessMove newMove)
        {
            return (newMove.DirectionalVector.X == DirectionalVector.X) && (newMove.DirectionalVector.Y == DirectionalVector.Y);
        }
    }
}
