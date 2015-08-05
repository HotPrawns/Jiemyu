using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using JiemyuDll.Map;
using Jiemyu.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using JiemyuDll.Entities.Behaviors.Move;
using Jiemyu.Util;
using JiemyuDll.Entities.Behaviors.Attack;

namespace Jiemyu.Map
{
    class MapObject : TileMap
    {
        public Texture2D HighlightTexture;

        public Texture2D MoveIndicator { get; set; }
        public Texture2D AttackIndicator { get; set; }

        List<Move> possibleMoves;
        List<Attack> possibleAttacks;

        EventList eventHandlers = new EventList();

        public MapObject(Tile[,] tiles) : base(tiles)
        {
            // Set up any mouse related event handlers
            eventHandlers.Add(MouseButton.PressedEvent.Subscribe(TheMouse.Instance().LeftButton, new MouseButtonChanged(MouseLeftClicked)));
            eventHandlers.Add(MouseButton.PressedEvent.Subscribe(TheMouse.Instance().RightButton, new MouseButtonChanged(MouseRightClicked)));

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
        /// <param name="processor"></param>
        private void MouseLeftClicked(MouseButton button)
        {
            bool isAction = false;
            if (selectionMode == SelectionModes.Move)
            {
                if (possibleMoves != null && possibleMoves.Any(p => p.Contains(currentPosition)) && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
                {
                    MoveEntity(currentPosition);
                    isAction = true;
                    selectionMode = SelectionModes.None;
                }
            }

            if (selectionMode == SelectionModes.Attack)
            {
                if (possibleAttacks != null && possibleAttacks.Any(p => p.TargetSpace == currentPosition) && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
                {
                    AttackEntity(selectedEntity, possibleAttacks.Find(f => f.TargetSpace == currentPosition));
                    isAction = true;
                    selectionMode = SelectionModes.None;
                }
            }
            //else if (moveCalculator.GetAvailableAttackLocations().Any(p => p.InMove(CurrentSelectedPosition, currentPosition)) && GetEntityFor(currentPosition) != null
            //    && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
            //{
            //    AttackEntity(currentPosition);
            //    isAction = true;
            //}

            if (!isAction)
            {
                selectedPosition = currentPosition;
                selectionMode = SelectionModes.Move;
            }
            else
            {
                TurnManager.Instance.AdvanceTurn();
            }

            selectionUpdated = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="processor"></param>
        private void MouseRightClicked(MouseButton button)
        {
            bool isAction = false;
            if (selectionMode == SelectionModes.Move)
            {
                if (possibleMoves != null && possibleMoves.Any(p => p.Contains(currentPosition)) && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
                {
                    MoveEntity(currentPosition);
                    isAction = true;
                    selectionMode = SelectionModes.None;
                }
            }

            if (selectionMode == SelectionModes.Attack)
            {
                if (possibleAttacks != null && possibleAttacks.Any(p => p.TargetSpace == currentPosition) && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
                {
                    AttackEntity(selectedEntity, possibleAttacks.Find(f => f.TargetSpace == currentPosition));
                    isAction = true;
                    selectionMode = SelectionModes.None;
                }
            }
            //else if (moveCalculator.GetAvailableAttackLocations().Any(p => p.InMove(CurrentSelectedPosition, currentPosition)) && GetEntityFor(currentPosition) != null
            //    && TurnManager.Instance.IsMyTurn(GetEntityFor(CurrentSelectedPosition)))
            //{
            //    AttackEntity(currentPosition);
            //    isAction = true;
            //}

            if (!isAction)
            {
                selectedPosition = currentPosition;
                selectionMode = SelectionModes.Attack;
            }
            else
            {
                TurnManager.Instance.AdvanceTurn();
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

            switch (selectionMode)
            {
                case SelectionModes.Move:
                    possibleMoves = MoveCalculator.GetMoves(selectedEntity, this);
                    possibleAttacks = new List<Attack>();
                    break;
                case SelectionModes.Attack:
                    possibleAttacks = AttackCalculator.GetAttacks(selectedEntity, this);
                    possibleMoves = new List<Move>();
                    break;
                default:
                    possibleMoves = new List<Move>();
                    possibleAttacks = new List<Attack>();
                    break;

            }

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

                    if (possibleMoves.Any<Move>(move => move.Contains(new Vector2(x, y))))
                    {
                        batch.Draw(MoveIndicator, new Rectangle(left, top2, TILEWIDTH, TILEHEIGHT), Color.White);
                    }
                    else if (possibleAttacks.Any<Attack>(attack => attack.TargetSpace == new Vector2(x, y)))
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
                batch.Draw(renderObject.Entity.EntityTexture, new Rectangle(point.X, point.Y, TILEWIDTH, TILEHEIGHT), TurnManager.Instance.TeamDictionary[renderObject.Entity].Color);
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

        protected List<Texture2D> tileTextures = new List<Texture2D>();
        protected List<Texture2D> decalTextures = new List<Texture2D>();
    }
}
