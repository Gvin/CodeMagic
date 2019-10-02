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

        public bool CanBuild => false;

        private int GetMaxLevel(ItemRareness rareness)
        {
            return (int) rareness * 2 + 1;
        }

        private ItemRareness GetLevelRareness(int level)
        {
            switch (level)
            {
                case 1:
                    return ItemRareness.Trash;
                case 2:
                case 3:
                    return ItemRareness.Common;
                case 4:
                case 5:
                    return ItemRareness.Uncommon;
                default:
                    return ItemRareness.Rare;
            }
        }

        public void Initialize(DateTime gameTime)
        {
            PlayerPosition = GenerateNewLevel(gameTime);
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

            PlayerPosition = GenerateNewLevel(game.GameTime);
            game.UpdatePlayerPosition(PlayerPosition);

            game.Journal.Write(new DungeonLevelMessage(CurrentLevel));
        }

        private Point GenerateNewLevel(DateTime gameTime)
        {
            var maxLevel = GetMaxLevel(Rareness);
            var rareness = GetLevelRareness(CurrentLevel);
            CurrentArea = dungeonMapGenerator.GenerateNewMap(CurrentLevel, rareness, maxLevel, out var newPlayerPosition);
            CurrentArea.Refresh(gameTime);
            levels.Add(new StoredMap(CurrentArea, null));
            return newPlayerPosition;
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