using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Game.Objects.SolidObjects;
using CodeMagic.Game.MapGeneration.Dungeon.MapGenerators;
using CodeMagic.Game.MapGeneration.Dungeon.MapObjectFactories;
using CodeMagic.Game.MapGeneration.Dungeon.MonstersGenerators;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.Game.MapGeneration.Dungeon
{
    public class DungeonMapGenerator : IDungeonMapGenerator
    {
        public static DungeonMapGenerator Current { get; private set; }

        public static void Initialize(IImagesStorage imagesStorage, bool writeMapFile)
        {
            Current = new DungeonMapGenerator(imagesStorage, writeMapFile);
        }

        private readonly Dictionary<MapType, IMapAreaGenerator> generators;
        private readonly bool writeMapFile;

        private DungeonMapGenerator(IImagesStorage imagesStorage, bool writeMapFile = false)
        {
            this.writeMapFile = writeMapFile;

            generators = new Dictionary<MapType, IMapAreaGenerator>
            {
                {MapType.Dungeon, new DungeonRoomsMapGenerator(
                    new DungeonMapObjectsFactory(), 
                    new ObjectsGenerators.DungeonObjectsGenerator(imagesStorage), 
                    new DungeonMonstersGenerator())},
                {MapType.Labyrinth, new LabyrinthMapGenerator(new DungeonMapObjectsFactory())},
                {MapType.Cave, new CaveDungeonMapGenerator(new CaveMapObjectsFactory())}
            };
        }

        public IAreaMap GenerateNewMap(int level, out Point playerPosition)
        {
            const int maxAttempts = 500;
            var attempts = 0;
            while (attempts < maxAttempts)
            {
                attempts++;
                try
                {
                    var size = GenerateMapSize();
                    return GenerateMap(level, size, out playerPosition);
                }
                catch (MapGenerationException)
                {
                    // Do nothing
                }
            }
            
            throw new ApplicationException("Unable to generate map.");
        }

        private MapSize GenerateMapSize()
        {
            var value = RandomHelper.GetRandomValue(0, 100);

            if (value >= 0 && value <= 25)
                return MapSize.Big; // 25%
            if (value > 25 && value <= 62)
                return MapSize.Medium; // 37%

            return MapSize.Small; // 38%
        }

        private IAreaMap GenerateMap(
            int level,
            MapSize size,
            out Point playerPosition)
        {
            var mapType = GenerateMapType(level);
            var generator = generators[mapType];
            var map = generator.Generate(level, size, out playerPosition);

            if (writeMapFile)
            {
                WriteMapToFile(map, playerPosition);
            }

            return map;
        }

        private void WriteMapToFile(IAreaMap map, Point playerPosition)
        {
            using (var file = File.CreateText(@".\Map.txt"))
            {
                for (int y = 0; y < map.Height; y++)
                {
                    var line = string.Empty;
                    for (int x = 0; x < map.Width; x++)
                    {
                        
                        line += GetCellSymbol(x, y, map, playerPosition);
                    }
                    file.WriteLine(line);
                }
            }
        }

        private string GetCellSymbol(int x, int y, IAreaMap map, Point playerPosition)
        {
            if (playerPosition.X == x && playerPosition.Y == y)
            {
                return "+";
            }

            var cell = map.GetCell(x, y);

            if (cell.Objects.OfType<DoorBase>().Any())
            {
                return "▒";
            }

            if (cell.Objects.OfType<WallBase>().Any())
            {
                return "█";
            }

            if (cell.Objects.OfType<DungeonTrapDoor>().Any())
            {
                return "v";
            }

            if (cell.Objects.OfType<DungeonStairs>().Any())
            {
                return "^";
            }

            return " ";
        }

        private MapType GenerateMapType(int level)
        {
            // TODO: Delete only-dungeon
            return MapType.Dungeon;

            if (level == 1) // Always dungeons for the 1st level.
                return MapType.Dungeon;

            var chance = RandomHelper.GetRandomValue(0, 100);
            if (chance < 20) // 20%
                return MapType.Labyrinth;
            if (chance >= 20 && chance < 30) // 10%
                return MapType.Cave;
            return MapType.Dungeon; // 70%
        }
    }
    public enum MapType
    {
        Dungeon,
        Labyrinth,
        Cave
    }

    public enum MapSize
    {
        Small,
        Medium,
        Big
    }
}