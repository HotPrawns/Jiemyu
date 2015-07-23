using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Behaviors.Move
{
    public class Move
    {
        private List<Vector2> path;

        /// <summary>
        /// Starting locaiton of the move
        /// </summary>
        public Vector2 Origin
        {
            get
            {
                return path.First();
            }
        }

        /// <summary>
        /// Final destination of the move
        /// </summary>
        public Vector2 Destination
        {
            get
            {
                return path.Last();
            }
        }

        /// <summary>
        /// Total distance, in tiles, of the move.
        /// </summary>
        public uint Distance
        {
            get
            {
                uint distance = 0;

                Vector2 previous = Origin;
                for (int i = 1; i < path.Count; i++)
                {
                    var current = path[i];
                    distance += MoveCalculator.GetMoveCost(current - previous);
                    previous = current;
                }

                return distance;
            }
        }

        public Move(List<Vector2> path)
        {
            this.path = path;
        }

        /// <summary>
        /// Returns true if the position is somewhere along the path
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public bool Contains(Vector2 position)
        {
            return path.Any(p => p == position);
        }

        /// <summary>
        /// Gets an enumorator for easy enumuration through the path. 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Vector2> GetEnumerator()
        {
            for (int i = 0; i < path.Count; i++)
            {
                yield return path[i];
            }
        }
    }
}
