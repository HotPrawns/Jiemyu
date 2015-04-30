using System;
using CocosSharp;

namespace Chess_Demo
{
    class ChessLayer : CCLayer
    {
        CCTileMap chessMap;

        public ChessLayer()
        {
        }

        protected override void AddedToScene()
        {
            base.AddedToScene();

            chessMap = new CCTileMap("tmx/Chess.tmx");
            this.AddChild(chessMap);
        }
    }
}
