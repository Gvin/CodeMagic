using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Spells.SpellActions
{
    public interface ISpellAction
    {
        Point Perform(IAreaMap map, Point position);

        int ManaCost { get; }
    }
}