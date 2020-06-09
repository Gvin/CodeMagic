using CodeMagic.Core.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class EquipItemPlayerAction : PlayerActionBase
    {
        private readonly IEquipableItem item;

        public EquipItemPlayerAction(IEquipableItem item)
        {
            this.item = item;
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            ((GameCore<Player>)CurrentGame.Game).Player.Equipment.EquipItem(item);
            CurrentGame.Journal.Write(new ItemEquipedMessage(item));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}