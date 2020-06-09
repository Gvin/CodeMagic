using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class DropItemsPlayerAction : PlayerActionBase
    {
        private readonly IItem[] items;

        public DropItemsPlayerAction(params IItem[] items)
        {
            this.items = items.ToArray();
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
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