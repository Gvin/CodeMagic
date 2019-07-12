using System;
using System.Collections.Generic;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CreateWaterSpellAction : ISpellAction
    {
        public const string ActionType = "create_water";
        private const int ManaCostPower = 2;
        private const double ManaCostMultiplier = 0.2d;

        private readonly int volume;

        public CreateWaterSpellAction(dynamic actionData)
        {
            volume = (int)actionData.volume;
        }

        public Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            cell.Liquids.AddLiquid(new WaterLiquid(volume));
            return position;
        }

        public int ManaCost => GetManaCost(volume);

        ///<remarks>
        /// 1-5 - 1
        /// 6-10 - 4
        /// 11-15 - 9
        /// 16-20 - 16
        /// 21-25 - 25
        /// </remarks>
        private static int GetManaCost(int volume)
        {
            var basement = (int) Math.Ceiling(volume * ManaCostMultiplier);
            return (int) Math.Pow(basement, ManaCostPower);
        }

        public static JsonData GetJson(int volume)
        {
            if (volume <= 0)
                throw new SpellException("Water volume should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"volume", volume},
                {"manaCost", GetManaCost(volume)}
            });
        }
    }
}