using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CodeMagic.Core.Area;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.Locations
{
    public class DungeonLocation : ILocation
    {
        private readonly List<StoredMap> levels;
        private readonly IDungeonMapGenerator dungeonMapGenerator;

        public DungeonLocation(ItemRareness rareness)
        {
            dungeonMapGenerator = Injector.Current.Create<IDungeonMapGenerator>();

            Rareness = rareness;
            CurrentLevel = 1;

            levels = new List<StoredMap>();

            Id = $"dungeon_{Guid.NewGuid()}";
        }

        public Point PlayerPosition { get; set; }

        public string Id { get; }

        public void Initialize(DateTime gameTime)
        {
            CurrentArea = dungeonMapGenerator.GenerateNewMap(CurrentLevel, out var newPlayerPosition);
            CurrentArea.Refresh(gameTime);
            levels.Add(new StoredMap(CurrentArea, null));
            PlayerPosition = newPlayerPosition;
        }

        public void MoveUp(IGameCore game)
        {
            levels[CurrentLevel - 1].PlayerPosition = game.PlayerPosition;

            CurrentLevel--;

            if (CurrentLevel == 0) // Exiting dungeon
            {
                game.World.TravelToLocation(game, "world");
                return;
            }

            var level = levels[CurrentLevel - 1];
            CurrentArea = level.Map;
            PlayerPosition = level.PlayerPosition;
        }

        public void MoveDown(IGameCore game)
        {
            levels[CurrentLevel - 1].PlayerPosition = game.PlayerPosition;

            CurrentLevel++;
            if (levels.Count >= CurrentLevel)
            {
                var level = levels[CurrentLevel - 1];
                CurrentArea = level.Map;
                PlayerPosition = level.PlayerPosition;
                return;
            }

            CurrentArea = dungeonMapGenerator.GenerateNewMap(CurrentLevel, out var newPlayerPosition);
            CurrentArea.Refresh(game.GameTime);
            levels.Add(new StoredMap(CurrentArea, null));
            PlayerPosition = newPlayerPosition;
        }

        private ItemRareness Rareness { get; }

        private int CurrentLevel { get; set; }

        public IAreaMap CurrentArea { get; private set; }

        public int TurnCycle => 1;

        public Task BackgroundUpdate(DateTime gameTime, int turnsCount)
        {
            throw new InvalidOperationException("Dungeon locations should not be updated in background.");
        }

        public bool KeepOnLeave => false;

        public bool CanCast => true;

        public bool CanFight => true;

        private class StoredMap
        {
            public StoredMap(IAreaMap map, Point playerPosition)
            {
                Map = map;
                PlayerPosition = playerPosition;
            }

            public IAreaMap Map { get; }

            public Point PlayerPosition { get; set; }
        }
    }
}