using System;
using System.Linq;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Common
{
    public static class DirectionHelper
    {
        public static Direction InvertDirection(Direction direction)
        {
            switch (direction)
            {
                case Direction.North:
                    return Direction.South;
                case Direction.South:
                    return Direction.North;
                case Direction.West:
                    return Direction.East;
                case Direction.East:
                    return Direction.West;
                default:
                    throw new ArgumentException($"Unknown direction: {direction}");
            }
        }

        public static Direction[] GetAllDirections()
        {
            return Enum.GetValues(typeof(Direction)).Cast<Direction>().ToArray();
        }

        public static Direction GetRandomDirection()
        {
            return RandomHelper.GetRandomElement(GetAllDirections());
        }

        public static Direction GetNextDirection(Direction direction)
        {
            var directions = GetAllDirections().ToList();
            var index = directions.IndexOf(direction);
            index++;
            if (index >= directions.Count)
            {
                index = 0;
            }

            return directions[index];
        }
    }
}