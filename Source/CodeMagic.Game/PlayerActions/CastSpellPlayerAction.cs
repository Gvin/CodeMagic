using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.ObjectEffects;
using CodeMagic.Game.Spells;

namespace CodeMagic.Game.PlayerActions
{
    public class CastSpellPlayerAction : IPlayerAction
    {
        private readonly BookSpell spell;

        public CastSpellPlayerAction(BookSpell spell)
        {
            this.spell = spell;
        }

        public bool Perform(out Point newPosition)
        {
            newPosition = CurrentGame.Game.PlayerPosition;

            if (CurrentGame.Player.Mana < spell.ManaCost)
            {
                CurrentGame.Player.Mana = 0;
                CurrentGame.Journal.Write(new NotEnoughManaToCastMessage());
                return true;
            }

            CurrentGame.Player.Mana -= spell.ManaCost;
            var codeSpell = spell.CreateCodeSpell(CurrentGame.Player);
            CurrentGame.Journal.Write(new SpellCastMessage(CurrentGame.Player, spell.Name));
            CurrentGame.Map.AddObject(CurrentGame.Game.PlayerPosition, codeSpell);
            CurrentGame.Player.ObjectEffects.Add(new SpellCastEffect());
            return true;
        }
    }
}