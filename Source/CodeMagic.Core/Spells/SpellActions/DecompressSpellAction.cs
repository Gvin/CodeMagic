using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class DecompressSpellAction : ISpellAction
    {
        public const string ActionType = "decompress";
        private const double ManaCostMultiplier = 0.1d;

        private readonly int pressure;

        public DecompressSpellAction(dynamic actionData)
        {
            pressure = (int)actionData.pressure;
        }

        public Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            cell.Environment.Pressure -= pressure;
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