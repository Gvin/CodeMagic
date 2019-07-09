using System.ComponentModel;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game.Journaling;
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

        public bool Perform(IPlayer player, Point playerPosition, IAreaMap map, Journal journal)
        {
            if (player.Direction != direction)
            {
                player.Direction = direction;
                return false;
            }

            var newCellPosition = Point.GetAdjustedPoint(playerPosition, direction);
            var newCell = SafeGetNewCell(newCellPosition, map);
            if (newCell == null || newCell.BlocksMovement)
                return true;

            var playerCell = map.GetCell(playerPosition);
            playerCell.Objects.Remove(player);
            newCell.Objects.Add(player);

            playerPosition.X = newCellPosition.X;
            playerPosition.Y = newCellPosition.Y;
            return true;
        }

        private AreaMapCell SafeGetNewCell(Point newCellPosition, IAreaMap map)
        {
            if (map.ContainsCell(newCellPosition))
            {
                return map.GetCell(newCellPosition);
            }

            return null;
        }
    }
}