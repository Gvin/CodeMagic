using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class MoveSpellAction : ISpellAction
    {
        public const string ActionType = "move";

        private const int ManaCostMultiplier = 1;

        private readonly Direction direction;
        private readonly int distance;
        private readonly CodeSpell spell;

        public MoveSpellAction(dynamic actionData, CodeSpell spell)
        {
            direction = ParseDirection((string)actionData.direction);
            distance = (int)actionData.distance;
            this.spell = spell;
        }

        public Point Perform(IAreaMap map, Point position, Journal journal)
        {
            var currentPosition = position;

            for (var step = 1; step <= distance; step++)
            {
                var newPosition = Point.GetAdjustedPoint(currentPosition, direction);
                var success = MovementHelper.MoveSpell(spell, map, currentPosition, newPosition);
                if (!success)
                    break;

                currentPosition = newPosition;
            }

            return currentPosition;
        }

        private Direction ParseDirection(string directionString)
        {
            var parsedDirection = SpellHelper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown move direction: {directionString}");
            return parsedDirection.Value;
        }

        public int ManaCost => GetManaCost(distance);

        private static int GetManaCost(int distance)
        {
            return distance * ManaCostMultiplier;
        }

        public static JsonData GetJson(string direction, int distance)
        {
            var parsedDirection = SpellHelper.ParseDirection(direction);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown move direction: {direction}");
            if (distance <= 0)
                throw new SpellException("Move distance should be greater than 0.");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"direction", direction},
                {"distance", distance},
                {"manaCost", GetManaCost(distance)}
            });
        }
    }
}