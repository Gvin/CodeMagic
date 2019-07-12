using System;
using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class DecompressSpellAction : ISpellAction
    {
        public const string ActionType = "decompress";
        private const double ManaCostMultiplier = 0.04d;
        private const int ManaCostPower = 2;

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

        /// <remarks>
        /// 100 - 16
        /// 200 - 64
        /// 300 - 121
        /// </remarks>>
        private static int GetManaCost(int pressure)
        {
            var basement = (int)Math.Ceiling(pressure * ManaCostMultiplier);
            return (int)Math.Pow(basement, ManaCostPower);
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