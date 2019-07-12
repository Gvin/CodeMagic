using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class MoveSpellAction : SpellActionBase
    {
        public const string ActionType = "move";

        private readonly Direction direction;
        private readonly int distance;
        private readonly CodeSpell spell;

        public MoveSpellAction(dynamic actionData, CodeSpell spell)
            :base(ActionType)
        {
            direction = ParseDirection((string)actionData.direction);
            distance = (int)actionData.distance;
            this.spell = spell;
        }

        public override Point Perform(IGameCore game, Point position)
        {
            var currentPosition = position;

            for (var step = 1; step <= distance; step++)
            {
                var newPosition = Point.GetPointInDirection(currentPosition, direction);
                var movementResult = MovementHelper.MoveSpell(spell, game, currentPosition, newPosition);
                if (!movementResult.Success)
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

        public override int ManaCost => GetManaCost(distance);

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
                {"manaCost", GetManaCost(ActionType, distance)}
            });
        }

        public override JsonData GetJson()
        {
            return GetJson(JsonData.GetDirectionString(direction), distance);
        }
    }
}