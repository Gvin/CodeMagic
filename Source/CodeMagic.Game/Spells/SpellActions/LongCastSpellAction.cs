﻿using System;
using System.Collections.Generic;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Game.Configuration;
using CodeMagic.Game.Configuration.Spells;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class LongCastSpellAction : ISpellAction
    {
        public const string ActionType = "long_cast";

        private readonly ISpellAction action;
        private readonly Direction direction;
        private readonly int distance;
        private readonly CodeSpell spell;

        private readonly double manaCostMultiplier;
        private readonly int manaCostPower;

        public LongCastSpellAction(dynamic actionData, CodeSpell spell)
        {
            var configuration = GetConfiguration();
            manaCostMultiplier = configuration.ManaCostMultiplier;
            manaCostPower = configuration.ManaCostPower;

            this.spell = spell;
            action = ParseSpellAction(actionData.action);
            direction = ParseDirection((string) actionData.direction);
            distance = (int) actionData.distance;
        }

        public Point Perform(Point position)
        {
            var castPosition = Point.GetPointInDirection(position, direction, distance);
            if (!CurrentGame.Map.ContainsCell(castPosition))
                return position;

            action.Perform(castPosition);
            return position;
        }

        private ISpellAction ParseSpellAction(dynamic actionData)
        {
            var factory = new SpellActionsFactory();
            return factory.GetSpellAction(actionData, spell);
        }

        private Direction ParseDirection(string directionString)
        {
            var parsedDirection = SpellHelper.ParseDirection(directionString);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown direction: {directionString}");
            return parsedDirection.Value;
        }

        public int ManaCost => GetManaCost(action.ManaCost, distance, manaCostMultiplier, manaCostPower);

        public JsonData GetJson()
        {
            return GetJson(action, JsonData.GetDirectionString(direction), distance);
        }

        private static int GetManaCost(int sourceActionCost, int distance, double manaCostMultiplier, int manaCostPower)
        {
            var basement = (int) Math.Ceiling(distance * manaCostMultiplier);
            var costMultiplier = (int) Math.Pow(basement, manaCostPower);
            return sourceActionCost * costMultiplier;
        }

        private static ISpellConfiguration GetConfiguration()
        {
            var configuration = ConfigurationManager.GetSpellConfiguration(ActionType);
            if (configuration == null)
                throw new ApplicationException($"Configuration for spell {ActionType} not found.");
            return configuration;
        }

        public static JsonData GetJson(dynamic actionData, string direction, int distance)
        {
            var parsedDirection = SpellHelper.ParseDirection(direction);
            if (!parsedDirection.HasValue)
                throw new SpellException($"Unknown direction: {direction}");
            if (distance <= 0)
                throw new SpellException("Cast distance should be greater than 0.");

            var configuration = GetConfiguration();

            var action = new SpellActionsFactory().GetSpellAction(actionData, null);
            if (action is MoveSpellAction)
                throw new SpellException("Move action cannot be casted on distance from the spell.");

            var manaCost = GetManaCost(action.ManaCost, distance, configuration.ManaCostMultiplier,
                configuration.ManaCostPower);

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"action", action.GetJson()},
                {"direction", direction},
                {"distance", distance},
                {"manaCost", manaCost}
            });
        }
    }
}