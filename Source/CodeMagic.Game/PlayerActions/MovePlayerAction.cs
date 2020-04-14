using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;
using CodeMagic.Game.JournalMessages;
using CodeMagic.Game.Statuses;

namespace CodeMagic.Game.PlayerActions
{
    public class MovePlayerAction : IPlayerAction
    {
        private readonly Direction direction;

        public MovePlayerAction(Direction direction)
        {
            this.direction = direction;
        }

        public bool Perform(out Point newPosition)
        {
            if (CurrentGame.Player.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                newPosition = CurrentGame.PlayerPosition;
                CurrentGame.Journal.Write(new ParalyzedMessage());
                return true;
            }

            if (CurrentGame.Player.Statuses.Contains(OverweightObjectStatus.StatusType))
            {
                newPosition = CurrentGame.PlayerPosition;
                CurrentGame.Journal.Write(new OverweightBlocksMovementMessage());
                return true;
            }

            if (CurrentGame.Player.Direction != direction)
            {
                CurrentGame.Player.Direction = direction;
                newPosition = CurrentGame.PlayerPosition;
                return false;
            }
            var moveResult = MovementHelper.MoveCreature(CurrentGame.Player, CurrentGame.PlayerPosition, direction, true, true);
            newPosition = moveResult.NewPosition;
            return moveResult.Success;
        }
    }
}