using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using JiemyuDll.Entities;

namespace JiemyuDll.Map
{
    public class TileMap
    {
        public TileMap(Tile[,] tiles)
        {
            map = tiles;
        }

        protected const int TILEHEIGHT = 171;
        protected const int TILEWIDTH = 101;
        protected const int TILEOFFSET = 87;

        protected const int LEFTMARGIN = 0;
        protected const int TOPMARGIN = 0;
        protected const int RIGHTMARGIN = 0;
        protected const int BOTTOMMARGIN = 0;

        protected const int TILESWIDE = 8;
        protected const int TILESHIGH = 8;

        protected Vector2 cameraPosition;
        protected Vector2 selectedPosition;
        protected Vector2 currentPosition;

        protected Entity selectedEntity;

        protected MoveCalculator moveCalculator = null;

        protected volatile bool selectionUpdated;

        protected List<RenderObject> PlacedObjects = new List<RenderObject>();

        // Public readonly values useful for calculations
        public List<RenderObject> ObjectsInMap { get{ return PlacedObjects;  } }
        public Vector2 CurrentSelectedPosition { get { return selectedPosition;  } }

        protected Tile[,] map;

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
        /// <param name="newPosition"></param>
        protected void MoveEntity(Vector2 newPosition)
        {
            // Update placed objects
            PlacedObjects.Remove(PlacedObjects.Find(r => r.Entity == selectedEntity));
            PlacedObjects.Add(new RenderObject(selectedEntity, newPosition));
        
            // Update local references
            selectedPosition = new Vector2(-1,-1);
            selectedEntity.Position = GetPointForTile(newPosition);

            // Send message to server indicating update
            var message = new Network.Messages.MoveMessage(newPosition);
            message.EntityId = selectedEntity.Id;

            // TODO: Trigger move event
            // Network.NetworkManager.Instance().SendMessage(message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        protected void AttackEntity(Vector2 position)
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
        
        public bool HasTile(Vector2 tile)
        {
            int xPadding = (Width - TILESWIDE) / 2;
            int yPadding = (Height - TILESHIGH) / 2;

            return (tile.X >= xPadding && tile.X < (TILESWIDE + xPadding) && (tile.Y >= yPadding && tile.Y < (TILESHIGH + yPadding)));
        }
    }
}
