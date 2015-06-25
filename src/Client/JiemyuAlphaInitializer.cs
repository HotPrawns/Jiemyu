using ChessDll.Entities;
using ChessDll.Entities.Jiemyu;
using ChessDll.Map;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo
{
    class JiemyuAlphaInitializer
    {
        Dictionary<String, Texture2D> entityTextures = new Dictionary<String, Texture2D>();

        /// <summary>
        /// Initial entity layout for the roughest version of Jiemyu
        /// </summary>
        /// <param name="name"></param>
        /// <param name="texture"></param>
        public void AddEntityTexture(String name, Texture2D texture)
        {
            entityTextures.Add(name, texture);
        }

        public List<Entity> InitializeJiemyuAlpha1Layout()
        {
            List<Entity> entityList = new List<Entity>();

            //Soldier A, what a badass
            entityList.Add(GenerateEntity<SoldierA>(gridPoint(6,4), entityTextures["Tree"]));

            //Pansy C, the fastest pansy alive
            entityList.Add(GenerateEntity<PansyC>(gridPoint(7, 9), entityTextures["Princess"]));



            return entityList;
        }

        /// <summary>
        /// Initial teams for the roughest version of Jiemyu
        /// </summary>
        /// <returns></returns>
        public List<Team> InitializeJiemyuAlpha1Teams()
        {
            List<Team> teamList = new List<Team>();

            //Team 1 (Top)
            Team top = new Team();
            top.Name = "top";
            top.Color = Color.Black;
            teamList.Add(top);

            //Team 2 (Bottom)
            Team bot = new Team();
            bot.Name = "bottom";
            bot.Color = Color.White;
            teamList.Add(bot);

            return teamList;

        }

        /// <summary>
        /// Generates the template entity with the given position and texture
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="position"></param>
        /// <param name="texture"></param>
        /// <returns></returns>
        private Entity GenerateEntity<T>(Point position, Texture2D texture) where T : Entity, new()
        {
            Entity entity = new T();
            entity.EntityTexture = texture;
            entity.Position = position;
            return entity;
        }
        
        /// <summary>
        /// Takes in coordinates on the level grid and transforms them into a Point.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        private Point gridPoint(int x, int y){
            int yPadding = 50,
            panelWidth = 96,
            panelHeight = 84;

            //0 Index for maths
            x--;
            y--;

            return new Point((x * panelWidth) + (panelWidth/2) , (y * panelHeight) + (panelHeight/2) + yPadding);
        }

        /// <summary>
        /// Loads all of the entity textures.
        /// </summary>
        /// <param name="Content"></param>
        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            AddEntityTexture("Cat Girl", Content.Load<Texture2D>("Character Cat Girl"));
            AddEntityTexture("Tree", Content.Load<Texture2D>("Tree Tall"));
            AddEntityTexture("Princess", Content.Load<Texture2D>("Character Princess Girl"));
            AddEntityTexture("Bug", Content.Load<Texture2D>("Enemy Bug"));
        }
    }
}
