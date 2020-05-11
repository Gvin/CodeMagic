﻿using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.Objects
{
    public static class MovementHelper
    {
        public static MovementResult MoveCreature(ICreatureObject creature, Point startPoint,
            Point endPoint, bool processStepReaction, bool changeDirection, bool avoidTraps)
        {
            if (changeDirection)
            {
                var movementDirection = Point.GetAdjustedPointRelativeDirection(startPoint, endPoint);
                if (!movementDirection.HasValue)
                    throw new ApplicationException("Unable to determine movement direction.");
                creature.Direction = movementDirection.Value;
            }

            return MoveObject(creature, startPoint, endPoint, processStepReaction, avoidTraps);
        }

        public static MovementResult MoveCreature(ICreatureObject creature, Point startPoint,
            Direction direction, bool processStepReaction, bool changeDirection, bool avoidTraps)
        {
            if (changeDirection)
            {
                creature.Direction = direction;
            }
            var endPoint = Point.GetPointInDirection(startPoint, direction);
            return MoveObject(creature, startPoint, endPoint, processStepReaction, avoidTraps);
        }

        public static MovementResult MoveObject(IMapObject mapObject, Point startPoint, Point endPoint, bool processStepReaction = true, bool avoidTraps = false)
        {
            return MoveMapObject(mapObject, startPoint, endPoint, cell => !cell.BlocksMovement, processStepReaction, avoidTraps);
        }

        public static MovementResult MoveProjectile(IMapObject projectile, Point startPoint, Point endPoint, bool avoidTraps = false)
        {
            return MoveMapObject(projectile, startPoint, endPoint, cell => !cell.BlocksProjectiles, false, avoidTraps);
        }

        private static MovementResult MoveMapObject(IMapObject mapObject, Point startPoint, Point endPoint,
            Func<IAreaMapCell, bool> canPassFilter, bool processStepReaction, bool avoidTraps)
        {
            if (!Point.IsAdjustedPoint(startPoint, endPoint))
                throw new ArgumentException("Movement points are not adjusted.");

            var nextCell = CurrentGame.Map.TryGetCell(endPoint);
            if (nextCell == null)
                return new MovementResult(startPoint, false);

            if (avoidTraps && nextCell.Objects.OfType<IDangerousObject>().Any())
                return new MovementResult(startPoint, false);

            if (!canPassFilter(nextCell))
                return new MovementResult(startPoint, false);

            CurrentGame.Map.RemoveObject(startPoint, mapObject);
            CurrentGame.Map.AddObject(endPoint, mapObject);

            if (!processStepReaction || !(mapObject is ICreatureObject creature))
                return new MovementResult(endPoint, true);

            var stepReactionObject = nextCell.Objects.OfType<IStepReactionObject>().FirstOrDefault();
            if (stepReactionObject == null)
                return new MovementResult(endPoint, true);

            var newPosition = stepReactionObject.ProcessStepOn(endPoint, creature, startPoint);
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