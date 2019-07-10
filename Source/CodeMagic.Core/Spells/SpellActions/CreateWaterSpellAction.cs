using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Area.Liquids;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CreateWaterSpellAction : ISpellAction
    {
        public const string ActionType = "create_water";
        private const int ManaCostMultiplier = 1;

        private readonly int volume;

        public CreateWaterSpellAction(dynamic actionData)
        {
            volume = (int)actionData.volume;
        }

        public Point Perform(IAreaMap map, Point position, Journal journal)
        {
            var cell = map.GetCell(position);
            cell.Liquids.AddLiquid(new WaterLiquid(volume));
            return position;
        }

        public int ManaCost => GetManaCost(volume);

        private static int GetManaCost(int volume)
        {
            return ManaCostMultiplier * volume;
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