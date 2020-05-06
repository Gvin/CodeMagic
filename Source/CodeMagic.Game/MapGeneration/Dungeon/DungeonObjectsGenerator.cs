using System;
using System.Collections.Generic;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Items.Materials;
using CodeMagic.Game.Objects.DecorativeObjects;

namespace CodeMagic.Game.MapGeneration.Dungeon
{
    internal partial class DungeonObjectsGenerator
    {
        private const double StonesCountMultiplier = 0.03;
        private const double TorchPostsCountMultiplier = 0.01;

        private const int MaxPositionSearchTries = 20;

        private readonly ObjectsPattern[] patterns;

        public DungeonObjectsGenerator(IImagesStorage storage)
        {
            patterns = GetPatterns(storage);
        }

        private static ObjectsPattern[] GetPatterns(IImagesStorage storage)
        {
            var patternsList = new List<ObjectsPattern>
            {
                // Table with 2 chairs
                CreateTableWithChairs(storage),
                // Single Shelf
                CreateShelfDown(storage),
                CreateShelfUp(storage),
                CreateShelfLeft(storage),
                CreateShelfRight(storage),
                // Crate
                CreateCrate(storage),
                // Chest
                CreateChest(storage),
                CreateGoldenChest(storage),
                // Water
                CreateWaterPool(storage)
            };

            return patternsList.ToArray();
        }

        public void GenerateObjects(IAreaMap map, bool addTorchPosts)
        {
            AddPatterns(map);

            var stonesCount = (int) Math.Round(map.Width * map.Height * StonesCountMultiplier);
            AddStones(map, stonesCount);

            if (addTorchPosts)
            {
                var torchPostsCount = (int) Math.Round(map.Width * map.Height * TorchPostsCountMultiplier);
                AddTorchPosts(map, torchPostsCount);
            }
        }

        private void AddPatterns(IAreaMap map)
        {
            foreach (var pattern in patterns)
            {
                for (int counter = 0; counter < pattern.MaxCount; counter++)
                {
                    if (RandomHelper.CheckChance(pattern.Rareness))
                    {
                        AddPattern(map, pattern);
                    }
                }
            }
        }

        private void AddPattern(IAreaMap map, ObjectsPattern pattern)
        {
            const int maxAttempts = 100;
            var attempt = 0;
            while (attempt < maxAttempts)
            {
                attempt++;

                if (TryAddPattern(map, pattern))
                    return;
            }
        }

        private bool TryAddPattern(IAreaMap map, ObjectsPattern pattern)
        {
            var x = RandomHelper.GetRandomValue(0, map.Width);
            var y = RandomHelper.GetRandomValue(0, map.Height);

            if (!map.ContainsCell(x, y))
                return false;

            for (int cursorX = 0; cursorX < pattern.Width; cursorX++)
            {
                for (int cursorY = 0; cursorY < pattern.Height; cursorY++)
                {
                    var posX = cursorX + x;
                    var posY = cursorY + y;

                    var cell = map.TryGetCell(posX, posY);
                    if (cell == null)
                        return false;

                    if (!pattern.CheckRequirements(cursorX, cursorY, cell))
                        return false;
                }
            }

            for (int cursorX = 0; cursorX < pattern.Width; cursorX++)
            {
                for (int cursorY = 0; cursorY < pattern.Height; cursorY++)
                {
                    var posX = cursorX + x;
                    var posY = cursorY + y;

                    foreach (var factory in pattern.Get(cursorX, cursorY))
                    {
                        map.AddObject(new Point(posX, posY), factory(map.Level));
                    }
                }
            }

            return true;
        }

        private void AddTorchPosts(IAreaMap map, int count)
        {
            for (int counter = 0; counter < count; counter++)
            {
                var position = GetFreePosition(map);
                if (position == null)
                    continue;

                map.AddObject(position, new DungeonTorchPost());
            }
        }

        private void AddStones(IAreaMap map, int stonesCount)
        {
            for (int counter = 0; counter < stonesCount; counter++)
            {
                var position = GetFreePosition(map);
                if (position == null)
                    continue;

                map.AddObject(position, new Stone());
            }
        }

        private Point GetFreePosition(IAreaMap map)
        {
            for (int counter = 0; counter < MaxPositionSearchTries; counter++)
            {
                var randomX = RandomHelper.GetRandomValue(0, map.Width - 1);
                var randomY = RandomHelper.GetRandomValue(0, map.Height - 1);

                var position = new Point(randomX, randomY);
                if (!map.GetCell(position).BlocksMovement)
                    return position;
            }
            return null;
        }
    }
}