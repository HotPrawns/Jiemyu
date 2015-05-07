using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChessDemo.Entities.Chess;
using Microsoft.Xna.Framework;

namespace ChessDemo.Entities
{
    class ChessEntityFactory
    {
        List<Texture2D> entityTextures = new List<Texture2D>();

        public void AddEntityTexture(Texture2D texture)
        {
            entityTextures.Add(texture);
        }

        public List<Entity> InitialChessLayout()
        {
            List<Entity> entityList = new List<Entity>();

            // Pawns Everywhere
            for (int x = 2*101; x < 10*101; x += 101)
            {
                entityList.Add(GenerateCatPawn(x, (int)(3.6*87)));
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

            return entityList;
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
        }
    }
}
