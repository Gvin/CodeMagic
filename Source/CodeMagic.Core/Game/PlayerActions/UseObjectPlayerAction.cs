﻿using System.Linq;
using CodeMagic.Core.Objects;

namespace CodeMagic.Core.Game.PlayerActions
{
    public class UseObjectPlayerAction : IPlayerAction
    {
        public bool Perform(IGameCore game, out Point newPosition)
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