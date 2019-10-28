using System;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Locations;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Game
{
    public class GameCore<TPlayer> : ITurnProvider, IGameCore where TPlayer : IPlayer
    {
        private readonly object worldLockObject = new object();
        private readonly GameTimeManager gameTimeManager;
        private Task updateTask;

        public GameCore(ILocation startingLocation, TPlayer player, Point playerPosition)
        {
            World = new GameWorld(startingLocation);

            PlayerPosition = playerPosition;
            Player = player;

            Map.AddObject(PlayerPosition, Player);

            Journal = new Journal(this);

            CurrentTurn = 1;
            gameTimeManager = new GameTimeManager();
        }

        public bool UpdateInProgress => updateTask != null;

        public int CurrentTurn { get; private set; }

        public GameWorld World { get; }

        public IAreaMap Map => World.CurrentLocation.CurrentArea;

        public TPlayer Player { get; }

        IPlayer IGameCore.Player => Player;

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
                if (UpdateInProgress)
                    return;

                updateTask = new Task(() =>
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

                    updateTask = null;
                });

                updateTask.Start();
                
            }
        }

        public void UpdatePlayerPosition(Point newPlayerPosition)
        {
            Map.AddObject(newPlayerPosition, Player);
            PlayerPosition = newPlayerPosition;
        }

        private void ProcessSystemTurn()
        {
            World.UpdateStoredLocations(gameTimeManager);

            gameTimeManager.RegisterTurn(World.CurrentLocation.TurnCycle);

            CurrentTurn++;
            UpdateMap();

            for (int counter = 0; counter < World.CurrentLocation.TurnCycle - 1; counter++)
            {
                CurrentTurn++;
                ((IDynamicObject)Player).Update(Map, Journal, PlayerPosition);
            }
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
            if (updateTask != null)
                return null;

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

        public void Dispose()
        {
            updateTask?.Wait();
            updateTask?.Dispose();
            World.Dispose();
        }
    }
}