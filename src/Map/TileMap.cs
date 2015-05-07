using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ChessDemo.Entities;
using ChessDemo.Input;

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
        const int TILESHIGH = 8;

        private Vector2 cameraPosition;
        private Vector2 currentPosition;
        private Vector2 selectedPosition;

        private Entity selectedEntity;

        private List<Vector2> possibleMoves;

        private volatile bool selectionUpdated;

        private List<Texture2D> tileTextures = new List<Texture2D>();
        private List<Texture2D> decalTextures = new List<Texture2D>();

        private MouseProcessor mouseProcessor = new MouseProcessor();

        // TODO: Replace with actual object representation, rather than just generic object
        Dictionary<Entity, Vector2> PlacedObjects = new Dictionary<Entity, Vector2>();

        public Texture2D HighlightTexture;

        public TileMap(Tile[,] tiles)
        {
            map = tiles;

            mouseProcessor.Clicked += new MouseEventHandler(MouseClicked);
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

        public Texture2D MoveIndicator { get; internal set; }

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
            cameraPosition += cameraDirection;

            if (cameraPosition.X < LEFTMARGIN)
                cameraPosition.X = LEFTMARGIN;
            if (cameraPosition.Y < TOPMARGIN)
                cameraPosition.Y = TOPMARGIN;
            if (cameraPosition.X > (Width - TILESWIDE) * TILEWIDTH + RIGHTMARGIN)
                cameraPosition.X = (Width - TILESWIDE) * TILEWIDTH + RIGHTMARGIN;
            if (cameraPosition.Y > (Height - TILESHIGH) * TILEHEIGHT - BOTTOMMARGIN)
                cameraPosition.Y = (Height - TILESHIGH) * TILEHEIGHT - BOTTOMMARGIN;
        }

        private void MouseClicked(MouseProcessor processor)
        {
            if (possibleMoves.Any(p => p.X == currentPosition.X && p.Y == currentPosition.Y))
            {
                MoveEntity(currentPosition);
            }
            else
            {
                selectedPosition = currentPosition;
            }

            selectionUpdated = true;
        }

        public void UpdateCursor(MouseState state)
        {
            Point position = state.Position;

            currentPosition = GetTileForPoint(position);
            mouseProcessor.Update(state);
        }

        private void MoveEntity(Vector2 newPosition)
        {
            selectedEntity.Position = GetPointForTile(newPosition);
            PlacedObjects[selectedEntity] = newPosition;
            selectedPosition = new Vector2(-1,-1);
        }

        public Vector2 GetTileForPoint(Point point)
        {
            Vector2 tile = new Vector2();

            // Calculate the current tile based on position
            tile.X = (int) Math.Floor((point.X + cameraPosition.X) / TILEWIDTH);

            // 0.5 * TILEOFFSET to offset the blue background showing at the top. 
            // TODO: Figure out why the tiles aren't drawn flush against the top
            tile.Y = (int) Math.Floor((point.Y + cameraPosition.Y - 0.5*TILEOFFSET) / TILEOFFSET);

            return tile;
        }

        /// <summary>
        /// Returns the coordinates of the top-left point
        /// of a given tile
        /// </summary>
        /// <param name="tile"></param>
        /// <returns></returns>
        public Point GetPointForTile(Vector2 tile)
        {
            Point point = new Point();

            point.X = (int)((tile.X * TILEWIDTH) - cameraPosition.X);
            point.Y = (int)((tile.Y * TILEOFFSET) - (cameraPosition.Y + 0.5*TILEOFFSET));

            return point;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public Entity GetEntityFor(Vector2 position)
        {
            return PlacedObjects.Keys.FirstOrDefault(entity => position.Equals(PlacedObjects[entity]));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddObjectToCurrentPosition(Entity obj)
        {
            PlacedObjects.Add(obj, currentPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="tile"></param>
        public void AddObject(Entity obj, Vector2 tile)
        {
            AddObject(obj, (int) tile.X, (int) tile.Y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        public void AddObject(Entity obj, int x, int y)
        {
            PlacedObjects.Add(obj, new Vector2(x, y));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="batch"></param>
        public void Draw(SpriteBatch batch)
        {
            if (selectionUpdated)
            {
                // Reset the update flag
                selectionUpdated = false;

                // Update the selected entity
                selectedEntity = GetEntityFor(selectedPosition);
            }

            // Every frame, possible moves gets updated.
            // TODO: Add a flag to indicate when this needs to be updated.
            // It only needs to update if anything moves on the board, or
            // the selection changes.
            UpdatePossibleMoves();

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    Point point = GetPointForTile(new Vector2(x, y));
                    var left = point.X;
                    var top = point.Y;
                    var top2 = y * (TILEHEIGHT - TILEOFFSET) - (int)cameraPosition.Y;

                    // DRAW TILES
                    var tile = map[y, x];
                    if (tile.HasTexture)
                    {
                        var texture = tileTextures[tile.TextureIndex];
                        batch.Draw(texture, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }


                    // DRAW DECALS
                    if (tile.HasDecal)
                    {
                        var decal = decalTextures[tile.DecalIndex];
                        batch.Draw(decal, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }

                    if (possibleMoves.Any(move => move.X == x && move.Y == y))
                    {
                        batch.Draw(MoveIndicator, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.Azure);
                    }
                }
            }

            // Draw objects
            foreach (Entity entity in PlacedObjects.Keys)
            {
                Vector2 position = PlacedObjects[entity];
                Point point = GetPointForTile(position);
                batch.Draw(entity.EntityTexture, new Rectangle(point.X, point.Y, TILEWIDTH, TILEHEIGHT), Color.White);
            }

            for (var x = 0; x < Width; x++)
            {
                for (var y = 0; y < Height; y++)
                {
                    var left = x * TILEWIDTH - (int)cameraPosition.X;
                    var top = y * TILEHEIGHT - (int)cameraPosition.Y;
                    var top2 = y * (TILEHEIGHT - TILEOFFSET) - (int)cameraPosition.Y;

                    // ADD HIGHLIGHT
                    if (x == currentPosition.X && y == currentPosition.Y)
                    {
                        batch.Draw(HighlightTexture, new Rectangle(left, top2 - (int)(0.5 * TILEOFFSET), TILEWIDTH, TILEHEIGHT), Color.White);
                    }
                }
            }
        }

        // TODO: This could use some love. It works for now, but seems hacky at best
        class MaxMoveDistance
        {
            public int Front = -1;
            public int Back = -1;
            public int FrontLeft = -1;
            public int FrontRight = -1;
            public int Right = -1;
            public int Left = -1;
            public int BackLeft = -1;
            public int BackRight = -1;

            public bool CanMove(Vector2 direction)
            {
                int max = 0;

                // Front
                if (direction.Y > 0)
                {
                    if (direction.X > 0)
                    {
                        max = this.FrontRight;
                    }
                    else if (direction.X < 0)
                    {
                        max = this.FrontLeft;
                    }
                    else
                    {
                        max = this.Front;
                    }
                }
                // Back
                else if (direction.Y < 0)
                {
                    if (direction.X > 0)
                    {
                        max = this.BackRight;
                    }
                    else if (direction.X < 0)
                    {
                        max = this.BackLeft;
                    }
                    else
                    {
                        max = this.Back;
                    }
                }
                // Left or Right
                else
                {
                    if (direction.X > 0)
                    {
                        max = this.Right;
                    }
                    else if (direction.X < 0)
                    {
                        max = this.Left;
                    }
                }

                var length = Math.Abs(direction.X) + Math.Abs(direction.Y);

                if (direction.X != 0 && direction.Y != 0)
                {
                    // Movement in x and y is complex, and I'm lazy.
                    // Just divide by 2 in hoping they are the same
                    length /= 2;
                }

                return length <= max;
            }
        }

        private void UpdatePossibleMoves()
        {
            possibleMoves = new List<Vector2>();

            if (selectedEntity == null)
            {
                return;
            }

            MaxMoveDistance maxMoveDistance = new MaxMoveDistance();

            foreach (var direction in selectedEntity.GetAvailableMovements(TILESWIDE))
            {
                CalculateMaxMovement(direction, ref maxMoveDistance);

                if (maxMoveDistance.CanMove(direction))
                {
                    // PossibleMoves stores in "Map Space" (i.e not relative)
                    possibleMoves.Add(selectedPosition + direction);
                }
            }
        }

        private void CalculateMaxMovement(Vector2 direction, ref MaxMoveDistance maxMoveDistance)
        {

            // Front
            if (direction.Y > 0)
            {
                if (direction.X > 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.FrontRight);
                }
                else if (direction.X < 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.FrontLeft);
                }
                else
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.Front);
                }
            }
            // Back
            else if (direction.Y < 0)
            {
                if (direction.X > 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.BackRight);
                }
                else if (direction.X < 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.BackLeft);
                }
                else 
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.Back);
                }
            }
            // Left or Right
            else
            { 
                if (direction.X > 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.Right);
                }
                else if (direction.X < 0)
                {
                    CalculateMaxMovement(direction, ref maxMoveDistance.Left);
                }
            }
        }

        private void CalculateMaxMovement(Vector2 totalDirection, ref int maxMoveDistance)
        {
            // Exit early if the distance has been set
            if (maxMoveDistance != -1 || totalDirection == Vector2.Zero)
            {
                return;
            }

            Vector2 direction = new Vector2();

            if (totalDirection.Y != 0)
            {
                direction.Y = (totalDirection.Y > 0) ? 1 : -1;
            }

            if (totalDirection.X != 0)
            {
                direction.X = (totalDirection.X > 0) ? 1 : -1;
            }

            var startPoint = selectedPosition;
            var currentPoint = startPoint + direction;

            maxMoveDistance = 0;
            while (GetEntityFor(currentPoint) == null && HasTile(currentPoint))
            {
                maxMoveDistance++;
                currentPoint += direction;
            }
        }

        private bool HasTile(Vector2 tile)
        {
            int xPadding = (Width - TILESWIDE) / 2;
            int yPadding = (Height - TILESHIGH) / 2;

            return (tile.X >= xPadding && tile.X < (TILESWIDE + xPadding) && (tile.Y >= yPadding && tile.Y < (TILESHIGH + yPadding)));
        }
    }
}
