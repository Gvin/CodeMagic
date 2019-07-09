using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.Journaling.Messages
{
    public class SpellCastMessage : IJournalMessage
    {
        public SpellCastMessage(IMapObject caster, string spellName)
        {
            Caster = caster;
            SpellName = spellName;
        }

        public IMapObject Caster { get; }

        public string SpellName { get; }
    }
}