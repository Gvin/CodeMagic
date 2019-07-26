using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class EmitLightSpellAction : SpellActionBase
    {
        public const string ActionType = "emit_light";

        private readonly int power;
        private readonly int time;
        private readonly ICodeSpell spell;

        public EmitLightSpellAction(dynamic actionData, ICodeSpell spell) : base(ActionType)
        {
            power = (int) actionData.power;
            if (power > (int) LightLevel.Blinding)
                power = (int) LightLevel.Blinding;

            time = (int) actionData.time;
            this.spell = spell;
        }

        public override Point Perform(IGameCore game, Point position)
        {
            spell.SetEmitLight((LightLevel) power, time);
            return position;
        }

        public override int ManaCost => GetManaCost(power * time);

        public override JsonData GetJson()
        {
            return GetJson(power, time);
        }

        public static JsonData GetJson(int power, int time)
        {
            if (power <= 0)
                throw new SpellException("Light power should be greater than 0");
            if (time <= 0)
                throw new SpellException("Light emit time should be greater than 0");

            return new JsonData(new Dictionary<string, object>
            {
                {"type", ActionType},
                {"power", power},
                {"time", time},
                {"manaCost", GetManaCost(ActionType, power * time)}
            });
        }
    }
}