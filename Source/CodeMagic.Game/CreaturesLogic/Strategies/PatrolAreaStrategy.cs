using System;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.Strategies
{
    public class PatrolAreaStrategy : ICreatureStrategy
    {
        private Direction? currentDirection;

        public bool Update(INonPlayableCreatureObject creature, Point position)
        {
            if (!currentDirection.HasValue)
            {
                currentDirection = GenerateDirection();
            }

            var targetPoint = Point.GetPointInDirection(position, currentDirection.Value);
            var movementResult = MovementHelper.MoveCreature(creature, position, targetPoint, true, true);
            if (!movementResult.Success)
            {
                currentDirection = null;
            }

            return true;
        }

        private Direction GenerateDirection()
        {
            return RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
        }
    }
}