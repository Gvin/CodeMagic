using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UseItemPlayerAction : PlayerActionBase
    {
        private readonly IUsableItem item;

        public UseItemPlayerAction(IUsableItem item)
        {
            this.item = item;
        }

        protected override int RestoresStamina => 20;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
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