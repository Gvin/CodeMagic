using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.Creatures;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class CreatureMovementHelper
    {
        public static bool MoveCreature(ICreatureObject creature, IAreaMap map, Point startPoint, Point endPoint)
        {
            var movementDirection = Point.GetAdjustedPointRelativeDirection(startPoint, endPoint);
            if (!movementDirection.HasValue)
                throw new ApplicationException("Unable to determine movement direction.");
            creature.Direction = movementDirection.Value;

            if (!map.ContainsCell(endPoint))
                return false;

            var nextCell = map.GetCell(endPoint);
            if (nextCell.BlocksMovement)
                return false;

            var currentCell = map.GetCell(startPoint);
            
            currentCell.Objects.Remove(creature);
            nextCell.Objects.Add(creature);
            return true;
        }
    }
}