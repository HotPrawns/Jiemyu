using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ChessDemo.Map
{
    class TileMap
    {
        const int TILEHEIGHT = 171;
        const int TILEWIDTH = 101;
        const int TILEOFFSET = 87;

        const int LEFTMARGIN = 0;
        const int TOPMARGIN = 0;
        const int RIGHTMARGIN = 0;
        const int BOTTOMMARGIN = 0;

        const int TILESWIDE = 8;
        const int TILESHIGH = 8; //13

        Vector2 CameraPosition;
        Vector2 CurrentPosition;

        List<Texture2D> tileTextures = new List<Texture2D>();
        List<Texture2D> decalTextures = new List<Texture2D>();

        // TODO: Replace with actual object representation, rather than just generic object
        Dictionary<Object, Vector2> PlacedObjects = new Dictionary<Object, Vector2>();

        public Texture2D HighlightTexture;

        public TileMap(Tile[,] tiles)
        {
            map = tiles;
        }

        Tile[,] map;

        public int Width
        {
            get { return map.GetLength(1); }
        }

        public int Height
        {
            get { return map.GetLength(0); }
        }

        public void AddTileTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        public void AddDecalTexture(Texture2D texture)
        {
            decalTextures.Add(texture);
        }

        public void MoveCamera(Vector2 cameraDirection)
        {
            CameraPosition += cameraDirection;

            if (CameraPosition.X < LEFTMARGIN)
                CameraPosition.X = LEFTMARGIN;
            if (CameraPosition.Y < TOPMARGIN)
                CameraPosition.Y = TOPMARGIN;
            if (CameraPosition.X > (Width - TILESWIDE) * TILEWIDTH + RIGHTMARGIN)
                CameraPosition.X = (Width - TILESWIDE) * TILEWIDTH + RIGHTMARGIN;
            if (CameraPosition.Y > (Height - TILESHIGH) * TILEHEIGHT - BOTTOMMARGIN)
                CameraPosition.Y = (Height - TILESHIGH) * TILEHEIGHT - BOTTOMMARGIN;
        }

        public void UpdateCursor(MouseState state)
        {
            Point position = state.Position;

            // Calculate the current tile based on position
            CurrentPosition.X = (position.X - LEFTMARGIN) / TILEWIDTH;
            CurrentPosition.Y = (position.Y - TOPMARGIN) / TILEHEIGHT;
        }

        public void AddObjectToCurrentPosition(Object obj)
        {
            PlacedObjects.Add(obj, CurrentPosition);
        }

        public void AddObject(Object obj, int x, int y)
        {
            PlacedObjects.Add(obj, new Vector2(x, y));
        }

        public void Draw(SpriteBatch batch)
        {
            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var left = x * TILEWIDTH - (int)CameraPosition.X;
                    var top = y * TILEHEIGHT - (int)CameraPosition.Y;
                    var top2 = y * (TILEHEIGHT - TILEOFFSET) - (int)CameraPosition.Y;

                    // DRAW TILES
                    var tile = map[y, x];
                    if (tile.HasTexture)
                    {
                        var texture = tileTextures[tile.TextureIndex];
                        if (y == 0)
                            batch.Draw(texture, new Rectangle(left, top, TILEWIDTH, TILEHEIGHT), Color.White);
                        else
                            batch.Draw(texture, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }


                    // DRAW DECALS
                    if (tile.HasDecal)
                    {
                        var decal = decalTextures[tile.DecalIndex];
                        batch.Draw(decal, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }

                    // ADD HIGHLIGHT
                    if (x == CurrentPosition.X && y == CurrentPosition.Y)
                    {
                        batch.Draw(HighlightTexture, new Rectangle(left, top2-(int)(0.25*TILEHEIGHT), TILEWIDTH, TILEHEIGHT), Color.White);
                    }
                }
            }

            // Draw objects
            foreach (Object obj in PlacedObjects.Keys)
            {
                // TODO: obj.Draw()
            }
        }

    }
}
