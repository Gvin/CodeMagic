using System;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects.PlayerData;

namespace CodeMagic.Core.Game
{
    public class GameCore : ITurnProvider, IGameCore
    {
        public GameCore(IAreaMap map, Point playerPosition)
        {
            Map = map;
            PlayerPosition = playerPosition;
            Player = map.GetCell(playerPosition).Objects.First(obj => obj is IPlayer) as IPlayer;

            if (Player == null)
                throw new ArgumentException("Player is not located on map in specified location.", nameof(playerPosition));

            Journal = new Journal(this);

            CurrentTurn = 1;
        }

        public int CurrentTurn { get; private set; }

        public IAreaMap Map { get; }

        public IPlayer Player { get; }

        public Journal Journal { get; }

        public Point PlayerPosition { get; private set; }

        public void PerformPlayerAction(IPlayerAction action)
        {
            var endsTurn = action.Perform(Player, PlayerPosition, this, out var newPosition);
            PlayerPosition = newPosition;
            if (endsTurn)
            {
                UpdateMap();
                CurrentTurn++;
            }
        }

        private void UpdateMap()
        {
            Map.Update(this);
        }

        public AreaMapFragment GetVisibleArea()
        {
            return VisibilityHelper.GetVisibleArea(Player.VisionRange, PlayerPosition, Map);
        }
    }
}