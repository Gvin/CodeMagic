using CodeMagic.Core.Spells;

namespace CodeMagic.ItemsGeneration
{
    public interface IAncientSpellsProvider
    {
        BookSpell[] GetUncommonSpells();

        BookSpell[] GetRareSpells();
    }
}