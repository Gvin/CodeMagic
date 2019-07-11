using CodeMagic.Core.Common;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class MovePlayerAction : IPlayerAction
    {
        private readonly Direction direction;

        public MovePlayerAction(Direction direction)
        {
            this.direction = direction;
        }

        public bool Perform(IPlayer player, Point playerPosition, IGameCore game, out Point newPosition)
        {
            if (player.Direction != direction)
            {
                player.Direction = direction;
                newPosition = playerPosition;
                return false;
            }
            var moveResult = MovementHelper.MoveCreature(player, game, playerPosition, direction, true, true);
            newPosition = moveResult.NewPosition;
            return moveResult.Success;
        }
    }
}