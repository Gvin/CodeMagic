using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.CreaturesLogic.MovementStrategies;
using CodeMagic.Game.CreaturesLogic.TargetStrategies;

namespace CodeMagic.Game.CreaturesLogic.Strategies
{
    public class ChaseTargetStrategy : ICreatureStrategy
    {
        private readonly ITargetStrategy targetStrategy;
        private readonly ICreatureMovementStrategy movementStrategy;

        public ChaseTargetStrategy(ITargetStrategy targetStrategy, ICreatureMovementStrategy movementStrategy)
        {
            this.targetStrategy = targetStrategy;
            this.movementStrategy = movementStrategy;
        }

        public bool Update(INonPlayableCreatureObject creature, Point position)
        {
            if (creature.Statuses.Contains(ParalyzedObjectStatus.StatusType))
                return true;

            var visibleArea = VisibilityHelper.GetVisibleArea(creature.VisibilityRange, position);
            var relativeTargetPosition = GetNearestTargetPosition(visibleArea);
            if (relativeTargetPosition == null)
                return true;

            var absoluteTargetPosition = new Point(
                position.X - creature.VisibilityRange + relativeTargetPosition.X, 
                position.Y - creature.VisibilityRange + relativeTargetPosition.Y);
            var adjustedTargetDirection = Point.GetAdjustedPointRelativeDirection(position, absoluteTargetPosition);
            if (adjustedTargetDirection.HasValue)
            {
                AttackTarget(creature, position, absoluteTargetPosition, adjustedTargetDirection.Value);
                return true;
            }

            TryMoveToTarget(creature, position, absoluteTargetPosition);
            return true;
        }

        private IDestroyableObject GetTarget(IAreaMapCell cell)
        {
            return cell.Objects.OfType<IDestroyableObject>().FirstOrDefault(targetStrategy.IsTarget);
        }

        private Point GetNearestTargetPosition(AreaMapFragment mapFragment)
        {
            for (int y = 0; y < mapFragment.Height; y++)
            {
                for (int x = 0; x < mapFragment.Width; x++)
                {
                    var cell = mapFragment.GetCell(x, y);
                    if (cell == null)
                        continue;

                    if (cell.Objects.Any(targetStrategy.IsTarget))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return null;
        }

        private void AttackTarget(
            INonPlayableCreatureObject creature, 
            Point position, 
            Point targetPosition, 
            Direction adjustedTargetDirection)
        {
            creature.Direction = adjustedTargetDirection;

            if (CurrentGame.Map.GetCell(position).Objects.Any(obj => obj.BlocksAttack)) // Don't attack if standing inside the wall
                return;

            var targetCell = CurrentGame.Map.GetCell(targetPosition);
            var wall = targetCell.Objects.FirstOrDefault(obj => obj.BlocksAttack);
            if (wall != null)
                return;

            var target = GetTarget(targetCell);
            if (target == null)
                return;

            creature.Attack(targetPosition, target);
        }

        private void TryMoveToTarget(INonPlayableCreatureObject creature, Point position, Point playerPosition)
        {
            movementStrategy.TryMove(creature, position, playerPosition);
        }
    }
}