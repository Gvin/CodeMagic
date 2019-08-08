using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.PlayerActions
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