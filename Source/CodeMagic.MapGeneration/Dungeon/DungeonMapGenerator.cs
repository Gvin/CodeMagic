using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.MapGeneration.Dungeon.MapGenerators;
using CodeMagic.MapGeneration.Dungeon.MapObjectFactories;
using Point = CodeMagic.Core.Game.Point;

namespace CodeMagic.MapGeneration.Dungeon
{
    public class DungeonMapGenerator : IDungeonMapGenerator
    {
        private readonly Dictionary<MapType, IMapAreaGenerator> generators;
        private readonly bool writeMapFile;

        public DungeonMapGenerator(bool writeMapFile = false)
        {
            this.writeMapFile = writeMapFile;

            var dungeonMapObjectsFactory = new DungeonMapObjectsFactory();
            var caveMapObjectsFactory = new CaveMapObjectsFactory();

            generators = new Dictionary<MapType, IMapAreaGenerator>
            {
                {MapType.Dungeon, new DungeonRoomsMapGenerator(dungeonMapObjectsFactory)},
                {MapType.Labyrinth, new LabyrinthMapGenerator(dungeonMapObjectsFactory)},
                {MapType.Cave, new CaveDungeonMapGenerator(caveMapObjectsFactory)}
            };
        }

        public IAreaMap GenerateNewMap(int level, int maxLevel, out Point playerPosition)
        {
            var size = GenerateMapSize();
            return GenerateMap(level, maxLevel, size, out playerPosition);
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
            int maxLevel,
            MapSize size,
            out Point playerPosition)
        {
            var lastLevel = level == maxLevel;
            var mapType = GenerateMapType(level, lastLevel);
            var generator = generators[mapType];
            var map = generator.Generate(size, lastLevel, out playerPosition);

            var generateTorchPosts = mapType == MapType.Cave;

            new DungeonObjectsGenerator().GenerateObjects(map, generateTorchPosts);

            new DungeonNpcGenerator().GenerateNpc(level, size, mapType, map, playerPosition);

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

            if (cell.Objects.OfType<DoorObject>().Any())
            {
                return "▒";
            }

            if (cell.Objects.OfType<WallBase>().Any())
            {
                return "█";
            }

            if (cell.Objects.OfType<TrapDoorObject>().Any())
            {
                return "v";
            }

            if (cell.Objects.OfType<StairsObject>().Any())
            {
                return "^";
            }

            if (cell.Objects.OfType<ExitPortalObject>().Any())
            {
                return "#";
            }

            return " ";
        }

        private MapType GenerateMapType(int level, bool lastLevel)
        {
            if (lastLevel) // Always caves for the last level.
                return MapType.Cave;

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