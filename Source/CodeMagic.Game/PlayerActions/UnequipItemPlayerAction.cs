using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UnequipItemPlayerAction : IPlayerAction
    {
        private readonly IEquipableItem item;

        public UnequipItemPlayerAction(IEquipableItem item)
        {
            this.item = item;
        }

        public bool Perform(out Point newPosition)
        {
            ((GameCore<Player>)CurrentGame.Game).Player.Equipment.UnequipItem(item);
            CurrentGame.Journal.Write(new ItemUnequipedMessage(item));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}