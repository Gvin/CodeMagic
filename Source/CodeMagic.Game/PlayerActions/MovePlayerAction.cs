using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Objects.Creatures;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.PlayerActions
{
    public class MovePlayerAction : PlayerActionBase
    {
        private readonly Direction direction;

        public MovePlayerAction(Direction direction)
        {
            this.direction = direction;
        }

        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            if (CurrentGame.Player.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                newPosition = CurrentGame.PlayerPosition;
                game.Journal.Write(new ParalyzedMessage());
                return true;
            }

            if (CurrentGame.Player.Statuses.Contains(OverweightObjectStatus.StatusType))
            {
                newPosition = game.PlayerPosition;
                game.Journal.Write(new OverweightBlocksMovementMessage());
                return true;
            }

            if (CurrentGame.Player.Direction != direction)
            {
                game.Player.Direction = direction;
                newPosition = game.PlayerPosition;
                return false;
            }
            var moveResult = MovementHelper.MoveCreature(game.Player, game.PlayerPosition, direction, true, true, false);
            newPosition = moveResult.NewPosition;
            return moveResult.Success;
        }
    }
}