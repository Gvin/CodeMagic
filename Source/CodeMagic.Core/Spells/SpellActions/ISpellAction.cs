using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public interface ISpellAction
    {
        Point Perform(IAreaMap map, IJournal journal, Point position);

        int ManaCost { get; }

        JsonData GetJson();
    }
}