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

        public bool Perform(out Point newPosition)
        {
            newPosition = CurrentGame.PlayerPosition;

            foreach (var item in items)
            {
                CurrentGame.Map.AddObject(CurrentGame.PlayerPosition, item);
                CurrentGame.Player.Inventory.RemoveItem(item);
            }

            return true;
        }
    }
}