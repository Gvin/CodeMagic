using System;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public class FreeWonderStrategy : ICreatureStrategy
    {
        public bool Update(INonPlayableCreatureObject creature, IGameCore game, Point position)
        {
            var direction = RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
            var targetPosition = Point.GetPointInDirection(position, direction);
            MovementHelper.MoveCreature(creature, game, position, targetPosition);
            return true;
        }
    }
}