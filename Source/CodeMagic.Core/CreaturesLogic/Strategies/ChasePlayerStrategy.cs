using System.Linq;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic.MovementStrategies;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Objects.SolidObjects;
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
                AttackPlayer(game, creature, position, adjustedPlayerDirection.Value);
                return true;
            }

            TryMoveToPlayer(creature, position, playerPosition, game);
            return true;
        }

        private void AttackPlayer(IGameCore game, INonPlayableCreatureObject creature, Point position, Direction adjustedPlayerDirection)
        {
            creature.Direction = adjustedPlayerDirection;

            if (game.Map.GetCell(position).Objects.OfType<IWallObject>().Any())
                return;

            var playerCell = game.Map.GetCell(game.PlayerPosition);
            var wall = playerCell.Objects.FirstOrDefault(obj => obj is IWallObject);
            if (wall == null)
            {
                creature.Attack(game.Player, game.Journal);
            }
        }

        private void TryMoveToPlayer(INonPlayableCreatureObject creature, Point position, Point playerPosition, IGameCore game)
        {
            movementStrategy.TryMove(creature, game, position, playerPosition);
        }
    }
}