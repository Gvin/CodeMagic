using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CompressSpellAction : ISpellAction
    {
        public const string ActionType = "compress";
        private const double ManaCostMultiplier = 0.1d;

        private readonly int pressure;

        public CompressSpellAction(dynamic actionData)
        {
            pressure = (int) actionData.pressure;
        }

        public Point Perform(IAreaMap map, Point position, Journal journal)
        {
            var cell = map.GetCell(position);
            cell.Environment.Pressure += pressure;
            return position;
        }

        public int ManaCost => GetManaCost(pressure);

        private static int GetManaCost(int pressure)
        {
            return (int)Math.Ceiling(pressure * ManaCostMultiplier);
        }

        public static JsonData GetJson(int pressure)
        {
            if (pressure <= 0)
                throw new SpellException("Pressure value should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"pressure", pressure},
                {"manaCost", GetManaCost(pressure)}
            });
        }
    }
}