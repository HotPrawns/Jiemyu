using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDll.Entities;
using ChessDll.Entities.Chess;
using Microsoft.Xna.Framework;
using ChessDll.Map;

namespace ChessDemo
{
    class ChessInitializer
    {
        List<Texture2D> entityTextures = new List<Texture2D>();

        public void AddEntityTexture(Texture2D texture)
        {
            entityTextures.Add(texture);
        }

        public List<Entity> InitializeChessLayout()
        {
            List<Entity> entityList = new List<Entity>();

            // Pawns Everywhere
            for (int x = 2*101; x < 10*101; x += 101)
            {
                //Top
                entityList.Add(GenerateCatPawn(x, (int)(3.6 * 87)));

                //Bottom
                var pawn = GenerateCatPawn(x, (int)(8.6 * 87));
                pawn.Forward = new Vector2(0, -1);
                entityList.Add(pawn);

            }

            // Rook me Amadeus 
            entityList.Add(GenerateRook(202, (int)3 * 87));
            entityList.Add(GenerateRook(909, (int)3 * 87));

            // Bishops please
            entityList.Add(GenerateBishop(4 * 101, (int)3 * 87));
            entityList.Add(GenerateBishop(7 * 101, (int)3 * 87));

            // Queeny
            entityList.Add(GenerateQueen(6 * 101, (int)3 * 87));

            // King
            entityList.Add(GenerateEntity<King>(5 * 101, (int)3 * 87, entityTextures[4]));

            // Oh holy knight
            entityList.Add(GenerateEntity<Knight>(3 * 101, (int)3 * 87, entityTextures[5]));
            entityList.Add(GenerateEntity<Knight>(8 * 101, (int)3 * 87, entityTextures[5]));

            return entityList;
        }

        public List<Team> InitializeChessTeams()
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

        private Entity GenerateCatPawn(int x, int y)
        {
            Entity catPawn = new Pawn();
            catPawn.EntityTexture = entityTextures[0];
            catPawn.Position = new Point(x, y);
            return catPawn;
        }

        private Entity GenerateRook(int x, int y)
        {
            Entity rook = new Rook();
            rook.EntityTexture = entityTextures[1];
            rook.Position = new Point(x, y);
            return rook;
        }

        private Entity GenerateBishop(int x, int y)
        {
            return GenerateEntity<Bishop>(x, y, entityTextures[2]);
        }

        private Entity GenerateQueen(int x, int y)
        {
            return GenerateEntity<Queen>(x, y, entityTextures[3]);
        }

        private Entity GenerateEntity<T>(int x, int y, Texture2D texture) where T : Entity, new()
        {
            Entity entity = new T();
            entity.EntityTexture = texture;
            entity.Position = new Point(x, y);
            return entity;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            AddEntityTexture(Content.Load<Texture2D>("Character Cat Girl"));
            AddEntityTexture(Content.Load<Texture2D>("Character Horn Girl"));
            AddEntityTexture(Content.Load<Texture2D>("Tree Tall"));
            AddEntityTexture(Content.Load<Texture2D>("Character Princess Girl"));
            AddEntityTexture(Content.Load<Texture2D>("Gem Blue"));
            AddEntityTexture(Content.Load<Texture2D>("Enemy Bug"));
        }
    }
}
