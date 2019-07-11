using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Spells;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class CastSpellAction : IPlayerAction
    {
        private readonly BookSpell spell;

        public CastSpellAction(BookSpell spell)
        {
            this.spell = spell;
        }

        public bool Perform(IPlayer player, Point playerPosition, IGameCore game, out Point newPosition)
        {
            newPosition = playerPosition;

            if (player.Mana < spell.ManaCost)
            {
                player.Mana = 0;
                game.Journal.Write(new NotEnoughManaMessage());
                return true;
            }

            player.Mana -= spell.ManaCost;
            var codeSpell = spell.CreateCodeSpell(player);
            game.Journal.Write(new SpellCastMessage(player, spell.Name));
            game.Map.AddObject(playerPosition, codeSpell);
            return true;
        }
    }
}