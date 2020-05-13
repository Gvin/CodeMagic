using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
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


        public bool Perform(out Point newPosition)
        {
            ((CurrentGame.GameCore<Player>)CurrentGame.Game).Player.Equipment.EquipItem(item);
            CurrentGame.Journal.Write(new ItemEquipedMessage(item));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}