using System.Collections.Generic;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.LiquidObjects;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class CreateWaterSpellAction : SpellActionBase
    {
        public const string ActionType = "create_water";

        private readonly int volume;

        public CreateWaterSpellAction(dynamic actionData)
            :base(ActionType)
        {
            volume = (int)actionData.volume;
        }

        public override Point Perform(Point position)
        {
            if (CurrentGame.Map.GetCell(position).BlocksMovement)
                return position;

            CurrentGame.Map.AddObject(position, new WaterLiquid(volume));
            return position;
        }

        public override int ManaCost => GetManaCost(volume);

        public override JsonData GetJson()
        {
            return GetJson(volume);
        }

        public static JsonData GetJson(int volume)
        {
            if (volume <= 0)
                throw new SpellException("Water volume should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"volume", volume},
                {"manaCost", GetManaCost(ActionType, volume)}
            });
        }
    }
}