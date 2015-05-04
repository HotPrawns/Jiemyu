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

        public List<Entity> InitialChessLayout(){
            List<Entity> entityList = new List<Entity>();

            //Pawns Everywhere
            for (int x = 200; x < 1000; x+=100){
                entityList.Add(generateCatPawn(x, 225));
            }



            
            return entityList;

        }
        
        private Entity generateCatPawn(int x, int y){
            Entity catPawn = new Pawn();
            catPawn.EntityTexture = entityTextures[0];
            catPawn.Position = new Point(x,y);
            return catPawn;
        }

    }
}
