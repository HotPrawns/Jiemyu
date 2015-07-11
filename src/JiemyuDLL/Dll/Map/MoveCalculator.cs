using JiemyuDll.Entities;
using JiemyuDll.Entities.Behaviors.Attack;
using JiemyuDll.Entities.Behaviors.Move;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace JiemyuDll.Map
{
    public class MoveCalculator
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

            if (moveAttack != null)
            {
                List<Move> moves = new List<Move>();

                foreach (var direction in moveAttack.MoveBehavior.GetAvailableMovements(map.Width))
                {
                    var mapDirection = Move.ToMapDirection(direction, entity.Forward);
                    moves.Add(new Move(mapDirection));
                }

                foreach (var move in moves)
                {
                    // Target = relative point from unit that we are trying to go to
                    var target = move.Vector;

                    // Direction = unit vector representing the direction of the move
                    var direction = move.DirectionalVector;

                    // CurrentPoint = current point being tested
                    var currentPoint = new Vector2(0, 0);

                    // NextPoint = next point to be test
                    var nextPoint = currentPoint + direction;

                    // EntityTile = The location of the entity, on the map
                    var entityTile = map.ObjectsInMap.First(r => r.Entity == entity).Location;

                    var isJump = entity.MoveBehavior.Capabilities.HasFlag(MoveBehavior.MoveCapabilities.Jump);

                    if (isJump)
                    {
                        if (map.HasTile(entityTile + target))
                        {
                            var otherEntity = map.GetEntityFor(entityTile + target);

                            if (otherEntity == null || TurnManager.Instance.TeamDictionary[otherEntity] != TurnManager.Instance.TeamDictionary[entity])
                            {
                                _Attacks.Add(new Move(target, entity.MoveBehavior.Capabilities));
                            }
                        }
                    }
                    else
                    {
                        while (currentPoint != target &&
                            map.HasTile(entityTile + nextPoint))
                        {
                            currentPoint += direction;
                            nextPoint += direction;

                            var otherEntity = map.GetEntityFor(entityTile + currentPoint);
                            if (otherEntity != null)
                            {
                                // If on the same team, don't include this point
                                if (TurnManager.Instance.TeamDictionary[otherEntity] == TurnManager.Instance.TeamDictionary[entity])
                                {
                                    nextPoint = direction;
                                    currentPoint -= direction;
                                }

                                break;
                            }
                        }

                        _Attacks.Add(new Move(currentPoint));
                    }
                }
            }
        }

        private void CalculateMoves()
        {
            _Moves = new MoveList();

            if (entity == null)
            {
                return;
            }

            var isJump = entity.MoveBehavior.Capabilities.HasFlag(MoveBehavior.MoveCapabilities.Jump);

            List<Move> moves = new List<Move>();

            // Get the vectors of moves
            foreach(var direction in entity.GetAvailableMovements(map.Width))
            {
                var mapDirection = Move.ToMapDirection(direction, entity.Forward);
                moves.Add(new Move(mapDirection));
            }

            // _Moves now has a set of Moves that contains the max in each direction.
            // Restrict these based on if something is in the way
            foreach (var move in moves)
            {
                // Target = relative point from unit that we are trying to go to
                var target = move.Vector;

                // Direction = unit vector representing the direction of the move
                var direction = move.DirectionalVector;

                // CurrentPoint = current point being tested
                var currentPoint = new Vector2(0, 0);

                // NextPoint = next point to be test
                var nextPoint = currentPoint + direction;

                // EntityTile = The location of the entity, on the map
                var entityTile = map.ObjectsInMap.First(r => r.Entity == entity).Location;

                if (isJump)
                {
                    currentPoint = entityTile + target;
                    if (map.HasTile(currentPoint) && map.GetEntityFor(currentPoint) == null)
                    {
                        _Moves.Add(new Move(target, entity.MoveBehavior.Capabilities));
                    }
                }
                else
                {
                    while (currentPoint != target &&
                        map.HasTile(entityTile + nextPoint) &&
                        map.GetEntityFor(entityTile + nextPoint) == null)
                    {
                        currentPoint += direction;
                        nextPoint += direction;
                    }

                    _Moves.Add(new Move(currentPoint, entity.MoveBehavior.Capabilities));
                }
            }
        }
    }
}
