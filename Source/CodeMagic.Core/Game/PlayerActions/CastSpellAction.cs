using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
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

        public bool Perform(IPlayer player, Point playerPosition, IAreaMap map, Journal journal)
        {
            if (player.Mana < spell.ManaCost)
            {
                player.Mana = 0;
                journal.Write(new NotEnoughManaMessage());
                return true;
            }

            player.Mana -= spell.ManaCost;
            var codeSpell = spell.CreateCodeSpell(player);
            journal.Write(new SpellCastMessage(player, spell.Name));
            map.AddObject(playerPosition, codeSpell);
            return true;
        }
    }
}