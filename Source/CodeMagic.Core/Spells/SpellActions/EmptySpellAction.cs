using CodeMagic.Core.Area;
using CodeMagic.Core.Game;

namespace CodeMagic.Core.Spells.SpellActions
{
    public class EmptySpellAction : ISpellAction
    {
        public Point Perform(IGameCore fame, Point position)
        {
            // Do nothing
            return position;
        }

        public int ManaCost => 1;
    }
}