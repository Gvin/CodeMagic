using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Objects
{
    public static class MovementHelper
    {
        public static MovementResult MoveCreature(ICreatureObject creature, IGameCore game, Point startPoint,
            Point endPoint, bool processStepReaction = true, bool changeDirection = true)
        {
            if (changeDirection)
            {
                var movementDirection = Point.GetAdjustedPointRelativeDirection(startPoint, endPoint);
                if (!movementDirection.HasValue)
                    throw new ApplicationException("Unable to determine movement direction.");
                creature.Direction = movementDirection.Value;
            }

            return MoveObject(creature, game, startPoint, endPoint, processStepReaction);
        }

        public static MovementResult MoveCreature(ICreatureObject creature, IGameCore game, Point startPoint,
            Direction direction, bool processStepReaction = true, bool changeDirection = true)
        {
            if (changeDirection)
            {
                creature.Direction = direction;
            }
            var endPoint = Point.GetAdjustedPoint(startPoint, direction);
            return MoveObject(creature, game, startPoint, endPoint, processStepReaction);
        }

        public static MovementResult MoveObject(IMapObject mapObject, IGameCore game, Point startPoint, Point endPoint, bool processStepReaction = true)
        {
            return MoveMapObject(mapObject, game, startPoint, endPoint, cell => !cell.BlocksMovement, processStepReaction);
        }

        public static MovementResult MoveSpell(CodeSpell spell, IGameCore game, Point startPoint, Point endPoint)
        {
            return MoveMapObject(spell, game, startPoint, endPoint, cell => !cell.BlocksProjectiles, false);
        }

        private static MovementResult MoveMapObject(IMapObject mapObject, IGameCore game, Point startPoint, Point endPoint,
            Func<AreaMapCell, bool> canPassFilter, bool processStepReaction)
        {
            if (!Point.IsAdjustedPoint(startPoint, endPoint))
                throw new ArgumentException("Movement points are not adjusted.");

            if (!game.Map.ContainsCell(endPoint))
                return new MovementResult(startPoint, false);

            var nextCell = game.Map.GetCell(endPoint);
            if (!canPassFilter(nextCell))
                return new MovementResult(startPoint, false);

            var currentCell = game.Map.GetCell(startPoint);

            currentCell.Objects.Remove(mapObject);
            nextCell.Objects.Add(mapObject);

            if (!processStepReaction || !(mapObject is ICreatureObject creature))
                return new MovementResult(endPoint, true);

            var stepReactionObject = nextCell.Objects.OfType<IStepReactionObject>().FirstOrDefault();
            if (stepReactionObject == null)
                return new MovementResult(endPoint, true);

            var newPosition = stepReactionObject.ProcessStepOn(game, endPoint, creature, startPoint);
            return new MovementResult(newPosition, true);
        }
    }

    public class MovementResult
    {
        public MovementResult(Point newPosition, bool success)
        {
            NewPosition = newPosition;
            Success = success;
        }

        public Point NewPosition { get; }

        public bool Success { get; }
    }
}