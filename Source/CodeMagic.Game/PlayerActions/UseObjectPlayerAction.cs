using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UseObjectPlayerAction : IPlayerAction
    {
        public bool Perform(IGameCore game, out Point newPosition)
        {
            var playerLookPosition = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);

            var cell = game.Map.TryGetCell(playerLookPosition);
            var usableObject = cell?.Objects.OfType<IUsableObject>().FirstOrDefault();
            usableObject?.Use((GameCore<Player>)game, playerLookPosition);

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}