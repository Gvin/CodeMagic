using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class DecompressSpellAction : SpellActionBase
    {
        public const string ActionType = "decompress";

        private readonly int pressure;

        public DecompressSpellAction(dynamic actionData)
            :base(ActionType)
        {
            pressure = (int)actionData.pressure;
        }

        public override Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (cell.BlocksEnvironment)
                return position;

            cell.Environment.Pressure -= pressure;
            return position;
        }

        public override int ManaCost => GetManaCost(pressure);

        public override JsonData GetJson()
        {
            return GetJson(pressure);
        }

        public static JsonData GetJson(int pressure)
        {
            if (pressure <= 0)
                throw new SpellException("Pressure value should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"pressure", pressure},
                {"manaCost", GetManaCost(ActionType, pressure)}
            });
        }
    }
}