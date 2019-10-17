using CodeMagic.Core.Spells;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public interface IAncientSpellsProvider
    {
        BookSpell[] GetUncommonSpells();

        BookSpell[] GetRareSpells();
    }
}