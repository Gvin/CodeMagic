using CodeMagic.Core.Game;
using CodeMagic.Game.Items;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class EquipHoldablePlayerAction : PlayerActionBase
    {
        private readonly IHoldableItem holdable;
        private readonly bool isRight;

        public EquipHoldablePlayerAction(IHoldableItem holdable, bool isRight)
        {
            this.holdable = holdable;
            this.isRight = isRight;
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            game.Player.Equipment.EquipHoldable(holdable, isRight);
            CurrentGame.Journal.Write(new HoldableEquippedMessage(holdable, isRight));

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}