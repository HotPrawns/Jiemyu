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
        public TileMap(Tile[,] tiles)
        {
            map = tiles;
            TeamDictionary = new Dictionary<Entity, Team>();

            // Set up any mouse related event handlers
            mouseProcessor.Clicked += new MouseEventHandler(MouseClicked);
        }

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

        private MoveCalculator moveCalculator = null;

        private volatile bool selectionUpdated;

        private List<Texture2D> tileTextures = new List<Texture2D>();
        private List<Texture2D> decalTextures = new List<Texture2D>();

        private MouseProcessor mouseProcessor = new MouseProcessor();

        List<RenderObject> PlacedObjects = new List<RenderObject>();

        // Public readonly values useful for calculations
        public List<RenderObject> ObjectsInMap { get{ return PlacedObjects;  } }
        public Vector2 CurrentSelectedPosition { get { return selectedPosition;  } }

        //Dictionary containing all entities and the team that they belong to
        public Dictionary<Entity, Team> TeamDictionary { get; private set; }

        public Texture2D HighlightTexture;

        public Texture2D MoveIndicator { get; set; }
        public Texture2D AttackIndicator { get; set; }

        Tile[,] map;

        /// <summary>
        /// 
        /// </summary>
        public int Width
        {
            get { return map.GetLength(1); }
        }

        /// <summary>
        /// 
        /// </summary>
        public int Height
        {
            get { return map.GetLength(0); }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        public void AddTileTexture(Texture2D texture)
        {
            tileTextures.Add(texture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texture"></param>
        public void AddDecalTexture(Texture2D texture)
        {
            decalTextures.Add(texture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cameraDirection"></param>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        private void MouseClicked(MouseProcessor processor)
        {
            bool isAction = false;
            if (moveCalculator != null)
            {
                if (moveCalculator.GetAvailableMoves().Any(p => p.InMove(CurrentSelectedPosition, currentPosition)))
                {
                    MoveEntity(currentPosition);
                    isAction = true;
                }
                else if (moveCalculator.GetAvailableAttackLocations().Any(p => p.InMove(CurrentSelectedPosition, currentPosition)) && GetEntityFor(currentPosition) != null)
                {
                    AttackEntity(currentPosition);
                    isAction = true;
                }
            }
            
            if (!isAction)
            {
                selectedPosition = currentPosition;
            }

            selectionUpdated = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="state"></param>
        public void UpdateCursor(MouseState state)
        {
            Point position = state.Position;

            currentPosition = GetTileForPoint(position);
            mouseProcessor.Update(state);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="newPosition"></param>
        private void MoveEntity(Vector2 newPosition)
        {
            // Update placed objects
            PlacedObjects.Remove(PlacedObjects.Find(r => r.Entity == selectedEntity));
            PlacedObjects.Add(new RenderObject(selectedEntity, newPosition));
        
            // Update local references
            selectedPosition = new Vector2(-1,-1);
            selectedEntity.Position = GetPointForTile(newPosition);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        private void AttackEntity(Vector2 position)
        {
            var targetEntity = GetEntityFor(position);

            if (targetEntity == null)
            {
                return;
            }

            selectedEntity.Attack(targetEntity);

            if (targetEntity.HitPoints <= 0)
            {
                PlacedObjects.Remove(PlacedObjects.Find(r => r.Entity == targetEntity));

                // For chess, attacks move everything into the space. So update the selectedEntity
                MoveEntity(position);
            }
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
            var renderObject = PlacedObjects.FirstOrDefault(p => p.Location == position);

            if (renderObject == null)
            {
                return null;
            }

            return renderObject.Entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        public void AddObjectToCurrentPosition(Entity obj)
        {
            PlacedObjects.Add(new RenderObject(obj, currentPosition));
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
            PlacedObjects.Add(new RenderObject(obj, new Vector2(x,y)));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="team"></param>
        public void AssignTeam(Entity entity, Team team)
        {
            TeamDictionary.Add(entity, team);
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

                // If selection updated in some way, then update the moveCalculator
                moveCalculator = new MoveCalculator(selectedEntity, this);
            }

            var possibleMoves = (moveCalculator == null) ? new MoveList() : moveCalculator.GetAvailableMoves();
            var attackMoves = (moveCalculator == null) ? new MoveList() : moveCalculator.GetAvailableAttackLocations();

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

                    if (possibleMoves.Any(move => move.InMove(CurrentSelectedPosition, new Vector2(x,y))))
                    {
                        batch.Draw(MoveIndicator, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }
                    else if (attackMoves.Any(move => move.InMove(CurrentSelectedPosition, new Vector2(x, y))) && GetEntityFor(new Vector2(x,y)) != null)
                    {
                        batch.Draw(AttackIndicator, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.IndianRed);
                    }
                }
            }

            // Sort objects in order they need to be drawn
            PlacedObjects.Sort();

            // Draw objects
            foreach (var renderObject in PlacedObjects)
            {
                Vector2 position = renderObject.Location;
                Point point = GetPointForTile(position);
                batch.Draw(renderObject.Entity.EntityTexture, new Rectangle(point.X, point.Y, TILEWIDTH, TILEHEIGHT), TeamDictionary[renderObject.Entity].color);
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

        public bool HasTile(Vector2 tile)
        {
            int xPadding = (Width - TILESWIDE) / 2;
            int yPadding = (Height - TILESHIGH) / 2;

            return (tile.X >= xPadding && tile.X < (TILESWIDE + xPadding) && (tile.Y >= yPadding && tile.Y < (TILESHIGH + yPadding)));
        }
    }
}
