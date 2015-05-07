﻿using Microsoft.Xna.Framework.Graphics;
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

            //Pawns Everywhere
            for (int x = 2*101; x < 10*101; x += 101)
            {
                entityList.Add(GenerateCatPawn(x, (int)(3.6*87)));
            }

            return entityList;
        }



        private Entity GenerateCatPawn(int x, int y)
        {
            Entity catPawn = new Pawn();
            catPawn.EntityTexture = entityTextures[0];
            catPawn.Position = new Point(x, y);
            return catPawn;
        }

        public void LoadContent(Microsoft.Xna.Framework.Content.ContentManager Content)
        {
            AddEntityTexture(Content.Load<Texture2D>("Character Cat Girl"));
        }
    }
}
