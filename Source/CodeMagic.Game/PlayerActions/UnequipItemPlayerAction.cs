using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UnequipItemPlayerAction : PlayerActionBase
    {
        private readonly IEquipableItem item;

        public UnequipItemPlayerAction(IEquipableItem item)
        {
            this.item = item;
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            game.Player.Equipment.UnequipItem(item);
            CurrentGame.Journal.Write(new ItemUnequipedMessage(item));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}