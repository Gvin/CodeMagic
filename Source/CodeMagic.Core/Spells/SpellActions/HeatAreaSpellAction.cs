using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class HeatAreaSpellAction : ISpellAction
    {
        public const string ActionType = "heat_area";
        private const double ManaCostMultiplier = 0.05;

        private readonly int temperature;

        public HeatAreaSpellAction(dynamic actionData)
        {
            temperature = (int)actionData.temperature;
        }

        public Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            cell.Environment.Temperature += temperature;
            return position;
        }

        public int ManaCost => GetManaCost(temperature);

        private static int GetManaCost(int temperature)
        {
            return (int) Math.Ceiling(temperature * ManaCostMultiplier);
        }

        public static JsonData GetJson(int temperature)
        {
            if (temperature <= 0)
                throw new SpellException("Heat temperature should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"temperature", temperature},
                {"manaCost", GetManaCost(temperature)}
            });
        }
    }
}