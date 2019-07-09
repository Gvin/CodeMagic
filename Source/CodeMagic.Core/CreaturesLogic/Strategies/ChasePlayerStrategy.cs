using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public class ChasePlayerStrategy : ICreatureStrategy
    {
        private readonly ICreatureMovementStrategy movementStrategy;

        public ChasePlayerStrategy(ICreatureMovementStrategy movementStrategy)
        {
            this.movementStrategy = movementStrategy;
        }

        public bool Update(INonPlayableCreatureObject creature, IAreaMap map, Point position, Journal journal)
        {
            var playerPosition = map.GetObjectPosition<IPlayer>();
            var adjustedPlayerDirection = Point.GetAdjustedPointRelativeDirection(position, playerPosition);
            if (adjustedPlayerDirection.HasValue)
            {
                creature.Direction = adjustedPlayerDirection.Value;
                var player = map.GetCell(playerPosition).Objects.OfType<IPlayer>().Single();
                creature.Attack(player, journal);
                return true;
            }

            TryMoveToPlayer(creature, position, playerPosition, map);
            return true;
        }

        private void TryMoveToPlayer(INonPlayableCreatureObject creature, Point position, Point playerPosition, IAreaMap map)
        {
            movementStrategy.TryMove(creature, map, position, playerPosition);
        }
    }
}