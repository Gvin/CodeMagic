using System;
using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Game.CreaturesLogic.Strategies
{
    public class FreeWonderStrategy : ICreatureStrategy
    {
        public bool Update(INonPlayableCreatureObject creature, Point position)
        {
            var direction = RandomHelper.GetRandomElement(Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray());
            var targetPosition = Point.GetPointInDirection(position, direction);
            MovementHelper.MoveCreature(creature, position, targetPosition, true, true);
            return true;
        }
    }
}