using CodeMagic.Core.Common;
using System.Linq;

namespace CodeMagic.Core.Spells
{
    public static class SpellHelper
    {
        private static readonly string[] DirectionsUp = { "up", "top", "above" };
        private static readonly string[] DirectionsDown = { "down", "bottom", "below" };
        private static readonly string[] DirectionsLeft = { "left" };
        private static readonly string[] DirectionsRight = { "right" };

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