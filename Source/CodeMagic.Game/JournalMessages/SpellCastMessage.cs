using CodeMagic.Core.Objects;

namespace CodeMagic.Game.JournalMessages
{
    public class SpellCastMessage : SelfDescribingJournalMessage
    {
        private readonly string spellName;
        private readonly IMapObject caster;

        public SpellCastMessage(IMapObject caster, string spellName)
        {
            this.caster = caster;
            this.spellName = spellName;
        }

        public override StyledLine GetDescription()
        {
            return new StyledLine
            {
                $"{GetMapObjectName(caster)} casted spell ",
                new StyledString(spellName, SpellNameColor)
            };
        }
    }
}