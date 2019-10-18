using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.PlayerActions
{
    public class DropItemsPlayerAction : IPlayerAction
    {
        private readonly IItem[] items;

        public DropItemsPlayerAction(params IItem[] items)
        {
            this.items = items.ToArray();
        }

        public bool Perform(IGameCore game, out Point newPosition)
        {
            newPosition = game.PlayerPosition;

            foreach (var item in items)
            {
                game.Map.AddObject(game.PlayerPosition, item);
                game.Player.Inventory.RemoveItem(item);
            }

            return true;
        }
    }
}