using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;

namespace CodeMagic.Game.PlayerActions
{
    public class UnequipItemPlayerAction : IPlayerAction
    {
        private readonly IEquipableItem item;

        public UnequipItemPlayerAction(IEquipableItem item)
        {
            this.item = item;
        }

        public bool Perform(IGameCore game, out Point newPosition)
        {
            game.Player.Equipment.UnequipItem(item);

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}