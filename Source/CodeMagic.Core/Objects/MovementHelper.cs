using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public static class MovementHelper
    {
        public static MovementResult MoveCreature(ICreatureObject creature, IAreaMap map, IJournal journal, Point startPoint,
            Point endPoint, bool processStepReaction = true, bool changeDirection = true)
        {
            if (changeDirection)
            {
                var movementDirection = Point.GetAdjustedPointRelativeDirection(startPoint, endPoint);
                if (!movementDirection.HasValue)
                    throw new ApplicationException("Unable to determine movement direction.");
                creature.Direction = movementDirection.Value;
            }

            return MoveObject(creature, map, journal, startPoint, endPoint, processStepReaction);
        }

        public static MovementResult MoveCreature(ICreatureObject creature, IAreaMap map, IJournal journal, Point startPoint,
            Direction direction, bool processStepReaction = true, bool changeDirection = true)
        {
            if (changeDirection)
            {
                creature.Direction = direction;
            }
            var endPoint = Point.GetPointInDirection(startPoint, direction);
            return MoveObject(creature, map, journal, startPoint, endPoint, processStepReaction);
        }

        public static MovementResult MoveObject(IMapObject mapObject, IAreaMap map, IJournal journal, Point startPoint, Point endPoint, bool processStepReaction = true)
        {
            return MoveMapObject(mapObject, map, journal, startPoint, endPoint, cell => !cell.BlocksMovement, processStepReaction);
        }

        public static MovementResult MoveProjectile(IMapObject projectile, IAreaMap map, IJournal journal, Point startPoint, Point endPoint)
        {
            return MoveMapObject(projectile, map, journal, startPoint, endPoint, cell => !cell.BlocksProjectiles, false);
        }

        private static MovementResult MoveMapObject(IMapObject mapObject, IAreaMap map, IJournal journal, Point startPoint, Point endPoint,
            Func<IAreaMapCell, bool> canPassFilter, bool processStepReaction)
        {
            if (!Point.IsAdjustedPoint(startPoint, endPoint))
                throw new ArgumentException("Movement points are not adjusted.");

            var nextCell = map.TryGetCell(endPoint);
            if (nextCell == null)
                return new MovementResult(startPoint, false, true);

            if (!canPassFilter(nextCell))
                return new MovementResult(startPoint, false);

            map.RemoveObject(startPoint, mapObject);
            map.AddObject(endPoint, mapObject);

            if (!processStepReaction || !(mapObject is ICreatureObject creature))
                return new MovementResult(endPoint, true);

            var stepReactionObject = nextCell.Objects.OfType<IStepReactionObject>().FirstOrDefault();
            if (stepReactionObject == null)
                return new MovementResult(endPoint, true);

            var newPosition = stepReactionObject.ProcessStepOn(map, journal, endPoint, creature, startPoint);
            return new MovementResult(newPosition, true);
        }
    }

    public class MovementResult
    {
        public MovementResult(Point newPosition, bool success, bool locationEdge = false)
        {
            NewPosition = newPosition;
            Success = success;
            LocationEdge = locationEdge;
        }

        public bool LocationEdge { get; }

        public Point NewPosition { get; }

        public bool Success { get; }
    }
}