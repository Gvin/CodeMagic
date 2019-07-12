using CodeMagic.Core.Common;
using System.Linq;

namespace CodeMagic.Core.Spells
{
    public static class SpellHelper
    {
        private static readonly string[] DirectionsUp = { "up", "top", "above", "forward", "front", "u", "^" };
        private static readonly string[] DirectionsDown = { "down", "bottom", "below", "backward", "back", "d", "v" };
        private static readonly string[] DirectionsLeft = { "left", "l", "<" };
        private static readonly string[] DirectionsRight = { "right", "r", ">" };

        public static Direction? ParseDirection(string direction)
        {
            var directionStringFormatted = direction.ToLower();
            if (DirectionsUp.Contains(directionStringFormatted))
                return Direction.Up;
            if (DirectionsDown.Contains(directionStringFormatted))
                return Direction.Down;
            if (DirectionsLeft.Contains(directionStringFormatted))
                return Direction.Left;
            if (DirectionsRight.Contains(directionStringFormatted))
                return Direction.Right;

            return null;
        }
    }
}