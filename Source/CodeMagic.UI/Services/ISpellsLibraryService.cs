using CodeMagic.Game.Spells;

namespace CodeMagic.UI
{
    public interface ISpellsLibraryService
    {
        void SaveSpell(BookSpell spell);

        void RemoveSpell(BookSpell spell);

        BookSpell[] ReadSpells();
    }
}