﻿using System.Collections.Generic;
using System.IO;
using System.Linq;
using CodeMagic.Core.Area;
using CodeMagic.Core.Game;
using CodeMagic.Core.Objects.SolidObjects;
using CodeMagic.Implementations.Objects.SolidObjects;
using CodeMagic.MapGeneration.Dungeon.MapGenerators;
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

            var wallsFactory = new DungeonMapObjectsFactory(WallObjectConfiguration.WallType.Stone);
            generators = new Dictionary<MapType, IMapAreaGenerator>
            {
                {MapType.Dungeon, new DungeonRoomsMapGenerator(wallsFactory)},
                {MapType.Labyrinth, new LabyrinthMapGenerator(wallsFactory)}
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
            var mapType = GenerateMapType(level);
            var generator = generators[mapType];
            var map = generator.Generate(size, level == maxLevel, out playerPosition);

            new DungeonObjectsGenerator().GenerateObjects(map);

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

            if (cell.Objects.OfType<WallObject>().Any())
            {
                return "█";
            }

            if (cell.Objects.OfType<DoorObject>().Any())
            {
                return "▒";
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

        private MapType GenerateMapType(int level)
        {
            if (level == 1) // Always dungeons for the 1st level.
                return MapType.Dungeon;

            if (RandomHelper.CheckChance(20))
                return MapType.Labyrinth;
            return MapType.Dungeon;
        }
    }
    public enum MapType
    {
        Dungeon,
        Labyrinth
    }

    public enum MapSize
    {
        Small,
        Medium,
        Big
    }
}