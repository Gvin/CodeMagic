using System;
using System.Diagnostics;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Game
{
    public class GameCore : ITurnProvider, IGameCore
    {
        private readonly object worldLockObject = new object();
        private readonly GameTimeManager gameTimeManager;

        public GameCore(ILocation startingLocation, IPlayer player)
        {
            World = new GameWorld(startingLocation);

            PlayerPosition = startingLocation.PlayerPosition;
            Player = player;

            Map.AddObject(PlayerPosition, Player);

            Journal = new Journal(this);

            CurrentTurn = 1;
            gameTimeManager = new GameTimeManager();
        }

        public int CurrentTurn { get; private set; }

        public GameWorld World { get; }

        public IAreaMap Map => World.CurrentLocation.CurrentArea;

        public IPlayer Player { get; }

        public Journal Journal { get; }

        public Point PlayerPosition { get; private set; }

        public DateTime GameTime
        {
            get
            {
                lock (worldLockObject)
                {
                    return gameTimeManager.CurrentTime;
                }
            }
        }

        public void PerformPlayerAction(IPlayerAction action)
        {
            lock (worldLockObject)
            {
                Map.PreUpdate(Journal);

                var endsTurn = action.Perform(this, out var newPosition);
                PlayerPosition = newPosition;

                if (endsTurn)
                {
                    ProcessSystemTurn();

                    if (GetIfPlayerIsFrozen())
                    {
                        ProcessSystemTurn();
                    }
                }
            }
        }

        public void UpdatePlayerPosition(Point newPlayerPosition)
        {
            Map.AddObject(newPlayerPosition, Player);
            PlayerPosition = newPlayerPosition;
        }

        private void ProcessSystemTurn()
        {
            CurrentTurn += World.CurrentLocation.TurnCycle;
            gameTimeManager.RegisterTurn(World.CurrentLocation.TurnCycle);

            var backgroundUpdateTask = World.UpdateStoredLocations(GameTime);

            UpdateMap();

            backgroundUpdateTask.Wait();
        }

        private bool GetIfPlayerIsFrozen()
        {
            return Player.Statuses.Contains(FrozenObjectStatus.StatusType);
        }

        private void UpdateMap()
        {
            Map.Update(Journal, GameTime);
        }

        public AreaMapFragment GetVisibleArea()
        {
            var visibleArea = VisibilityHelper.GetVisibleArea(Player.VisibilityRange, PlayerPosition, World.CurrentLocation.CurrentArea);
            if (Player.VisibilityRange == Player.MaxVisibilityRange)
                return visibleArea;

            var visibilityDifference = Player.MaxVisibilityRange - Player.VisibilityRange;
            var visibleAreaDiameter = Player.MaxVisibilityRange * 2 + 1;
            var result = new IAreaMapCell[visibleAreaDiameter][];

            for (int y = 0; y < visibleAreaDiameter; y++)
            {
                result[y] = new IAreaMapCell[visibleAreaDiameter];
            }

            for (int y = 0; y < visibleArea.Height; y++)
            {
                for (int x = 0; x < visibleArea.Width; x++)
                {
                    var visibleAreaY = y + visibilityDifference;
                    var visibleAreaX = x + visibilityDifference;
                    result[visibleAreaY][visibleAreaX] = visibleArea.GetCell(x, y);
                }
            }

            return new AreaMapFragment(result, visibleAreaDiameter, visibleAreaDiameter);
        }

        public void RemovePlayerFromMap()
        {
            Map.RemoveObject(PlayerPosition, Player);
        }
    }
}