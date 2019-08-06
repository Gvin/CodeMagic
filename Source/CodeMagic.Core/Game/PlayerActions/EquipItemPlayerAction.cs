using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class EquipItemPlayerAction : IPlayerAction
    {
        private readonly IEquipableItem item;

        public EquipItemPlayerAction(IEquipableItem item)
        {
            this.item = item;
        }


        public bool Perform(IGameCore game, out Point newPosition)
        {
            game.Player.Equipment.EquipItem(item);

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}