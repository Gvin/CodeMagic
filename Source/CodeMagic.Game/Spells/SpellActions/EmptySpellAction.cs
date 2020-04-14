using System;
using CodeMagic.Core.Game;
using CodeMagic.Game.Spells.Script;

namespace CodeMagic.Game.Spells.SpellActions
{
    public class EmptySpellAction : ISpellAction
    {
        public Point Perform(Point position)
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