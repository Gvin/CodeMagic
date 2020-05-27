using System;
using System.Collections.Generic;
using System.Diagnostics;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Logging;

namespace CodeMagic.Game.MapGeneration.Dungeon.ObjectsGenerators
{
    internal partial class DungeonObjectsGenerator : IObjectsGenerator
    {
        private static readonly ILog Log = LogManager.GetLog<DungeonObjectsGenerator>();

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
                CreateWaterPool(),
                // Spiked Floor
                CreateSpikedFloor(),
                // Stone
                CreateStone()
            };

            return patternsList.ToArray();
        }

        public void GenerateObjects(IAreaMap map, Point playerPosition)
        {
            var stopwatch = Stopwatch.StartNew();

            foreach (var pattern in patterns)
            {
                var count = (int)Math.Floor(map.Width * map.Height * pattern.MaxCountMultiplier);
                for (int counter = 0; counter < count; counter++)
                {
                    AddPattern(map, playerPosition, pattern);
                }
            }

            stopwatch.Stop();
            Log.Debug($"GenerateObjects took {stopwatch.ElapsedMilliseconds} milliseconds.");
        }

        private void AddPattern(IAreaMap map, Point playerPosition, ObjectsPattern pattern)
        {
            const int maxAttempts = 100;
            var attempt = 0;
            while (attempt < maxAttempts)
            {
                attempt++;

                if (TryAddPattern(map, playerPosition, pattern))
                    return;
            }
        }

        private bool TryAddPattern(IAreaMap map, Point playerPosition, ObjectsPattern pattern)
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

                    if (posX == playerPosition.X && posY == playerPosition.Y)
                        return false;

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
    }
}