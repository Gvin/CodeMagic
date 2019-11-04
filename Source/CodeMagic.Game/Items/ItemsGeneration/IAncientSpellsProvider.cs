using CodeMagic.Game.Spells;

namespace CodeMagic.Game.Items.ItemsGeneration
{
    public interface IAncientSpellsProvider
    {
        BookSpell[] GetUncommonSpells();

        BookSpell[] GetRareSpells();
    }
}