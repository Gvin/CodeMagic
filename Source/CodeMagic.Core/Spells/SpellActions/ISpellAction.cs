using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;

namespace CodeMagic.Core.Spells.SpellActions
{
    public interface ISpellAction
    {
        Point Perform(IAreaMap map, Point position, Journal journal);

        int ManaCost { get; }
    }
}