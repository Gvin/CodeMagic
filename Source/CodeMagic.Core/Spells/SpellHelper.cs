using CodeMagic.Core.Common;
using System.Linq;

namespace CodeMagic.Core.Spells
{
    public static class SpellHelper
    {
        private static readonly string[] DirectionsUp = { "up", "top", "above", "forward", "front", "u", "^", "north", "n" };
        private static readonly string[] DirectionsDown = { "down", "bottom", "below", "backward", "back", "d", "v", "south", "s" };
        private static readonly string[] DirectionsLeft = { "left", "l", "<", "west", "w" };
        private static readonly string[] DirectionsRight = { "right", "r", ">", "east", "e" };

        public static Direction? ParseDirection(string direction)
        {
            var directionStringFormatted = direction.ToLower();
            if (DirectionsUp.Contains(directionStringFormatted))
                return Direction.North;
            if (DirectionsDown.Contains(directionStringFormatted))
                return Direction.South;
            if (DirectionsLeft.Contains(directionStringFormatted))
                return Direction.West;
            if (DirectionsRight.Contains(directionStringFormatted))
                return Direction.East;

            return null;
        }
    }
}