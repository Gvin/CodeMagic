using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

namespace CodeMagic.Core.Spells.SpellActions
{
    public interface ISpellAction
    {
        Point Perform(IGameCore game, Point position);

        int ManaCost { get; }

        JsonData GetJson();
    }
}