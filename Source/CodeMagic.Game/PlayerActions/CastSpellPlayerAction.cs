using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Objects.ObjectEffects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Locations;
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

        public bool Perform(IGameCore game, out Point newPosition)
        {
            newPosition = game.PlayerPosition;

            if (!game.World.CurrentLocation.CanCast())
            {
                game.Journal.Write(new CastNotAllowedMessage());
                return false;
            }

            if (game.Player.Mana < spell.ManaCost)
            {
                game.Player.Mana = 0;
                game.Journal.Write(new NotEnoughManaToCastMessage());
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