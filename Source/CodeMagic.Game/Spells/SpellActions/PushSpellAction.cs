﻿using System;
using System.Collections.Generic;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class PushSpellAction : SpellActionBase
    {
        public const string ActionType = "push";

        private readonly Direction direction;
        private readonly int force;

        public PushSpellAction(dynamic actionData)
            :base(ActionType)
        {
            direction = ParseDirection(actionData.direction);
            force = (int) actionData.force;
        }

        public override Point Perform(IAreaMap map, IJournal journal, Point position)
        {
            var target = GetTarget(map, position);
            if (target == null)
                return position;

            var remainingForce = TryPush(target, map, journal, position, out var currentPosition);
            if (remainingForce == 0)
                return position;

            var collideCellPosition = Point.GetPointInDirection(currentPosition, direction);

            var damage = GetCollideDamage(target, remainingForce);

            if (target is IDestroyableObject destroyableTarget)
            {
                destroyableTarget.Damage(journal, damage, Element.Blunt);
                journal.Write(new EnvironmentDamageMessage(target, damage, Element.Blunt));
            }

            ApplyCollideDamageToCell(map, journal, collideCellPosition, damage);

            return position;
        }

        private void ApplyCollideDamageToCell(IAreaMap map, IJournal journal, Point position, int damage)
        {
            var cell = map.TryGetCell(position);
            if (cell == null)
                return;

            var collidedObjects = cell.Objects.OfType<IDestroyableObject>();
            foreach (var collidedObject in collidedObjects)
            {
                collidedObject.Damage(journal, damage, Element.Blunt);
                journal.Write(new EnvironmentDamageMessage(collidedObject, damage, Element.Blunt));
            }
        }

        private int TryPush(IMapObject target, IAreaMap map, IJournal journal, Point position, out Point currentPosition)
        {
            currentPosition = position;
            var initialForce = GetInitialForce(target);
            for (var remainingForce = initialForce; remainingForce > 0; remainingForce--)
            {
                var nextPosition = Point.GetPointInDirection(currentPosition, direction);
                var movementResult = MovementHelper.MoveObject(target, map, journal, currentPosition, nextPosition);
                if (!movementResult.Success)
                    return remainingForce;

                if (!movementResult.NewPosition.Equals(nextPosition))
                    return 0;
                currentPosition = nextPosition;
            }

            return 0;
        }

        private int GetCollideDamage(IMapObject target, int remainingForce)
        {
            return (int) Math.Round(remainingForce * GetDamageMultiplier(target.Size));
        }

        private float GetDamageMultiplier(ObjectSize size)
        {
            switch (size)
            {
                case ObjectSize.Small:
                    return 2;
                case ObjectSize.Medium:
                    return 4;
                case ObjectSize.Big:
                    return 8;
                case ObjectSize.Huge:
                    throw new InvalidOperationException("It is impossible to throw Huge objects.");
                default:
                    throw new ArgumentOutOfRangeException($"Unknown object size: {size}");
            }
        }

        private int GetInitialForce(IMapObject target)
        {
            return (int) Math.Round(force * GetForceMultiplier(target.Size));
        }

        private float GetForceMultiplier(ObjectSize size)
        {
            switch (size)
            {
                case ObjectSize.Small:
                    return 2;
                case ObjectSize.Medium:
                    return 1;
                case ObjectSize.Big:
                    return 0.5f;
                case ObjectSize.Huge:
                    throw new InvalidOperationException("It is impossible to throw Huge objects.");
                default:
                    throw new ArgumentOutOfRangeException($"Unknown object size: {size}");
            }
        }

        private IMapObject GetTarget(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            return cell.Objects
                .Where(obj => obj.Size < ObjectSize.Huge)
                .OrderByDescending(obj => obj.Size)
                .FirstOrDefault();
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