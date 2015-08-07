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

namespace JiemyuDll.Entities.Behaviors.Attack
{
    public class AttackCalculator
    {
        private Entity entity;

        /// <summary>
        /// Gets all the attacks available for an entity on the map.
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static List<Attack> GetAttacks(Entity entity, TileMap map)
        {
            List<Attack> attacks = new List<Attack>();

            if (entity == null || map == null)
            {
                return attacks;
            }

            // Get the origin of the entity on the map
            var origin = map.ObjectsInMap.Find(m => m.Entity == entity).Location;

            foreach (var relativeTargetSpace in entity.AttackBehavior.GetPossibleAttacks())
            {
                // All possible movements are in relative to the origin, so add the origin 
                // to make the destination be within the map
                var targetSpace = relativeTargetSpace + origin;

                Vector2 attackRangeVector = targetSpace - origin;

                if (((uint)Math.Floor(attackRangeVector.Length())) <= entity.AttackRange)
                {
                    //TODO Add all tiles that do not have something blocking, or end of map
                    //Cycle through Attacks on map and display the tiles from the Attack Objects
                    //If one is executed, check attack entity, or run it through Entity.Attack(Attack)
                    if (!map.HasTile(targetSpace))
                    {
                        continue;    
                    }

                    Attack atk = new Attack(targetSpace, entity.AttackDamage);

                    attacks.Add(atk);
                }               
            }

            return attacks;
        }
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
