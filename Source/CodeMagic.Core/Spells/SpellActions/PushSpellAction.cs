﻿using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class PushSpellAction : SpellActionBase
    {
        public const string ActionType = "push";
        private const int PushDamageMultiplier = 1;

        private readonly Direction direction;
        private readonly int force;

        public PushSpellAction(dynamic actionData)
            :base(ActionType)
        {
            direction = ParseDirection(actionData.direction);
            force = (int) actionData.force;
        }

        public override  Point Perform(IGameCore game, Point position)
        {
            var target = GetTarget(game.Map, position);
            if (target == null)
                return position;

            var remainingForce = TryPush(target, game, position, out var currentPosition);
            if (remainingForce == 0)
                return position;

            var collideCellPosition = Point.GetPointInDirection(currentPosition, direction);
            var collideTarget = GetTarget(game.Map, collideCellPosition);

            var damage = remainingForce * PushDamageMultiplier;

            target.Damage(damage);
            game.Journal.Write(new EnvironmentDamageMessage(target, damage));

            if (collideTarget != null)
            {
                collideTarget.Damage(damage);
                game.Journal.Write(new EnvironmentDamageMessage(collideTarget, damage));
            }

            return position;
        }

        private int TryPush(IDestroyableObject target, IGameCore game, Point position, out Point currentPosition)
        {
            currentPosition = position;
            for (var remainingForce = force; remainingForce > 0; remainingForce--)
            {
                var nextPosition = Point.GetPointInDirection(currentPosition, direction);
                var movementResult = MovementHelper.MoveObject(target, game, currentPosition, nextPosition);
                if (!movementResult.Success)
                    return remainingForce;

                if (!movementResult.NewPosition.Equals(nextPosition))
                    return 0;
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

        public override int ManaCost => GetManaCost(force);

        public override JsonData GetJson()
        {
            return GetJson(JsonData.GetDirectionString(direction), force);
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
                {"manaCost", GetManaCost(ActionType, force)}
            });
        }
    }
}