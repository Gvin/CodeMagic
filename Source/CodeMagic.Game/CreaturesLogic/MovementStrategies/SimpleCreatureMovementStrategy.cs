using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.MovementStrategies
{
    public class SimpleCreatureMovementStrategy : ICreatureMovementStrategy
    {
        public bool TryMove(ICreatureObject creature, Point position, Point targetPosition)
        {
            var possibleMoves = GetPossibleMoves(position, targetPosition);
            foreach (var possibleMove in possibleMoves)
            {
                var movementResult = MovementHelper.MoveCreature(creature, position, possibleMove, true, true);
                if (movementResult.Success)
                    return true;
            }

            return false;
        }

        private Point[] GetPossibleMoves(Point position, Point targetPosition)
        {
            var result = new List<Point>();
            var difX = Math.Abs(position.X - targetPosition.X);
            var difY = Math.Abs(position.Y - targetPosition.Y);

            var shiftXPositive = new Point(position.X + 1, position.Y);
            var shiftXNegative = new Point(position.X - 1, position.Y);
            var shiftYPositive = new Point(position.X, position.Y + 1);
            var shiftYNegative = new Point(position.X, position.Y - 1);

            if (difX > difY)
            {
                result.Add(targetPosition.X > position.X
                    ? shiftXPositive
                    : shiftXNegative);

                result.Add(targetPosition.Y > position.Y
                    ? shiftYPositive
                    : shiftYNegative);
            }
            else
            {
                result.Add(targetPosition.Y > position.Y
                    ? shiftYPositive
                    : shiftYNegative);

                result.Add(targetPosition.X > position.X
                    ? shiftXPositive
                    : shiftXNegative);
            }

            return result.ToArray();
        }
    }
}