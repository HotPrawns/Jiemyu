using JiemyuDll.Entities;
using JiemyuDll.Entities.Behaviors.Attack;
using JiemyuDll.Entities.Behaviors.Move;
using JiemyuDll.Map;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Behaviors.Move
{
    public class MoveCalculator
    {
        private Entity entity;

        /// <summary>
        /// Helper to take a direction, and the forward vector of a unit, and 
        /// make a vector that represents that direction in map space. 
        /// </summary>
        /// <param name="direction"></param>
        /// <param name="forward"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets the tile moves it takes, without pathing, for a 
        /// given move delta. 
        /// </summary>
        /// <param name="move"></param>
        /// <returns></returns>
        public static uint GetMoveCost(Vector2 move)
        {
            uint x = (uint)Math.Abs(move.X);
            uint y = (uint)Math.Abs(move.Y);

            return x + y;
        }

        /// <summary>
        /// Gets all the moves available for an entity on the map.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static List<Move> GetMoves(Entity entity, TileMap map)
        {
            List<Move> moves = new List<Move>();

            if (entity == null || map == null)
            {
                return moves;
            }

            // Get the origin of the entity on the map
            var origin = map.ObjectsInMap.Find(m => m.Entity == entity).Location;

            foreach (var relativeDestination in entity.MoveBehavior.GetPossibleMovements())
            {
                // All possible movements are in relative to the origin, so add the origin 
                // to make the destination be within the map
                var destination = relativeDestination + origin;

                if (entity.MoveBehavior.Capabilities.HasFlag(MoveBehavior.MoveCapabilities.Dig) ||
                 entity.MoveBehavior.Capabilities.HasFlag(MoveBehavior.MoveCapabilities.Jump) ||
                 entity.MoveBehavior.Capabilities.HasFlag(MoveBehavior.MoveCapabilities.Fly))
                {

                    Vector2 distanceVector = destination - origin;

                    if (((uint)Math.Floor(distanceVector.Length())) <= entity.MoveDistance)
                    {
                        List<Vector2> path = new List<Vector2>();
                        path.Add(origin);
                        path.Add(destination);

                        Move m = new Move(path);

                        moves.Add(m);
                    }
                }
                else
                {
                    Move m = FindMove(entity, map, origin, destination);
                    if (m != null)
                    {
                        moves.Add(m);
                    }
                }

            }

            return moves;
        }

        /// <summary>
        /// Convenient struct to store A* values. 
        /// G is cost to move to a location
        /// H is a heuristic calculated on the likelihood that something is the shortest path
        /// F = G + H
        /// Parent is the point before this value, for constructing the path at the end
        /// </summary>
        private struct AStarValues
        {
            public int G;
            public int H;
            public int F;
            public Vector2 Parent;
        };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="origin">Point to start the path search</param>
        /// <param name="destination">Point to end the path search</param>
        /// <returns></returns>
        private static Move FindMove(Entity entity, TileMap map, Vector2 origin, Vector2 destination)
        {
            List<Vector2> path = new List<Vector2>();

            // There's always a path if the origin is the destination
            if (origin == destination)
            {
                path.Add(origin);
                return new Move(path);
            }

            // Use A* to try and make a path to the destination. Return null
            // if no path is found, or a list of moves (in order) if found.
            // Make sure to cache the destination for future use.
            // If the unit can fly, dig, or jump, just make sure the destination is within reach
            // and currently doesn't contain anything

            // For other cases, try and use A* to find a path
            Dictionary<Vector2, AStarValues> openPoints = new Dictionary<Vector2, AStarValues>();
            List<KeyValuePair<Vector2, AStarValues>> closedPoints = new List<KeyValuePair<Vector2, AStarValues>>();

            AStarValues originValues = new AStarValues();
            originValues.G = 0;
            originValues.Parent = origin;

            openPoints.Add(origin, originValues);
            KeyValuePair<Vector2, AStarValues> current = new KeyValuePair<Vector2, AStarValues>(origin, originValues);

            var relativePositions = GetRelativePositions();

            while (openPoints.Count > 0)
            {
                current = openPoints.OrderBy(p => p.Value.F).First<KeyValuePair<Vector2, AStarValues>>();

                // Drop the nextPoint from openPoints and add it to the closedPoints
                openPoints.Remove(current.Key);
                closedPoints.Add(current);

                if (current.Key == destination)
                {
                    break;
                }

                // Add to our open list the new set of squares to use. 
                foreach (Vector2 position in relativePositions)
                {
                    Vector2 next = current.Key + position;

                    if (closedPoints.Any(p => p.Key == next) || !map.HasTile(next) || map.ObjectsInMap.Any<RenderObject>(p => p.Location == next))
                    {
                        continue;
                    }

                    AStarValues values = new AStarValues();
                    values.Parent = current.Key;

                    // Use the current G value plus the G value to move to the next point to find the actual G value
                    values.G = (int)GetMoveCost(position) + current.Value.G;

                    if (values.G > entity.MoveDistance)
                    {
                        // The distance is too great. don't add to the open points
                        continue;
                    }

                    if (next == destination)
                    {
                        openPoints[next] = values;
                        break;
                    }

                    // H is calculated using the Manhatten method. Essentially, calculate the move cost from the proposed square
                    // to the destination square using only horizontal and vertical movements. 
                    values.H = (int)GetMoveCost(destination - next);

                    values.F = values.G + values.H;

                    if (openPoints.ContainsKey(next))
                    {
                        // Check the G score of the current path to this square. If this path is faster, then update the 
                        // value to include the current values for going this direction
                        if (openPoints[next].G > values.G)
                        {
                            openPoints[next] = values;
                        }
                    }
                    else
                    {
                        openPoints[next] = values;
                    }
                }
            }

            // The origin always gets put into the closedList, so check for anything greater than 1
            var lastPoint = closedPoints.Last();

            if (lastPoint.Key != destination)
            {
                // Didn't find a path to the destination
                return null;
            }

            var currentPoint = lastPoint;
            while (currentPoint.Key != origin)
            {
                path.Add(currentPoint.Key);
                currentPoint = closedPoints.First(v => v.Key == currentPoint.Value.Parent);
            }

            //  Cache this path
            return new Move(path);
        }

        /// <summary>
        /// Helper function to get the 8 surrounding tiles as Vector2
        /// </summary>
        /// <returns></returns>
        private static Vector2[] GetRelativePositions()
        {
            Vector2[] positions = new Vector2[8];

            positions[0] = new Vector2(0, 1);
            positions[1] = new Vector2(1, 1);
            positions[2] = new Vector2(1, 0);
            positions[3] = new Vector2(1, -1);
            positions[4] = new Vector2(0, -1);
            positions[5] = new Vector2(-1, -1);
            positions[6] = new Vector2(-1, 0);
            positions[7] = new Vector2(-1, 1);

            return positions;
        }
    }
}
