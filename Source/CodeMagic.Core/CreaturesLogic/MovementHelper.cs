using System;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.Creatures;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.CreaturesLogic
{
    public static class MovementHelper
    {
        public static bool MoveCreature(ICreatureObject creature, IAreaMap map, Point startPoint, Point endPoint, bool changeDirection = true)
        {
            if (changeDirection)
            {
                var movementDirection = Point.GetAdjustedPointRelativeDirection(startPoint, endPoint);
                if (!movementDirection.HasValue)
                    throw new ApplicationException("Unable to determine movement direction.");
                creature.Direction = movementDirection.Value;
            }

            return MoveObject(creature, map, startPoint, endPoint);
        }

        public static bool MoveObject(IMapObject mapObject, IAreaMap map, Point startPoint, Point endPoint)
        {
            return MoveMapObject(mapObject, map, startPoint, endPoint, cell => !cell.BlocksMovement);
        }

        public static bool MoveSpell(CodeSpell spell, IAreaMap map, Point startPoint, Point endPoint)
        {
            return MoveMapObject(spell, map, startPoint, endPoint, cell => !cell.BlocksProjectiles);
        }

        private static bool MoveMapObject(IMapObject mapObject, IAreaMap map, Point startPoint, Point endPoint,
            Func<AreaMapCell, bool> canPassFilter)
        {
            if (!Point.IsAdjustedPoint(startPoint, endPoint))
                throw new ArgumentException("Movement points are not adjusted.");

            if (!map.ContainsCell(endPoint))
                return false;

            var nextCell = map.GetCell(endPoint);
            if (!canPassFilter(nextCell))
                return false;

            var currentCell = map.GetCell(startPoint);

            currentCell.Objects.Remove(mapObject);
            nextCell.Objects.Add(mapObject);
            return true;
        }
    }
}