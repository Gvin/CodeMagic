using CodeMagic.Core.Game;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public interface ISpellAction
    {
        Point Perform(Point position);

        int ManaCost { get; }

        JsonData GetJson();
    }
}