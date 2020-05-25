using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UseItemPlayerAction : IPlayerAction
    {
        private readonly IUsableItem item;

        public UseItemPlayerAction(IUsableItem item)
        {
            this.item = item;
        }

        public bool Perform(out Point newPosition)
        {
            CurrentGame.Journal.Write(new UsedItemMessage(item));
            var keepItem = item.Use((GameCore<Player>)CurrentGame.Game);

            if (!keepItem)
            {
                CurrentGame.Player.Inventory.RemoveItem(item);
            }

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}