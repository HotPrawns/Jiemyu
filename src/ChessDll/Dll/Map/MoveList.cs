using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDll.Map
{
    /// <summary>
    /// List of moves. Note that all moves are in map space
    /// </summary>
    public class MoveList : List<Move>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPoint"></param>
        /// <returns></returns>
        public bool Add(Vector2 newPoint)
        {
            Move newMove = new Move(newPoint);
            bool contains = false;

            for (int i = 0; i < this.Count; i++)
            {
                Move move = this[i];

                if (move.InMove(newMove.Vector))
                {
                    return false;
                }

                bool sameDirection = move.SameDirection(newMove);

                if (sameDirection && (newMove.Distance > move.Distance))
                {
                    this.RemoveAt(i);
                    this.Add(newMove);
                    return true;
                }

                contains |= sameDirection;
            }

            // If the list doesn't contain a move in this direction
            // then add it
            if (!contains)
            {
                this.Add(newMove);
                return true;
            }

            return false;
        }

        public void ToArray(out Vector2[] outArray)
        {
            List<Vector2> moves = new List<Vector2>();

            foreach (Move m in this)
            {
                moves.Add(m.Vector);
            }

            outArray = moves.ToArray();
        }
    }
}
