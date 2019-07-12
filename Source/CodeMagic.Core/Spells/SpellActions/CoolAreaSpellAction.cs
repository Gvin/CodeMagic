using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class CoolAreaSpellAction : SpellActionBase
    {
        public const string ActionType = "cool_area";

        private readonly int temperature;

        public CoolAreaSpellAction(dynamic actionData)
            :base(ActionType)
        {
            temperature = (int) actionData.temperature;
        }

        public override Point Perform(IGameCore game, Point position)
        {
            var cell = game.Map.GetCell(position);
            if (cell.BlocksEnvironment)
                return position;

            cell.Environment.Temperature -= temperature;
            return position;
        }

        public override int ManaCost => GetManaCost(temperature);

        public override JsonData GetJson()
        {
            return GetJson(temperature);
        }

        public static JsonData GetJson(int temperature)
        {
            if (temperature <= 0)
                throw new SpellException("Cool temperature should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"temperature", temperature},
                {"manaCost", GetManaCost(ActionType, temperature)}
            });
        }
    }
}