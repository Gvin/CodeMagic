using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UseObjectPlayerAction : IPlayerAction
    {
        public bool Perform(out Point newPosition)
        {
            var playerLookPosition = Point.GetPointInDirection(CurrentGame.PlayerPosition, CurrentGame.Player.Direction);

            var cell = CurrentGame.Map.TryGetCell(playerLookPosition);
            var usableObject = cell?.Objects.OfType<IUsableObject>().FirstOrDefault();
            usableObject?.Use((CurrentGame.GameCore<Player>)CurrentGame.Game, playerLookPosition);

            newPosition = CurrentGame.PlayerPosition;
            return true;
        }
    }
}