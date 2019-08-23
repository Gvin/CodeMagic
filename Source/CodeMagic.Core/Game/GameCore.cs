using CodeMagic.Core.Area;
using CodeMagic.Core.Game.Journaling;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Game.PlayerActions;
using CodeMagic.Core.Objects.PlayerData;
using CodeMagic.Core.Statuses;

namespace CodeMagic.Core.Game
{
    public class GameCore : ITurnProvider, IGameCore
    {
        private readonly object mapLockObject = new object();
        private readonly IMapGenerator mapGenerator;
        private IAreaMap map;

        public GameCore(IMapGenerator mapGenerator, IPlayer player)
        {
            this.mapGenerator = mapGenerator;
            Player = player;

            Journal = new Journal(this);

            CurrentTurn = 1;
            Level = 1;

            Journal.Write(new DungeonLevelMessage(Level));

            GenerateNewMap();
        }

        private void GenerateNewMap()
        {
            lock (mapLockObject)
            {
                Map = mapGenerator.GenerateNewMap(Level, out var playerPosition);

                PlayerPosition = playerPosition;
                Map.AddObject(PlayerPosition, Player);
                Map.Refresh();
            }
        }

        public int CurrentTurn { get; private set; }

        public int Level { get; private set; }

        public IAreaMap Map
        {
            get
            {
                lock (mapLockObject)
                {
                    return map;
                }
            }
            private set => map = value;
        }

        public IPlayer Player { get; }

        public Journal Journal { get; }

        public Point PlayerPosition { get; private set; }

        public void PerformPlayerAction(IPlayerAction action)
        {
            lock (mapLockObject)
            {
                Map.PreUpdate(this);

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

        public void GoToNextLevel()
        {
            Level++;
            
            GenerateNewMap();

            Journal.Write(new DungeonLevelMessage(Level));
        }

        private void ProcessSystemTurn()
        {
            UpdateMap();
            CurrentTurn++;
        }

        private bool GetIfPlayerIsFrozen()
        {
            return Player.Statuses.Contains(FrozenObjectStatus.StatusType);
        }

        private void UpdateMap()
        {
            Map.Update(this);
        }

        public AreaMapFragment GetVisibleArea()
        {
            var visibleArea = VisibilityHelper.GetVisibleArea(Player.VisibilityRange, PlayerPosition, Map);
            if (Player.VisibilityRange == Player.MaxVisibilityRange)
                return visibleArea;

            var visibilityDifference = Player.MaxVisibilityRange - Player.VisibilityRange;
            var visibleAreaDiameter = Player.MaxVisibilityRange * 2 + 1;
            var result = new AreaMapCell[visibleAreaDiameter][];

            for (int y = 0; y < visibleAreaDiameter; y++)
            {
                result[y] = new AreaMapCell[visibleAreaDiameter];
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
    }
}