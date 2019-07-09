using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.CreaturesLogic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class PushSpellAction : ISpellAction
    {
        public const string ActionType = "push";
        private const int ManaCostMultiplier = 10;
        private const int PushDamageMultiplier = 1;

        private readonly Direction direction;
        private readonly int force;

        public PushSpellAction(dynamic actionData)
        {
            direction = ParseDirection(actionData.direction);
            force = (int) actionData.force;
        }

        public Point Perform(IAreaMap map, Point position, Journal journal)
        {
            var target = GetTarget(map, position);
            if (target == null)
                return position;

            var remainingForce = TryPush(target, map, position, out var currentPosition);
            if (remainingForce == 0)
                return position;

            var collideCellPosition = Point.GetAdjustedPoint(currentPosition, direction);
            var collideTarget = GetTarget(map, collideCellPosition);

            var damage = remainingForce * PushDamageMultiplier;

            target.Damage(damage, null);
            journal.Write(new EnvironmentDamageMessage(target, damage, null));

            if (collideTarget != null)
            {
                collideTarget.Damage(damage, null);
                journal.Write(new EnvironmentDamageMessage(collideTarget, damage, null));
            }

            return position;
        }

        private int TryPush(IDestroyableObject target, IAreaMap map, Point position, out Point currentPosition)
        {
            currentPosition = position;
            for (var remainingForce = force; remainingForce > 0; remainingForce--)
            {
                var nextPosition = Point.GetAdjustedPoint(currentPosition, direction);
                var success = MovementHelper.MoveObject(target, map, currentPosition, nextPosition);
                if (!success)
                    return remainingForce;

                currentPosition = nextPosition;
            }

            return 0;
        }

        private IDestroyableObject GetTarget(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            var destroyable = cell.Objects.OfType<IDestroyableObject>().ToArray();

            var bigObject = destroyable.FirstOrDefault(obj => obj.BlocksMovement);
            if (bigObject != null)
                return bigObject;

            return destroyable.LastOrDefault();
        }

        private Direction ParseDirection(string directionString)
        {
            var parsedDirection = SpellHelper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown push direction: {directionString}");
            return parsedDirection.Value;
        }

        public int ManaCost => GetManaCost(force);

        private static int GetManaCost(int force)
        {
            return force * ManaCostMultiplier;
        }

        public static JsonData GetJson(string direction, int force)
        {
            var parsedDirection = SpellHelper.ParseDirection(direction);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown push direction: {direction}");
            if (force <= 0)
                throw new SpellException("Force should be greater than 0.");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"direction", direction},
                {"force", force},
                {"manaCost", GetManaCost(force)}
            });
        }
    }
}