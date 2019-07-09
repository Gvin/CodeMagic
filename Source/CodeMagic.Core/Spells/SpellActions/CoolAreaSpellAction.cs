using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CoolAreaSpellAction : ISpellAction
    {
        public const string ActionType = "cool_area";

        private const int ManaCostMultiplier = 1;

        private readonly int temperature;

        public CoolAreaSpellAction(dynamic actionData)
        {
            temperature = (int) actionData.temperature;
        }

        public Point Perform(IAreaMap map, Point position)
        {
            var cell = map.GetCell(position);
            cell.Temperature.Value -= temperature;
            return position;
        }

        public int ManaCost => GetManaCost(temperature);

        private static int GetManaCost(int temperature)
        {
            return ManaCostMultiplier * temperature;
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