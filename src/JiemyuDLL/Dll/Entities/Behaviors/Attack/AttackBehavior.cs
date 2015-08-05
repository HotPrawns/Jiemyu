using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Entities.Behaviors.Attack
{
    public class AttackBehavior
    {
        /*
         * TODO: We will need to discuss things such as...
         * 1. AttackType can it shoot over/through rocks/units etc
         * 2. AttackArea AoE and all the magic that comes along
         * 3. In respect to the first two, potential of separate damage zones for an attack
         * 4. Moving attacks 
         * 5. Attacks that move other things
         * 6. Action Point Values for attacks?
         * 7. Intentionally whiffing attacks, if they set up a field across turns etc
        */
        public AttackBehavior()
        {
            Damage = 1;
            Range = 0;
        }

        public uint Damage { get; set; }
        public uint Range { get; set; }

        /// <summary>
        /// Gets all points, relative to 0,0 (representing the current entity location) 
        /// that an entity can move.
        /// </summary>
        /// <returns></returns>
        public virtual Vector2[] GetPossibleAttacks()
        {
            HashSet<Vector2> attacks = new HashSet<Vector2>();

            var max =  (int)Range;

            for (int i = 1; i <= max; i++)
            {
                for (int j = 0; (i + j) <= max; j++)
                {
                    attacks.Add(new Vector2(i, j));
                    attacks.Add(new Vector2(i, -j));
                    attacks.Add(new Vector2(-i, j));
                    attacks.Add(new Vector2(-i, -j));

                    attacks.Add(new Vector2(j, i));
                    attacks.Add(new Vector2(j, -i));
                    attacks.Add(new Vector2(-j, i));
                    attacks.Add(new Vector2(-j, -i));
                }
            }


            return attacks.ToArray();
        }
    }
}
