using System.Linq;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects;
using CodeMagic.Game.Objects.Creatures;

namespace CodeMagic.Game.PlayerActions
{
    public class UseObjectPlayerAction : PlayerActionBase
    {
        protected override int RestoresStamina => 10;

        protected override bool Perform(GameCore<Player> game, out Point newPosition)
        {
            var playerLookPosition = Point.GetPointInDirection(game.PlayerPosition, game.Player.Direction);

            var cell = game.Map.TryGetCell(playerLookPosition);
            var usableObject = cell?.Objects.OfType<IUsableObject>().FirstOrDefault();
            usableObject?.Use(game, playerLookPosition);

            newPosition = game.PlayerPosition;
            return true;
        }
    }
}