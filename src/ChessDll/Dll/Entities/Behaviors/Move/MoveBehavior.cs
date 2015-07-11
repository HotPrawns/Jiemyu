using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDll.Entities.Behaviors.Move
{
    public class MoveBehavior
    {
        [Flags]
        public enum MoveCapabilities
        {
            Walk = 0x1,
            Fly = 0x2,
            Dig = 0x4,
            Swim = 0x8,
            Hover = 0x10,
            Jump = 0x20
        };

        [Flags]
        public enum MoveTypes
        {
            Diagonal = 0x1,
            Linear = 0x2,
            Forward = 0x4,
            Standard = 0x8
        };

        /// <summary>
        /// Number that represents infinite move distance
        /// </summary>
        public static readonly int InfiniteMoveDistance = -1;

        /// <summary>
        /// Available distance to move. -1 represents max, as set by the map.
        /// </summary>
        public int MoveDistance = InfiniteMoveDistance;

        /// <summary>
        /// MoveCapabilities of the entity. Defaults to Walk.
        /// </summary>
        public MoveCapabilities Capabilities = MoveCapabilities.Walk;

        /// <summary>
        /// Move types, used for calculating available movements
        /// </summary>
        public MoveTypes MoveType;
        
        /// <summary>
        /// Gets all points, relative to 0,0 (representing the current entity location) 
        /// that an entity can move.
        /// </summary>
        /// <returns></returns>
        public virtual Vector2[] GetAvailableMovements(int max)
        {
            List<Vector2> moves = new List<Vector2>();

            max = (MoveDistance == InfiniteMoveDistance) ? max : MoveDistance;

            if (IsFlagSet(MoveCapabilities.Walk))
            {
                // If a unit can walk, it can move from 0-X squares in available directions
                if (IsFlagSet(MoveTypes.Diagonal))
                {
                    for (int i = 1; i <= max; i++)
                    {
                        Vector2 rf = new Vector2(i,i);
                        Vector2 lf = new Vector2(-i, i);
                        Vector2 lb = new Vector2(-i, -i);
                        Vector2 rb = new Vector2(i, -i);

                        moves.Add(rf);
                        moves.Add(lf);
                        moves.Add(lb);
                        moves.Add(rb);
                    }
                }

                if (IsFlagSet(MoveTypes.Linear))
                {
                    for (int i = 1; i <= max; i++)
                    {
                        Vector2 f = new Vector2(0, i);
                        Vector2 b = new Vector2(0, -i);
                        Vector2 l = new Vector2(-i, 0);
                        Vector2 r = new Vector2(i, 0);

                        moves.Add(f);
                        moves.Add(b);
                        moves.Add(l);
                        moves.Add(r);
                    }
                }

                if (IsFlagSet(MoveTypes.Forward))
                {
                    for (int i = 1; i <= max; i++)
                    {
                        moves.Add(new Vector2(0, i));
                    }
                }

                if (IsFlagSet(MoveTypes.Standard))
                {

                    for (int i = 1; i <= max; i++)
                    {
                        int diagi = i - 1;
                        //Currently structured so that they can only walk N E W S per distance unit
                        Vector2 f = new Vector2(0, i);
                        Vector2 b = new Vector2(0, -i);
                        Vector2 l = new Vector2(-i, 0);
                        Vector2 r = new Vector2(i, 0);
                        //Uses diagi (i-1) since units cannot walk directly diagonal
                        Vector2 rf = new Vector2(diagi, diagi);
                        Vector2 lf = new Vector2(-diagi, diagi);
                        Vector2 lb = new Vector2(-diagi, -diagi);
                        Vector2 rb = new Vector2(diagi, -diagi);

                        moves.Add(f);
                        moves.Add(b);
                        moves.Add(l);
                        moves.Add(r);
                        moves.Add(rf);
                        moves.Add(lf);
                        moves.Add(lb);
                        moves.Add(rb);
                    }


                }     
            }
            else
            {
                throw new NotImplementedException("Fuck off, we don't do that");
            }
            
            return moves.ToArray();
        }

        /// <summary>
        /// Checks if the MoveBehavior supports a capability
        /// </summary>
        /// <param name="capability"></param>
        /// <returns></returns>
        private bool IsFlagSet(MoveCapabilities capability)
        {
            return ((capability & Capabilities) == capability);
        }

        /// <summary>
        /// Checks if a movetype is available. 
        /// </summary>
        /// <param name="moveType"></param>
        /// <returns></returns>
        private bool IsFlagSet(MoveTypes moveType)
        {
            return (moveType & this.MoveType) == moveType;
        }
    }
}
