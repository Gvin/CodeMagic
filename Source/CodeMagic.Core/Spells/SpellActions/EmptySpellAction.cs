using System;
using CodeMagic.Core.Game;
using CodeMagic.Core.Spells.Script;

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

        public JsonData GetJson()
        {
            throw new InvalidOperationException("Empty spell action cannot produce json");
        }
    }
}