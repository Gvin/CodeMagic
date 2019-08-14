using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class CastSpellPlayerAction : IPlayerAction
    {
        private readonly BookSpell spell;

        public CastSpellPlayerAction(BookSpell spell)
        {
            this.spell = spell;
        }

        public bool Perform(IGameCore game, out Point newPosition)
        {
            newPosition = game.PlayerPosition;

            if (game.Player.Mana < spell.ManaCost)
            {
                game.Player.Mana = 0;
                game.Journal.Write(new NotEnoughManaMessage());
                return true;
            }

            game.Player.Mana -= spell.ManaCost;
            var codeSpell = spell.CreateCodeSpell(game.Player);
            game.Journal.Write(new SpellCastMessage(game.Player, spell.Name));
            game.Map.AddObject(game.PlayerPosition, codeSpell);
            game.Player.ObjectEffects.Add(Injector.Current.Create<ISpellCastEffect>());
            return true;
        }
    }
}