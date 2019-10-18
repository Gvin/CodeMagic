using CodeMagic.Core.Common;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Game.PlayerActions
{
    public class MovePlayerAction : IPlayerAction
    {
        private readonly Direction direction;

        public MovePlayerAction(Direction direction)
        {
            this.direction = direction;
        }

        public bool Perform(IGameCore game, out Point newPosition)
        {
            if (game.Player.Statuses.Contains(ParalyzedObjectStatus.StatusType))
            {
                newPosition = game.PlayerPosition;
                game.Journal.Write(new ParalyzedMessage());
                return true;
            }

            if (game.Player.Statuses.Contains(OverweightObjectStatus.StatusType))
            {
                newPosition = game.PlayerPosition;
                game.Journal.Write(new OverweightBlocksMovementMessage());
                return true;
            }

            if (game.Player.Direction != direction)
            {
                game.Player.Direction = direction;
                newPosition = game.PlayerPosition;
                return false;
            }
            var moveResult = MovementHelper.MoveCreature(game.Player, game.Map, game.Journal, game.PlayerPosition, direction, true, true);
            if (moveResult.LocationEdge)
            {
                game.World.TravelToLocation(game, "world", direction);
                newPosition = game.PlayerPosition;
            }
            else
            {
                newPosition = moveResult.NewPosition;
            }
            return moveResult.Success;
        }
    }
}