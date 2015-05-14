using ChessDemo.Entities;
using ChessDemo.Entities.Behaviors;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ChessDemo.Map
{
    class MoveCalculator
    {
        private Entity entity;
        private TileMap map;

        private MoveList _Attacks = null;
        private MoveList _Moves = null;


        public MoveCalculator(Entity entity, TileMap map)
        {
            this.entity = entity;
            this.map = map;
        }


        public MoveList GetAvailableAttackLocations()
        {
            if (_Attacks == null)
            {
                CalculateAttacks();
            }

            return _Attacks;
        }

        public MoveList GetAvailableMoves()
        {
            if (_Moves == null)
            {
                CalculateMoves();
            }

            return _Moves;
        }

        private void CalculateAttacks()
        {
            _Attacks = new MoveList();

            if (entity == null)
            {
                return;
            }

            var moveAttack = entity.AttackBehavior as MoveAttack;
        }

        private void CalculateMoves()
        {
            _Moves = new MoveList();

            if (entity == null)
            {
                return;
            }

            MoveList moves = new MoveList();

            // Get the vectors of moves
            foreach(var direction in entity.GetAvailableMovements(map.Width))
            {
                var mapDirection = Move.ToMapDirection(direction, entity.Forward);
                moves.Add(mapDirection);
            }

            // _Moves now has a set of Moves that contains the max in each direction.
            // Restrict these based on if something is in the way
            foreach (var move in moves)
            {
                var target = move.Vector;
                var direction = move.DirectionalVector;
                var currentPoint = new Vector2(0, 0);
                var nextPoint = currentPoint + direction;
                var entityTile = map.ObjectsInMap.First(r => r.Entity == entity).Location;

                while (currentPoint != target && 
                    map.HasTile(entityTile + nextPoint) && 
                    map.GetEntityFor(entityTile + nextPoint) == null)
                {
                    currentPoint += direction;
                    nextPoint += direction;
                }

                _Moves.Add(new Move(currentPoint));
            }
        }
    }
}
