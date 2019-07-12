using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CoolAreaSpellAction : ISpellAction
    {
        public const string ActionType = "cool_area";

        private const int ManaCostPower = 2;
        private const double ManaCostMultiplier = 0.2d;

        private readonly int temperature;

        public CoolAreaSpellAction(dynamic actionData)
        {
            temperature = (int) actionData.temperature;
        }

        public Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            cell.Environment.Temperature -= temperature;
            return position;
        }

        public int ManaCost => GetManaCost(temperature);

        /// <remarks>
        /// 1-5 - 1
        /// 6-10 - 4
        /// 11-15 - 9
        /// 16-20 - 16
        /// 21-25 - 25
        /// </remarks>
        private static int GetManaCost(int temperature)
        {
            var basement = (int) Math.Ceiling(ManaCostMultiplier * temperature);
            return (int) Math.Pow(basement, ManaCostPower);
        }

        public static JsonData GetJson(int temperature)
        {
            if (temperature <= 0)
                throw new SpellException("Cool temperature should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"temperature", temperature},
                {"manaCost", GetManaCost(temperature)}
            });
        }
    }
}