using System;
using CocosSharp;
using Chess_Demo.Utilities;
using System.IO;

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
            chessMap = new CCTileMap(Assets.GetStreamForAsset(Path.Combine("tmx","Chess.tmx")));
            this.AddChild(chessMap);
        }
    }
}
