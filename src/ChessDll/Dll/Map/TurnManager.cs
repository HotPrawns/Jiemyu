using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities;

namespace ChessDll.Map
{
    public sealed class TurnManager
    {
        ///Singleton instance for TurnManager
        private static readonly TurnManager turnManager = new TurnManager();

        //Dictionary containing all entities and the team that they belong to
        public Dictionary<Entity, Team> TeamDictionary { get; private set; }

        /// Team which is currently active
        public Team currentTeam;

        /// <summary>
        /// Private constructor for TurnManager
        /// </summary>
        private TurnManager() 
        {
            TeamDictionary = new Dictionary<Entity, Team>();
        }

        /// <summary>
        /// Return the static instance of TurnManager
        /// </summary>
        public static TurnManager Instance
        {
            get
            {
                return turnManager;
            }
        }
        
        public Boolean IsMyTurn(Entity currentEntity)
        {
            if (currentEntity != null)
                return (TeamDictionary[currentEntity].Name == currentTeam.Name);
            else return false;

        }

        /// <summary>
        /// Add a team to the TeamDictionary
        /// </summary>
        /// <param name="entity"</param>
        /// <param name="team"></param>
        public void Add(Entity entity, Team team)
        {
            TeamDictionary.Add(entity, team);

            if (currentTeam == null)
            {
                currentTeam = team;
            }
        }

        /// <summary>
        /// Advance turn to the next Team in the list
        /// </summary>
        public void AdvanceTurn()
        {
            List<Team> teamList = TeamDictionary.Values.Distinct().ToList();
            
            if (currentTeam != null)
            {
                int index = teamList.IndexOf(currentTeam) + 1;

                if (index < teamList.Count)
                    currentTeam = teamList.ElementAt(index);
                else
                    currentTeam = teamList.First();
            }
        }

    }
}
