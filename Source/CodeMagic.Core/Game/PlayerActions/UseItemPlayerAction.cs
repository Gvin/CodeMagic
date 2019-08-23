using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.PlayerActions
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
            var keepItem = item.Use(game);

            if (!keepItem)
            {
                game.Player.Inventory.RemoveItem(item);
            }

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}