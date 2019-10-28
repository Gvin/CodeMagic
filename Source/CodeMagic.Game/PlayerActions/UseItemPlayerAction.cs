using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
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

        public bool Perform(IGameCore game, out Point newPosition)
        {
            game.Journal.Write(new UsedItemMessage(item));
            var keepItem = item.Use((GameCore<Player>) game);

            if (!keepItem)
            {
                game.Player.Inventory.RemoveItem(item);
            }

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}