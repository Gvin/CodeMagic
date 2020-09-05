using CodeMagic.Game.Spells;

namespace CodeMagic.UI.Services
{
    public interface ISpellsLibraryService
    {
        void SaveSpell(BookSpell spell);

        void RemoveSpell(BookSpell spell);

        BookSpell[] ReadSpells();
    }
}