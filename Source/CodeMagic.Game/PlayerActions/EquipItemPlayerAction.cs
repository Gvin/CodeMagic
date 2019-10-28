using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Items;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
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
            ((GameCore<Player>) game).Player.Equipment.EquipItem(item);

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}