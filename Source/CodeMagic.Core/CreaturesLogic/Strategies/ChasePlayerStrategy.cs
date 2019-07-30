using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.CreaturesLogic.Strategies
{
    public class ChasePlayerStrategy : ICreatureStrategy
    {
        private readonly ICreatureMovementStrategy movementStrategy;

        public ChasePlayerStrategy(ICreatureMovementStrategy movementStrategy)
        {
            this.movementStrategy = movementStrategy;
        }

        public bool Update(INonPlayableCreatureObject creature, IGameCore game, Point position)
        {
            if (creature.Statuses.Contains(ParalyzedObjectStatus.StatusType))
                return true;

            var playerPosition = game.PlayerPosition;
            var adjustedPlayerDirection = Point.GetAdjustedPointRelativeDirection(position, playerPosition);
            if (adjustedPlayerDirection.HasValue)
            {
                creature.Direction = adjustedPlayerDirection.Value;
                creature.Attack(game.Player, game.Journal);
                return true;
            }

            TryMoveToPlayer(creature, position, playerPosition, game);
            return true;
        }

        private void TryMoveToPlayer(INonPlayableCreatureObject creature, Point position, Point playerPosition, IGameCore game)
        {
            movementStrategy.TryMove(creature, game, position, playerPosition);
        }
    }
}