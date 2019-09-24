using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Common;
using CodeMagic.Core.Game.Journaling.Messages;
using CodeMagic.Core.Injection;
using CodeMagic.Core.Items;

namespace CodeMagic.Core.Game.Locations
{
    public class DungeonLocation : ILocation
    {
        private readonly List<StoredMap> levels;
        private readonly IDungeonMapGenerator dungeonMapGenerator;
        private Direction enterDirection;

        public DungeonLocation(ItemRareness rareness)
        {
            dungeonMapGenerator = Injector.Current.Create<IDungeonMapGenerator>();

            Rareness = rareness;
            CurrentLevel = 1;

            levels = new List<StoredMap>();

            Id = $"dungeon_{Guid.NewGuid()}";
        }

        private Point PlayerPosition { get; set; }

        public string Id { get; }

        public string Name => "Dungeon";

        private int GetMaxLevel(ItemRareness rareness)
        {
            return (int) rareness * 2 + 1;
        }

        public void Initialize(DateTime gameTime)
        {
            var maxLevel = GetMaxLevel(Rareness);
            CurrentArea = dungeonMapGenerator.GenerateNewMap(CurrentLevel, maxLevel, out var newPlayerPosition);
            CurrentArea.Refresh(gameTime);
            levels.Add(new StoredMap(CurrentArea, null));
            PlayerPosition = newPlayerPosition;
        }

        public void MoveUp(IGameCore game)
        {
            game.RemovePlayerFromMap();

            levels[CurrentLevel - 1].PlayerPosition = game.PlayerPosition;

            CurrentLevel--;

            if (CurrentLevel == 0) // Exiting dungeon
            {
                game.World.TravelToLocation(game, "world", DirectionHelper.InvertDirection(enterDirection));
                return;
            }

            var level = levels[CurrentLevel - 1];
            CurrentArea = level.Map;
            PlayerPosition = level.PlayerPosition;

            game.UpdatePlayerPosition(PlayerPosition);

            game.Journal.Write(new DungeonLevelMessage(CurrentLevel));
        }

        public void MoveDown(IGameCore game)
        {
            game.RemovePlayerFromMap();

            levels[CurrentLevel - 1].PlayerPosition = game.PlayerPosition;

            CurrentLevel++;
            if (levels.Count >= CurrentLevel)
            {
                var level = levels[CurrentLevel - 1];
                CurrentArea = level.Map;
                PlayerPosition = level.PlayerPosition;
                game.UpdatePlayerPosition(PlayerPosition);
                return;
            }

            var maxLevel = GetMaxLevel(Rareness);
            CurrentArea = dungeonMapGenerator.GenerateNewMap(CurrentLevel, maxLevel, out var newPlayerPosition);
            CurrentArea.Refresh(game.GameTime);
            levels.Add(new StoredMap(CurrentArea, null));
            PlayerPosition = newPlayerPosition;
            game.UpdatePlayerPosition(PlayerPosition);

            game.Journal.Write(new DungeonLevelMessage(CurrentLevel));
        }

        private ItemRareness Rareness { get; }

        private int CurrentLevel { get; set; }

        public IAreaMap CurrentArea { get; private set; }

        public int TurnCycle => 1;

        public void ProcessPlayerEnter(IGameCore game)
        {
            game.Journal.Write(new DungeonLevelMessage(CurrentLevel));
            enterDirection = game.Player.Direction;
        }

        public void ProcessPlayerLeave(IGameCore game)
        {
            // Do nothing
        }

        public Point GetEnterPoint(Direction direction)
        {
            return PlayerPosition;
        }

        public void BackgroundUpdate(DateTime gameTime)
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